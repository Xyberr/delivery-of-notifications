import { reactive, ref } from 'vue';
import { createGlobalState, useAsyncState, useLocalStorage } from '@vueuse/core';
import router from '@/router'
import { AuthService } from '@/heyapi';

export const useAuthStore = createGlobalState(() => {

  const isAuthed = useLocalStorage<boolean>('isAuthed', false)

  const loginError = ref<string | null>(null)

  const { isLoading: isLoginLoading, execute: loginAsync} = useAsyncState(
    async (apiKey: string) => {
      loginError.value = null

      return await AuthService.postAuthLogin({
        body: { apiKey },
        throwOnError: true,
      })
    },
    null,
    {
      immediate: false,
      resetOnExecute: false,

      onSuccess() {
        isAuthed.value = true
        router.push('/private')
      },

      onError(error: any) {
        loginError.value =
          error?.title
            ? `${error.title}: ${error.status}`
            : 'Login failed'
      },
    },
  )

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
    loginAsync,
    logOut,
    loginError,
  })
});
