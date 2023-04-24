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

export default router
