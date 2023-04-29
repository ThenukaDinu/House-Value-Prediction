<script setup>
import { useAuthStore } from '../stores/auth'
import mgr from '../../services/security'
import { useRouter } from 'vue-router'
import { onMounted } from 'vue'

const router = useRouter()
const { setIsAuthenticated } = useAuthStore()
async function doAuth() {
  try {
    var result = await mgr.signinRedirectCallback()
    console.log('mgr.signinRedirectCallback() result = ', result)
    var returnToUrl = '/'
    if (result.state !== undefined) {
      console.log('redirection URL: ' + result.state)
      returnToUrl = result.state
      setIsAuthenticated(true)
      setTimeout(() => {
        router.push({ path: returnToUrl })
      }, 1000)
    } else {
      router.push({ path: returnToUrl })
    }
  } catch (e) {
    console.log(e)
    router.push({ path: '/' })
  }
}

onMounted(async () => {
  setTimeout(async () => {
    // handles the oidc redireaction after authentication return from Identity server
    await doAuth()
  }, 1000)
})
</script>

<template>
  <div>
    <p>Sign-in in progress</p>
  </div>
</template>
