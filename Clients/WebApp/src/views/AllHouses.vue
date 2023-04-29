<script setup>
import axios from 'axios'
import { onMounted, reactive, ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import currency from 'currency.js'

const router = useRouter()
let allHouses = reactive([])

async function loadAllHouses() {
  try {
    Object.assign(allHouses, [])
    const response = await axios.get(`${import.meta.env.VITE_BASE_URL_HOUSE_MANAGE_API}api/houses`)
    console.log(response)
    if (response.status === 200) {
      Object.assign(allHouses, response.data)
    }
  } catch (error) {
    console.error(error)
  }
}

onMounted(loadAllHouses)
</script>

<template>
  <div class="all_houses">
    <!-- grid container -->
    <div
      v-if="allHouses && allHouses.length"
      v-for="house in allHouses"
      :key="house.id"
      class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-x-6 gap-y-10 mt-8 mx-6"
    >
      <!-- product card -->
      <a
        href="#"
        class="flex flex-col bg-white drop-shadow hover:drop-shadow-lg hover:opacity-70 rounded-md select-text"
      >
        <img
          src=" https://www.kindacode.com/wp-content/uploads/2022/07/kindacode-example.png"
          alt="Fiction Product"
          class="h-36 object-cover rounded-tl-md rounded-tr-md"
        />

        <div class="px-3 py-2">
          <!-- <pre>{{ house }}</pre> -->
          <h1 class="font-semibold">{{ house.location }}</h1>
          <div class="section">
            <div class="flex pb-2 pt-5">
              <div class="mr-2">Location:</div>
              <div>{{ house.location }}</div>
            </div>
            <div class="text-sm pb-2">
              <div class="flex">
                <div class="mr-2">Price in USD:</div>
                <div>
                  {{
                    currency(house.predictedPrice, {
                      separator: ',',
                      precision: 2,
                      symbol: '$ '
                    }).format()
                  }}
                </div>
              </div>
            </div>
            <div class="text-sm pb-2">
              <div class="flex">
                <div class="mr-2">Price in LKR:</div>
                <div>
                  {{
                    currency(house.predictedPriceLKR, {
                      separator: ',',
                      precision: 2,
                      symbol: 'Rs '
                    }).format()
                  }}
                </div>
              </div>
            </div>
          </div>
        </div>
      </a>
      <!-- product card end -->
    </div>
  </div>
</template>

<style lang="scss" scoped>
.all_houses {
}
</style>
