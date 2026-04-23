import { reactive, ref } from 'vue';
import { createGlobalState, useAsyncState } from '@vueuse/core';
import { useRouter } from 'vue-router';
import { AuthService } from '@/heyapi';

export const useAuthStore = createGlobalState(() => {
  const router = useRouter();

  const isLoginLoading = ref(false)
  const loginError = ref<string | null>(null)

  const loginAsync = async (apiKey: string) => {
    isLoginLoading.value = true
    loginError.value = null

    try {
      await AuthService.postAuthLogin({
        body: { apiKey },
        throwOnError: true,
      })

      router.push('/private')
    } catch (error: any) {
      loginError.value = error.title ? `${error.title}: ${error.status}` : 'Login failed'
    } finally {
      isLoginLoading.value = false
    }  
  }

  async function logOut(reason?: string) {
    try {
      await AuthService.postAuthLogout()
      router.push('/auth')
    } catch (error) {
      console.error('Logout failed:', error);
      router.push('/auth')
    }
  }

  return reactive({
    isLoginLoading,
    loginError,
    loginAsync,
    logOut,
  })
});
