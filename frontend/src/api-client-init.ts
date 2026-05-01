import { useRouter } from "vue-router";
import { client } from "./heyapi/client.gen";
import { useAuthStore } from "./stores/auth";
import { showToast } from "./toastService";

export const initApiClient = () => {
    const authStore = useAuthStore()
    const router = useRouter()
    
    const isAuthEndpoint = (url = '') => {
        return url.includes('auth/login') || url.includes('auth/logout')
    }

    client.interceptors.request.use(async (request) => {
        if (!isAuthEndpoint(request.url)) {
            if (!authStore.isAuthed.value) {
                showToast({
                    severity: 'error',
                    summary: 'Сессия истекла',
                    detail: 'Пожалуйста, войдите снова.',
                    life: 0
                })

                await authStore.logout();
                router.push('/auth')
                throw new Error('Пользователь не авторизирован')
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

            if (!isAuthEndpoint(response.url)) {
                await authStore.logout()
                router.push('/auth')
            }
        } else if (response.status === 403) {
            showToast({
                severity: 'error',
                summary: 'Ошибка: 403',
                detail: 'Отказано в доступе',
                life: 0
            })
        } else if (response.status > 399) {
            showToast({
                severity: 'error',
                summary: 'Ошибка: ' + response.status,
                detail: 'Произошла ошибка',
                life: 0
            })
        }

        return response;
    });
}