<script setup>
import axios from 'axios'
import mgr from '../../services/security'
import { ref, reactive, computed } from 'vue'

const userInfo = ref('')
const isEditMode = ref(false)

async function onCreated() {
  try {
    // const user = await mgr.getUser()
    // const userJson = JSON.parse(JSON.stringify(user))

    const response = await axios.get(`${import.meta.env.VITE_BASE_URL_HOUSE_MANAGE_API}api/Users`)
    userInfo.value = response.data
  } catch (error) {
    console.error(error)
  }
}

const fullName = computed(() => {
  if (!userInfo) return ''
  return `${userInfo.value.given_name} ${userInfo.value.family_name}`
})

onCreated()
</script>

<template>
  <div class="user_profile">
    <link
      rel="stylesheet"
      href="https://demos.creative-tim.com/notus-js/assets/styles/tailwind.css"
    />
    <link
      rel="stylesheet"
      href="https://demos.creative-tim.com/notus-js/assets/vendor/@fortawesome/fontawesome-free/css/all.min.css"
    />

    <main class="profile-page">
      <section class="relative block h-500-px">
        <div
          class="absolute top-0 w-full h-full bg-center bg-cover"
          style="
            background-image: url('https://images.pexels.com/photos/1396122/pexels-photo-1396122.jpeg');
          "
        >
          <span id="blackOverlay" class="w-full h-full absolute opacity-50 bg-black"></span>
        </div>
        <div
          class="top-auto bottom-0 left-0 right-0 w-full absolute pointer-events-none overflow-hidden h-70-px"
          style="transform: translateZ(0px)"
        >
          <svg
            class="absolute bottom-0 overflow-hidden"
            xmlns="http://www.w3.org/2000/svg"
            preserveAspectRatio="none"
            version="1.1"
            viewBox="0 0 2560 100"
            x="0"
            y="0"
          >
            <polygon
              class="text-blueGray-200 fill-current"
              points="2560 0 2560 100 0 100"
            ></polygon>
          </svg>
        </div>
      </section>
      <section class="relative py-16 bg-blueGray-200">
        <div class="container mx-auto px-4">
          <div
            class="relative flex flex-col min-w-0 break-words bg-white w-full mb-6 shadow-xl rounded-lg -mt-64"
          >
            <div class="px-6">
              <div class="flex flex-wrap justify-center">
                <div class="w-full lg:w-3/12 px-4 lg:order-2 flex justify-center">
                  <div class="relative">
                    <img
                      alt="..."
                      src="https://demos.creative-tim.com/notus-js/assets/img/team-2-800x800.jpg"
                      class="shadow-xl rounded-full h-auto align-middle border-none absolute -m-16 -ml-20 lg:-ml-16 max-w-150-px"
                    />
                  </div>
                </div>
                <div class="w-full lg:w-4/12 px-4 lg:order-3 lg:text-right lg:self-center">
                  <div class="py-6 px-3 mt-32 sm:mt-0">
                    <button
                      class="bg-purple-600 hover:bg-purple-700 active:bg-purple-950 uppercase text-white font-bold hover:shadow-md shadow text-xs px-4 py-2 rounded outline-none focus:outline-none sm:mr-2 mb-1 ease-linear transition-all duration-150"
                      type="button"
                    >
                      Switch To Edit Mode
                    </button>
                  </div>
                </div>
                <div class="w-full lg:w-4/12 px-4 lg:order-1">
                  <div class="flex justify-center py-4 lg:pt-4 pt-8">
                    <div class="mr-4 p-3 text-center">
                      <!-- <span
                        class="text-xl font-bold block uppercase tracking-wide text-blueGray-600"
                        >22</span
                      ><span class="text-sm text-blueGray-400">Friends</span> -->
                    </div>
                    <div class="mr-4 p-3 text-center">
                      <!-- <span
                        class="text-xl font-bold block uppercase tracking-wide text-blueGray-600"
                        >10</span
                      ><span class="text-sm text-blueGray-400">Photos</span> -->
                    </div>
                    <div class="lg:mr-4 p-3 text-center">
                      <!-- <span
                        class="text-xl font-bold block uppercase tracking-wide text-blueGray-600"
                        >89</span
                      ><span class="text-sm text-blueGray-400">Comments</span> -->
                    </div>
                  </div>
                </div>
              </div>
              <div class="text-center mt-12">
                <h3 class="text-4xl font-semibold leading-normal mb-2 text-blueGray-700 mb-2">
                  {{ fullName }}
                </h3>
                <div class="text-sm leading-normal mt-0 mb-2 text-blueGray-400 font-bold uppercase">
                  <i class="fas fa-address-card mr-2 text-lg text-blueGray-400"></i>
                  {{ userInfo.preferred_username }}
                </div>
                <!-- <div class="text-sm leading-normal mt-0 mb-2 text-blueGray-400 font-bold uppercase">
                  <i class="fas fa-map-marker-alt mr-2 text-lg text-blueGray-400"></i>
                  Los Angeles, California
                </div> -->
                <!-- <div class="mb-2 text-blueGray-600 mt-10">
                  <i class="fas fa-briefcase mr-2 text-lg text-blueGray-400"></i>Solution Manager -
                  Creative Tim Officer
                </div>
                <div class="mb-2 text-blueGray-600">
                  <i class="fas fa-university mr-2 text-lg text-blueGray-400"></i>University of
                  Computer Science
                </div> -->
              </div>
              <div class="mt-10 py-10 border-t border-blueGray-200 text-center">
                <div class="flex flex-wrap justify-center">
                  <div class="w-full lg:w-9/12 px-4">
                    <p class="mb-4 text-lg leading-relaxed text-blueGray-700"></p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
    </main>
  </div>
</template>
