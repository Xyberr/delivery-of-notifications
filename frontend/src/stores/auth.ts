import { reactive } from 'vue';
import { createGlobalState, useAsyncState } from '@vueuse/core';
import { useRouter } from 'vue-router';
import { postAuthLogin, postAuthLogout } from '@/heyapi';

export const useUserStore = createGlobalState(() => {
  const router = useRouter();

  const { isLoading: isLoginLoading, execute: loginAsync } = useAsyncState((APIKey: string) => postAuthLogin({body: {apiKey: APIKey}}),
    null,
    {
      resetOnExecute: false,
      shallow: false,
      immediate: false,
      throwError: true,
      onSuccess(res) {
        if (router) {
          router.push('/private')
        }
      },
    },
  )

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
    loginAsync,
    logOut,
  })
});
