<script setup>
import { reactive, ref, computed, onMounted, initCustomFormatter } from 'vue'
import axios from 'axios'
import { useAuthStore } from '../stores/auth'
import { storeToRefs } from 'pinia'

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
  }
}

const houseValue = ref(0)

let formErrors = reactive(JSON.parse(JSON.stringify(errorsInitialVal)))

async function predictHouseValue() {
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
    window.scrollTo({ top: 0, behavior: 'smooth' })
  } catch (error) {
    console.error(error)
  }
}
</script>

<template>
  <div class="new_house flex justify-center items-center">
    <div class="w-full max-w-6xl mt-5">
      <div class="prediction_results">
        <p class="text-xl pt-5 shadow-md pl-8">
          Predicted Price LKR: <span class="font-bold">{{ houseValue }}</span>
        </p>
      </div>
      <form
        class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4"
        @submit.prevent="predictHouseValue"
      >
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
            autofocus
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
