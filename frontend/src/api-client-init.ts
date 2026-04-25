import { client } from "./heyapi/client.gen";
import { useAuthStore } from "./stores/auth";
import { showToast } from "./toastService";

export const initApiClient = () => {
    const authStore = useAuthStore()

    client.interceptors.request.use((request) => {
        if (!request.url.includes('auth/login') && !request.url.includes('auth/logout')) {
            if (!authStore.isAuthed) {
                showToast({
                    severity: 'error',
                    summary: 'Сессия истекла',
                    detail: 'Пожалуйста, войдите снова.',
                    life: 0
                })

                authStore.logOut();
            }
        }

        return request;
    });

    client.interceptors.response.use(async (response) => {
        if (response.status === 401) {
            showToast({
                severity: 'error',
                summary: 'Авторизация не пройдена',
                detail: 'Возможно устарел токен доступа. Пожалуйста, войдите снова.',
                life: 0
            })

            if (!response.url.includes('auth/login') && !response.url.includes('auth/logout')) {
                authStore.logOut()
            }
        } else if (response.status === 403) {
            showToast({
                severity: 'error',
                summary: 'Ошибка: 403',
                detail: 'Отказано в доступе',
                life: 0
            })
        }

        return response;
    });
}