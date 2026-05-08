import { MessagesService, type CreateMessageRequest } from "@/heyapi";
import { showToast } from "@/toastService";
import { createGlobalState, useAsyncState } from "@vueuse/core";

export const useMessagesStore = createGlobalState(() => {
    const { isLoading: isMsgSending, execute: sendMsgAsync } = useAsyncState(
        async (msg: CreateMessageRequest) => {
            return MessagesService.postMessages({
                body: msg,
            })
        },
        null,
        {
            immediate: false,
            resetOnExecute: false,
            throwError: true,
            onSuccess() {
                showToast({
                    severity: 'success',
                    summary: 'Успех',
                    detail: 'Сообщение успешно зарегистрировано',
                    life: 3000
                })
            }
        },
    )

    return {
        isMsgSending,
        sendMsgAsync,
    }
})