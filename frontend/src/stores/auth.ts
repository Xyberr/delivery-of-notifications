import { reactive, ref } from 'vue';
import { createGlobalState, useAsyncState, useLocalStorage } from '@vueuse/core';
import router from '@/router'
import { AuthService } from '@/heyapi';

export const useAuthStore = createGlobalState(() => {

  const isAuthed = useLocalStorage<boolean>('isAuthed', false)

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

      isAuthed.value = true
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
    } catch (error) {
      console.error('Logout failed:', error);
    } finally {
      isAuthed.value = false
      router.push('/auth')
    }
  }

  return reactive({
    isAuthed,
    isLoginLoading,
    loginError,
    loginAsync,
    logOut,
  })
});
