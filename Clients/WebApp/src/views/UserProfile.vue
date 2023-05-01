<script setup>
import axios from 'axios'
import mgr from '../../services/security'

async function onCreated() {
  try {
    const user = await mgr.getUser()
    const userJson = JSON.parse(JSON.stringify(user))

    const response = await axios.get(`${import.meta.env.VITE_BASE_URL_HOUSE_MANAGE_API}api/Users`)
    console.log(response)
  } catch (error) {
    console.error(error)
  }
}

onCreated()
</script>

<template>
  <div class="user_profile">
    <h1>User Profile</h1>
  </div>
</template>
