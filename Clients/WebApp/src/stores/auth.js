import { ref, computed, reactive } from 'vue'
import { defineStore } from 'pinia'
import mgr from '../../services/security'

export const useAuthStore = defineStore('auth', () => {
  const isAuthenticated = ref(false)
  const user = ref('')
  const mrg = ref(mgr)

  function setIsAuthenticated(payload) {
    isAuthenticated.value = payload
  }

  async function authenticate(returnPath) {
    const user = await getUser() //see if the user details are in local storage
    if (!!user) {
      isAuthenticated.value = true
      user.value = user
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
    signIn
  }
})
