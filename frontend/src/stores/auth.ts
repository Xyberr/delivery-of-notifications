import { reactive } from 'vue';
import { createGlobalState, useAsyncState } from '@vueuse/core';
import { useRouter } from 'vue-router';

export const useUserStore = createGlobalState(() => {
  const router = useRouter();

  // todo: replace this func with func from heyapi
  const login = async (APIkey: string) => {
    await new Promise(resolve => setTimeout(resolve, 1200));

    if (!APIkey || APIkey.trim() === '') {
      throw new Error('API ключ не может быть пустым');
    }

    if (APIkey === 'demo' || APIkey === 'valid-key-12345') {
      console.log('Моковый логин успешен');

      return {
        success: true,
        message: 'Авторизация прошла успешно',
      };
    }
    else {
      throw new Error('Неверный API ключ');
    }
  };

  const { isLoading: isLoginLoading, execute: loginAsync } = useAsyncState((APIKey: string) => login(APIKey),
    null,
    {
      resetOnExecute: false,
      shallow: false,
      immediate: false,
      throwError: true,
      onSuccess(res) {
        // todo: add login logic
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
