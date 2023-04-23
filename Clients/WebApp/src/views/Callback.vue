<script setup>
import { useAuthStore } from '../stores/auth'
import mgr from '../../services/security'
import { useRouter } from 'vue-router'
import { onMounted } from 'vue'

const router = useRouter()

async function doAuth() {
  try {
    var result = await mgr.signinRedirectCallback()
    var returnToUrl = '/'
    if (result.state !== undefined) {
      returnToUrl = result.state
    }
    router.push({ path: returnToUrl })
  } catch (e) {
    console.log(e)
    //router.push({ name: 'Unauthorized' })
  }
}

onMounted(async () => {
  await doAuth()
})
</script>

<template>
  <div>
    <p>Sign-in in progress</p>
  </div>
</template>
