<script setup>
import { reactive, ref, computed, onMounted, initCustomFormatter } from 'vue'
import axios from 'axios'
import { useAuthStore } from '../stores/auth'
import { storeToRefs } from 'pinia'
import currency from 'currency.js'
import { useRouter } from 'vue-router'
import { useToast } from 'vue-toast-notification'
import 'vue-toast-notification/dist/theme-sugar.css'

const router = useRouter()
const toast = useToast({
  position: 'top-right'
})
let form = reactive({
  OverallQual: {
    value: '',
    label: 'Overall quality',
    description: 'Rates the overall material and finish of the house',
    type: 'Number'
  },
  YearBuilt: {
    value: '',
    label: 'Construction year',
    description: 'Original construction year',
    type: 'Number'
  },
  YearRemodAdd: {
    value: '',
    label: 'Remodel year',
    description: 'Remodel year (same as construction year if no remodeling or additions)',
    type: 'Number'
  },
  TotalBsmtSF: {
    value: '',
    label: 'Total square feet',
    description: 'Total square feet of basement area',
    type: 'Number'
  },
  A1stFlrSF: {
    value: '',
    label: 'First Floor square feet',
    description: 'First Floor square feet',
    type: 'Number'
  },
  GrLivArea: {
    value: '',
    label: 'Above living area square feet',
    description: 'Above grade (ground) living area square feet',
    type: 'Number'
  },
  FullBath: {
    value: '',
    label: 'Number of bathrooms',
    description: 'Full bathrooms above grade',
    type: 'Number'
  },
  TotRmsAbvGrd: {
    value: '',
    label: 'Number of rooms',
    description: 'Total rooms above grade (does not include bathrooms)',
    type: 'Number'
  },
  GarageCars: {
    value: '',
    label: 'Garage vehicles count',
    description: 'Size of garage in car capacity',
    type: 'Number'
  },
  GarageArea: {
    value: '',
    label: 'Size of garage',
    description: 'Size of garage in square feet',
    type: 'Number'
  },
  Location: {
    value: '',
    label: 'Location',
    description: 'Location of the house',
    type: 'Text'
  }
})

let errorsInitialVal = {
  OverallQual: {
    errors: []
  },
  YearBuilt: {
    errors: []
  },
  YearRemodAdd: {
    errors: []
  },
  TotalBsmtSF: {
    errors: []
  },
  A1stFlrSF: {
    errors: []
  },
  GrLivArea: {
    errors: []
  },
  FullBath: {
    errors: []
  },
  TotRmsAbvGrd: {
    errors: []
  },
  GarageCars: {
    errors: []
  },
  GarageArea: {
    errors: []
  },
  Location: {
    errors: []
  }
}

const houseValue = ref(0)
const houseValueLKR = ref(0)
const isImageUploaderError = ref(false)

let formErrors = reactive(JSON.parse(JSON.stringify(errorsInitialVal)))
let toastInstance = ref({})

