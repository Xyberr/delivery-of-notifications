import { createGlobalState, useAsyncState, useLocalStorage } from '@vueuse/core';
import { AuthService } from '@/heyapi';
import { showToast } from '@/toastService';

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
      },
    },
  )

  async function logout() {
    try {
      await AuthService.postAuthLogout()
    } catch (error) {
      console.error('Logout failed:', error);
      showToast({
        severity: 'error',
        summary: 'Ошибка при выходе',
        detail: `${error}`,
        life: 0
      })
    } finally {
      isAuthed.value = false
    }
  }

  return {
    isAuthed,
    isLoginLoading,
    loginAsync,
    logout,
  }
});
