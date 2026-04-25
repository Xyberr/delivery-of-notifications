import { useAuthStore } from '@/stores/auth'
import { createRouter, createWebHistory } from 'vue-router'
import { routes } from 'vue-router/auto-routes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

router.beforeEach((to) => {
  const authStore = useAuthStore()
  const isAuthed = authStore.isAuthed

  console.log(`Meta: ${to.meta}; IsAuthed: ${isAuthed}`)

  if (to.name === '/[...unknown]') {
    return;
  }

  if (to.meta.needAuth && !isAuthed) {
    return '/auth'
  }

  // todo: replace '/private' with actual private route
  if (to.path === '/auth' && isAuthed) {
    return '/private'
  }

  return true
})

export default router