async function predictHouseValue() {
  toastInstance.value = toast.info('please wait we are processing your request...', {
    duration: 6000
  })
  try {
    const data = {
      OverallQual: form.OverallQual.value,
      YearBuilt: form.YearBuilt.value,
      YearRemodAdd: form.YearRemodAdd.value,
      TotalBsmtSF: form.TotalBsmtSF.value,
      A1stFlrSF: form.A1stFlrSF.value,
      GrLivArea: form.GrLivArea.value,
      FullBath: form.FullBath.value,
      TotRmsAbvGrd: form.TotRmsAbvGrd.value,
      GarageCars: form.GarageCars.value,
      GarageArea: form.GarageArea.value
    }
    const dataList = [data]
    const response = await axios.post(
      `${import.meta.env.VITE_BASE_URL_HOUSE_MANAGE_API}api/Houses/GetValuePrediction`,
      dataList
    )
    houseValue.value = response?.data?.predictions[0]
    if (import.meta.env.VITE_ENABLE_LKR_COVERTION === 'true') {
      houseValueLKR.value = await getLRKValue(houseValue.value)
    }
    setTimeout(() => {
      toastInstance.value.dismiss()
      window.scrollTo({ top: 0, behavior: 'smooth' })
    }, 1000)
  } catch (error) {
    toastInstance.value.dismiss()
    toastInstance.value = toast.error('Something went wrong, please try again later!')
    console.error(error)
  }
}
async function getLRKValue(usdValue) {
  try {
    if (!usdValue) return 0

    const response = await axios.get(
      `https://api.apilayer.com/exchangerates_data/convert?to=LKR&from=USD&amount=${usdValue}`,
      {
        headers: {
          apikey: import.meta.env.VITE_EXCHANGERATES_DATA.toString()
        }
      }
    )
    console.log(response.data.result)
    return response.data.result
  } catch (error) {
    console.error(error)
  }
}
async function listNewHouse() {
  try {
    if (media.added.length <= 0) {
      isImageUploaderError.value = true
      toastInstance.value = toast.warning('Please upload at least one photo of the house.', {
        duration: 4000
      })
      return
    }

    toastInstance.value = toast.info('please wait we are processing your request...', {
      duration: 6000
    })
    const HousePhotos = []
    media.added.forEach((m) => {
      HousePhotos.push({
        Url: `${import.meta.env.VITE_BASE_MEDIA_SERVER_URL}${m.name}`,
        Name: m.name
      })
    })
    const data = {
      location: form.Location.value,
      overallQual: form.OverallQual.value,
      yearBuilt: form.YearBuilt.value,
      yearRemodAdd: form.YearRemodAdd.value,
      totalBsmtSF: form.TotalBsmtSF.value,
      a1stFlrSF: form.A1stFlrSF.value,
      grLivArea: form.GrLivArea.value,
      fullBath: form.FullBath.value,
      totRmsAbvGrd: form.TotRmsAbvGrd.value,
      garageCars: form.GarageCars.value,
      garageArea: form.GarageArea.value,
      predictedPrice: houseValue.value,
      PredictedPriceLKR: houseValueLKR.value,
      HousePhotos
    }

    const response = await axios.post(
      `${import.meta.env.VITE_BASE_URL_HOUSE_MANAGE_API}api/houses`,
      data
    )
    const responseListing = await axios.post(
      `${import.meta.env.VITE_BASE_URL_HOUSE_MANAGE_API}api/Listings`,
      {
        HouseId: response.data.id,
        Description: '',
        IsFeatured: true
      }
    )
    toastInstance.value.dismiss()
    if (response.status === 200 || response.status === 201) {
      toastInstance.value = toast.success('House listed successfully.')
      setTimeout(() => {
        router.push({ name: 'allHouses' })
      }, 1500)
    }
  } catch (error) {
    toastInstance.value.dismiss()
    toastInstance.value = toast.error('Something went wrong, please try again later!')
    console.error(error)
  }
}

const uploadPath = ref(`${import.meta.env.VITE_BASE_URL_HOUSE_MANAGE_API}api/HousePhotos/Upload`)
let media = reactive({
  saved: [],
  added: [],
  removed: []
})

function changeMedia(media) {
  console.log(media)
}

function addMedia(_, addedMedia) {
  isImageUploaderError.value = false
  media.added = addedMedia
}
function removeMedia(_, removedMedia) {
  isImageUploaderError.value = false
  media.removed = removedMedia
}
</script>

