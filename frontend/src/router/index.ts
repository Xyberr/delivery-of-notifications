import { useAuthStore } from '@/stores/auth'
import { createRouter, createWebHistory } from 'vue-router'
import { routes } from 'vue-router/auto-routes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

const authStore = useAuthStore()

router.beforeEach((to) => {
  const isAuthed = authStore.isAuthed.value

  if (to.name === '/[...unknown]') {
    return true;
  }

  if (to.meta.needAuth && !isAuthed) {
    return '/auth'
  }

  if (to.path === '/auth' && isAuthed) {
    return '/send'
  }

  return true
})

export default router
