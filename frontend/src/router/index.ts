import { createRouter, createWebHistory } from 'vue-router'
import { routes } from 'vue-router/auto-routes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

const PUBLIC_PATHS = new Set([''])

router.beforeEach((to) => {
  if (to.name === '/[...unknown]') {
    return;
  }

  if (PUBLIC_PATHS.has(to.path)) {
    return;
  }
})

export default router
