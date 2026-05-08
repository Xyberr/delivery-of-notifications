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
            onSuccess(data) {
                showToast({
                    severity: 'success',
                    summary: 'Сообщение зарегистрировано',
                    detail: `ID: ${data?.data?.messageId}`,
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