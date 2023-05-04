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
    },
    {
      path: '/register',
      name: 'Register',
      component: () => import('../views/Register.vue')
    },
    {
      path: '/about',
      name: 'About',
      component: () => import('../views/AboutUs.vue')
    },
    {
      path: '/estimate-value',
      name: 'estimate-value',
      component: () => import('../views/EstimateValue.vue'),
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/all-houses',
      name: 'allHouses',
      component: () => import('../views/AllHouses.vue'),
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/profile',
      name: 'Profile',
      component: () => import('../views/UserProfile.vue'),
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/my-inquiries',
      name: 'MyInquiries',
      component: () => import('../views/MyInquiries.vue'),
      meta: {
        requiresAuth: true
      }
    }
  ]
})

export default router