<template>
  <div class="new_house flex justify-center items-center">
    <div class="w-full max-w-6xl mt-5">
      <div class="flex items-center pt-5 pb-3 shadow-md">
        <div class="prediction_results">
          <div class="text-xl pl-8">
            <div>Predicted Price</div>
            <div>
              USD:
              <span class="font-bold">{{
                currency(houseValue, { separator: ',', precision: 2, symbol: '$ ' }).format()
              }}</span>
            </div>
            <div>
              LKR:
              <span class="font-bold">{{
                currency(houseValueLKR, { separator: ',', precision: 2, symbol: 'Rs ' }).format()
              }}</span>
            </div>
          </div>
        </div>
        <button
          class="ml-14 bg-green-700 hover:bg-green-800 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
          type="button"
          @click.native="listNewHouse"
          v-if="houseValue"
        >
          List House Now
        </button>
      </div>
      <label v-if="houseValue" class="block text-gray-700 text-sm font-bold mt-4 mb-2 ml-3">
        Upload house photos
      </label>
      <Uploader
        v-if="houseValue"
        :class="['mt-5 mb-3 mx-3', { 'border-red-500 border-2': isImageUploaderError }]"
        :server="uploadPath"
        @change="changeMedia"
        @add="addMedia"
        @remove="removeMedia"
      />
      <form
        class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4"
        @submit.prevent="predictHouseValue"
      >
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" :for="form.Location.label">
            {{ form.Location.label }}
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.Location.errors.length ? 'border-red-500' : ''"
            id="Location"
            :type="form.Location.type"
            :placeholder="form.Location.description"
            v-model="form.Location.value"
            autofocus
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.Location.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" :for="form.OverallQual.label">
            {{ form.OverallQual.label }}
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.OverallQual.errors.length ? 'border-red-500' : ''"
            id="OverallQual"
            :type="form.OverallQual.type"
            :placeholder="form.OverallQual.description"
            v-model="form.OverallQual.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.OverallQual.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" :for="form.YearBuilt.label">
            {{ form.YearBuilt.label }}
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.YearBuilt.errors.length ? 'border-red-500' : ''"
            id="YearBuilt"
            :type="form.YearBuilt.type"
            :placeholder="form.YearBuilt.description"
            v-model="form.YearBuilt.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.YearBuilt.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" :for="form.YearRemodAdd.label">
            {{ form.YearRemodAdd.label }}
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.YearRemodAdd.errors.length ? 'border-red-500' : ''"
            id="YearRemodAdd"
            :type="form.YearRemodAdd.type"
            :placeholder="form.YearRemodAdd.description"
            v-model="form.YearRemodAdd.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.YearRemodAdd.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" :for="form.TotalBsmtSF.label">
            {{ form.TotalBsmtSF.label }}
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.TotalBsmtSF.errors.length ? 'border-red-500' : ''"
            id="TotalBsmtSF"
            :type="form.TotalBsmtSF.type"
            :placeholder="form.TotalBsmtSF.description"
            v-model="form.TotalBsmtSF.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.TotalBsmtSF.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" :for="form.A1stFlrSF.label">
            {{ form.A1stFlrSF.label }}
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.A1stFlrSF.errors.length ? 'border-red-500' : ''"
            id="A1stFlrSF"
            :type="form.A1stFlrSF.type"
            :placeholder="form.A1stFlrSF.description"
            v-model="form.A1stFlrSF.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.A1stFlrSF.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" :for="form.GrLivArea.label">
            {{ form.GrLivArea.label }}
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.GrLivArea.errors.length ? 'border-red-500' : ''"
            id="GrLivArea"
            :type="form.GrLivArea.type"
            :placeholder="form.GrLivArea.description"
            v-model="form.GrLivArea.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.GrLivArea.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" :for="form.FullBath.label">
            {{ form.FullBath.label }}
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.FullBath.errors.length ? 'border-red-500' : ''"
            id="FullBath"
            :type="form.FullBath.type"
            :placeholder="form.FullBath.description"
            v-model="form.FullBath.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.FullBath.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" :for="form.TotRmsAbvGrd.label">
            {{ form.TotRmsAbvGrd.label }}
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.TotRmsAbvGrd.errors.length ? 'border-red-500' : ''"
            id="TotRmsAbvGrd"
            :type="form.TotRmsAbvGrd.type"
            :placeholder="form.TotRmsAbvGrd.description"
            v-model="form.TotRmsAbvGrd.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.TotRmsAbvGrd.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" :for="form.GarageCars.label">
            {{ form.GarageCars.label }}
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.GarageCars.errors.length ? 'border-red-500' : ''"
            id="GarageCars"
            :type="form.GarageCars.type"
            :placeholder="form.GarageCars.description"
            v-model="form.GarageCars.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.GarageCars.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" :for="form.GarageArea.label">
            {{ form.GarageArea.label }}
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.GarageArea.errors.length ? 'border-red-500' : ''"
            id="GarageArea"
            :type="form.GarageArea.type"
            :placeholder="form.GarageArea.description"
            v-model="form.GarageArea.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.GarageArea.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>

        <div class="flex items-center justify-between">
          <button
            class="mt-2 bg-[#662ac5] hover:bg-[#6d33ca] text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
            type="submit"
          >
            Get prediction
          </button>
        </div>
      </form>
      <p class="text-center text-gray-500 text-xs">&copy;2023 HouseX. All rights reserved.</p>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.new_house {
}
</style>
