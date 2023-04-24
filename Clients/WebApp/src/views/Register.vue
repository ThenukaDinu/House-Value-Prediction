<script setup>
import { reactive, ref, computed } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const { signIn } = useAuthStore()

let form = reactive({
  email: {
    value: '',
    errors: []
  },
  password: {
    value: '',
    errors: []
  },
  firstName: {
    value: '',
    errors: []
  },
  lastName: {
    value: '',
    errors: []
  }
})

const fullName = computed(() => {
  return form.firstName.value + form.lastName.value
})

async function registerUser() {
  try {
    const data = {
      Email: form.email.value,
      FullName: fullName.value,
      Password: form.password.value,
      FirstName: form.firstName.value,
      LastName: form.lastName.value
    }
    const response = await axios.post(
      `${import.meta.env.VITE_BASE_URL_IDENTITY_SERVER_URL}Auth/signUpUser`,
      data
    )
    if (response.status === 200 || response.status === 201) {
      await signIn('/')
    }
  } catch (error) {
    console.error(error)
  }
}
</script>

<template>
  <div class="register flex justify-center items-center h-[90vh]">
    <div class="w-full max-w-xs">
      <form class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="emailAddress">
            Email address
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="username"
            type="text"
            placeholder="johndoe@gmail.com"
          />
        </div>
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="firstName">
            First name
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="username"
            type="text"
            placeholder="First name"
          />
        </div>
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="lastName">
            Last name
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="username"
            type="text"
            placeholder="Last name"
          />
        </div>
        <div class="mb-6">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="password">
            Password
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:shadow-outline"
            id="password"
            type="password"
            placeholder="******************"
          />
        </div>
        <div class="flex items-center justify-between">
          <button
            class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
            type="button"
          >
            Register
          </button>
        </div>
      </form>
      <p class="text-center text-gray-500 text-xs">&copy;2023 HouseX. All rights reserved.</p>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
