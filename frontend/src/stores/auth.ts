import { reactive, ref } from 'vue';
import { createGlobalState, useAsyncState } from '@vueuse/core';
import { useRouter } from 'vue-router';
import { postAuthLogin, postAuthLogout } from '@/heyapi';

export const useUserStore = createGlobalState(() => {
  const router = useRouter();

  const isLoginLoading = ref(false)
  const loginError = ref<string | null>(null)

  const loginAsync = async (apiKey: string) => {
    isLoginLoading.value = true
    loginError.value = null

    try {
      await postAuthLogin({
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
      await postAuthLogout()
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
