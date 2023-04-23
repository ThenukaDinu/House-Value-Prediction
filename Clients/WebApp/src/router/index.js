import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import { useAuthStore } from '../stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/auth-test',
      name: 'Auth-Test',
      component: () => import('../views/AuthTest.vue'),
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/auth/callback',
      name: 'Callback',
      component: () => import('../views/Callback.vue')
    },
    {
      path: '/unauthorized',
      name: 'Unauthorized',
      component: () => import('../views/Unauthorized.vue')
    }
  ]
})

router.beforeEach(async (to, from, next) => {
  const { isAuthenticated, authenticate } = useAuthStore()

  if (isAuthenticated) {
    //already signed in, we can navigate anywhere
    next()
  } else if (to.matched.some((record) => record.meta.requiresAuth)) {
    //authentication is required. Trigger the sign in process, including the return URI
    authenticate(to.path).then(() => {
      console.log('authenticating a protected url:' + to.path)
      next()
    })
  } else {
    //No auth required. We can navigate
    next()
  }
})

export default router
