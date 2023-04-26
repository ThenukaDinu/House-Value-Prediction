<script setup>
import { reactive, ref, computed, onMounted } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { v4 as uuidv4 } from 'uuid'
import { storeToRefs } from 'pinia'

const router = useRouter()
const authStore = useAuthStore()
const { isAuthenticated } = storeToRefs(authStore)
const { signIn } = useAuthStore()

onMounted(() => {
  // only guest users can come to sign  up page
  if (isAuthenticated.value) {
    router.push('/')
  }
})

let form = reactive({
  email: {
    value: ''
  },
  password: {
    value: ''
  },
  firstName: {
    value: ''
  },
  lastName: {
    value: ''
  }
})

let errorsInitialVal = {
  email: {
    errors: []
  },
  password: {
    errors: []
  },
  firstName: {
    errors: []
  },
  lastName: {
    errors: []
  }
}

const formErrors = reactive(JSON.parse(JSON.stringify(errorsInitialVal)))

// reset the formErrors to initial value
function resetFormErrors() {
  Object.assign(formErrors, JSON.parse(JSON.stringify(errorsInitialVal)))
}

// validate email
function isEmailValid(email) {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return emailRegex.test(email)
}

function isPasswordValid(password) {
  const passwordRegex = /^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*]).{8,}$/
  return passwordRegex.test(password)
}

const fullName = computed(() => {
  return form.firstName.value + form.lastName.value
})

const isInvalidForm = computed(() => {
  return Object.values(formErrors).some((prop) => prop.errors.length > 0)
})

async function registerUser() {
  try {
    resetFormErrors()

    if (form.email.value.trim() === '') {
      formErrors.email.errors.push({
        id: uuidv4(),
        message: 'Email cannot be empty.'
      })
    }
    if (!isEmailValid(form.email.value)) {
      formErrors.email.errors.push({
        id: uuidv4(),
        message: 'Please provide a valid email address'
      })
    }
    if (form.password.value.trim() === '') {
      formErrors.password.errors.push({
        id: uuidv4(),
        message: 'Password cannot be empty'
      })
    }
    if (!isPasswordValid(form.password.value)) {
      formErrors.password.errors.push({
        id: uuidv4(),
        message:
          'Password must contain at least one capital letter, one number, and one special character and must contains 8 or more characters in length'
      })
    }
    if (form.firstName.value.trim() === '') {
      formErrors.firstName.errors.push({
        id: uuidv4(),
        message: 'First name cannot be empty'
      })
    }
    if (form.lastName.value.trim() === '') {
      formErrors.lastName.errors.push({
        id: uuidv4(),
        message: 'Last name cannot be empty'
      })
    }

    if (isInvalidForm.value) {
      console.error('invalid form please provide valid ', formErrors)
      return
    }

    const data = {
      Email: form.email.value,
      FullName: fullName.value,
      Password: form.password.value,
      FirstName: form.firstName.value,
      LastName: form.lastName.value
    }
    const response = await axios.post(
      `${import.meta.env.VITE_BASE_URL_IDENTITY_SERVER_URL}api/Auth/signUpUser`,
      data
    )
    console.log(response)
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
      <form class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4" @submit.prevent="registerUser">
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="emailAddress">
            Email address
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.email.errors.length ? 'border-red-500' : ''"
            id="username"
            type="email"
            placeholder="johndoe@gmail.com"
            v-model="form.email.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.email.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="firstName">
            First name
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.firstName.errors.length ? 'border-red-500' : ''"
            id="username"
            type="text"
            placeholder="First name"
            v-model="form.firstName.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.firstName.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="lastName">
            Last name
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.lastName.errors.length ? 'border-red-500' : ''"
            id="username"
            type="text"
            placeholder="Last name"
            v-model="form.lastName.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.lastName.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>
        <div class="mb-6">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="password">
            Password
          </label>
          <input
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:shadow-outline"
            :class="formErrors.password.errors.length ? 'border-red-500' : ''"
            id="password"
            type="password"
            placeholder="******************"
            v-model="form.password.value"
          />
          <p
            class="text-red-500 text-xs italic"
            v-for="error in formErrors.password.errors"
            :key="error.id"
          >
            {{ error.message }}
          </p>
        </div>
        <div class="flex items-center justify-between">
          <button
            class="bg-[#662ac5] hover:bg-[#6d33ca] text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
            type="submit"
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
