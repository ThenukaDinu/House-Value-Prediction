<script setup>
import { onMounted } from 'vue'
import mgr from '../../services/security'
import { useAuthStore } from '../stores/auth'
import { useRouter } from 'vue-router'

const { setIsAuthenticated } = useAuthStore()
const router = useRouter()

function setResponsive() {
  var x = document.getElementById('myTopnav')
  if (x.className === 'topnav') {
    x.className += ' responsive'
  } else {
    x.className = 'topnav'
  }
}

async function logout() {
  try {
    localStorage.clear()
    setIsAuthenticated(false)
    const response = await mgr.signoutRedirect()
    console.log(response)
    router.push({ path: '/' })
  } catch (error) {
    localStorage.clear()
    setIsAuthenticated(false)
    console.error(error)
    router.push({ path: '/' })
  }
}

onMounted(() => {
  setResponsive()
})
</script>

<template>
  <div>
    <div class="topnav" id="myTopnav">
      <router-link to="/" class="active">Home</router-link>
      <router-link to="/estimate-value">Estimate Value</router-link>
      <a href="#houses">Houses</a>
      <a href="#inqiery">Inquiry</a>
      <a href="#profile">Profile</a>
      <a href="#about">About</a>
      <a class="logout" href="#logout" @click="logout">Logout</a>
      <a href="javascript:void(0);" class="icon" onclick="setResponsive()">
        <i class="fa fa-bars"></i>
      </a>
    </div>
  </div>
  <div class="user_layout">
    <slot />
  </div>
</template>

<style lang="scss" scoped>
/* Add a black background color to the top navigation */
.topnav {
  background-color: #333;
  overflow: hidden;
}

/* Style the links inside the navigation bar */
.topnav a {
  float: left;
  display: block;
  color: #f2f2f2;
  text-align: center;
  padding: 14px 16px;
  text-decoration: none;
  font-size: 17px;
}

.topnav .logout {
  float: right;
}

/* Change the color of links on hover */
.topnav a:hover {
  background-color: #ddd;
  color: black;
}

/* Add an active class to highlight the current page */
.topnav a.active {
  background-color: #662ac5;
  color: white;
}

/* Hide the link that should open and close the topnav on small screens */
.topnav .icon {
  display: none;
}

/* When the screen is less than 600 pixels wide, hide all links, except for the first one ("Home"). Show the link that contains should open and close the topnav (.icon) */
@media screen and (max-width: 600px) {
  .topnav a:not(:first-child) {
    display: none;
  }
  .topnav a.icon {
    float: right;
    display: block;
  }
}

/* The "responsive" class is added to the topnav with JavaScript when the user clicks on the icon. This class makes the topnav look good on small screens (display the links vertically instead of horizontally) */
@media screen and (max-width: 600px) {
  .topnav.responsive {
    position: relative;
  }
  .topnav.responsive a.icon {
    position: absolute;
    right: 0;
    top: 0;
  }
  .topnav.responsive a {
    float: none;
    display: block;
    text-align: left;
  }
}
</style>
