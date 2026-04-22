import { reactive } from 'vue';
import { createGlobalState, useAsyncState } from '@vueuse/core';
import { useRouter } from 'vue-router';
import { postAuthLogin } from '@/heyapi';

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
          router.push('/')
        }
      },
    },
  )

  function logOut(reason?: string) {
    router.push('/auth')
    if (reason) {
      console.log(`Выход: ${reason}`)
      return
    }
  }

  return reactive({
    isLoginLoading,
    loginAsync,
    logOut,
  })
});
