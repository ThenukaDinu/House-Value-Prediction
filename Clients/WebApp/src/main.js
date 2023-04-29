import { createApp } from 'vue'
import { createPinia } from 'pinia'
import axios from 'axios'
import App from './App.vue'
import router from './router'
import mgr from '../services/security'

import './assets/main.css'
import { useAuthStore } from './stores/auth'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.mount('#app')

router.beforeEach(async (to, from, next) => {
  const { isAuthenticated, authenticate, user } = useAuthStore()

  if (isAuthenticated) {
    //already signed in, we can navigate anywhere
    next()
  } else if (to.matched.some((record) => record.meta.requiresAuth)) {
    //authentication is required. Trigger the sign in process, including the return URI
    console.log('authenticating a protected url:' + to.path)
    await authenticate(to.path)
    next()
  } else {
    //No auth required. We can navigate
    next()
  }
})

axios.interceptors.request.use(
  async (config) => {
    const user = await mgr.getUser()
    const userJson = JSON.parse(JSON.stringify(user))
    console.log(user)
    if (user) {
      const authToken = user.access_token
      if (authToken && !config.url.includes('apilayer.com')) {
        config.headers.Authorization = `Bearer ${authToken}`
      }
    }
    return config
  },
  (err) => {
    console.error($`err = {err}`)
  }
)
