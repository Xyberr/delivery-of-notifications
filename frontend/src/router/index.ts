import { AuthService } from '@/heyapi'
import { createRouter, createWebHistory } from 'vue-router'
import { routes } from 'vue-router/auto-routes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

router.beforeEach(async (to) => {
  if (to.name === '/[...unknown]') {
    return;
  }

  const isAuthPage = to.path === '/auth'

  try {
    const res = await AuthService.getAuthSecure()

    if (!res.response.ok) {
      throw new Error('Not authenticated')
    }

    if (isAuthPage) {
      return '/private'
    }

    return true
  } catch (e) {

    if (isAuthPage) {
      return true
    }

    return '/auth'
  }
})

export default router
