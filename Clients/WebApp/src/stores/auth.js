import { ref, computed, reactive, watch } from 'vue'
import { defineStore } from 'pinia'
import mgr from '../../services/security'

export const useAuthStore = defineStore('auth', () => {
  let isAuthenticated = ref(false)
  let user = ref({})
  const mrg = ref(mgr)

  if (localStorage.getItem('isAuthenticated') !== null) {
    isAuthenticated.value = Boolean(localStorage.getItem('isAuthenticated'))
  }

  if (localStorage.getItem('user') !== null && localStorage.getItem('user') !== undefined) {
    user.value = JSON.parse(localStorage.getItem('user'))
  }

  watch(
    isAuthenticated,
    (authVal) => {
      localStorage.setItem('isAuthenticated', authVal)
    },
    { deep: true }
  )
  watch(
    user,
    (userVal) => {
      localStorage.setItem('user', JSON.stringify(userVal))
    },
    { deep: true }
  )

  function setIsAuthenticated(payload) {
    isAuthenticated.value = payload
  }

  async function authenticate(returnPath) {
    const userDTO = await getUser() //see if the user details are in local storage
    if (!!userDTO) {
      isAuthenticated.value = true
      user.value = userDTO
    } else {
      await signIn(returnPath)
    }
  }

  async function getUser() {
    try {
      let user = await mgr.getUser()
      return user
    } catch (err) {
      console.log(err)
    }
  }

  async function signIn(returnPath) {
    returnPath ? mgr.signinRedirect({ state: returnPath }) : mgr.signinRedirect()
  }

  return {
    isAuthenticated,
    setIsAuthenticated,
    authenticate,
    getUser,
    signIn,
    user
  }
})
