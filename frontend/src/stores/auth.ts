import { reactive, ref } from 'vue';
import { createGlobalState, useAsyncState, useLocalStorage } from '@vueuse/core';
import router from '@/router'
import { AuthService } from '@/heyapi';

export const useAuthStore = createGlobalState(() => {

  const isAuthed = useLocalStorage<boolean>('isAuthed', false)

  const { isLoading: isLoginLoading, execute: loginAsync} = useAsyncState(
    async (apiKey: string) => {
      return AuthService.postAuthLogin({
        body: { apiKey },
      })
    },
    null,
    {
      immediate: false,
      resetOnExecute: false,
      throwError: true,
      onSuccess(data) {
        isAuthed.value = !!data?.data;
        router.push('/private')
      },
    },
  )

  async function logOut(reason?: string) {
    try {
      AuthService.postAuthLogout()
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
  })
});
