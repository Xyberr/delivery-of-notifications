<script setup lang="ts">
import { MessagesService, type CreateMessageRequest } from '@/heyapi';
import { useAsyncState } from '@vueuse/core';
import { Button, InputText, Panel, Textarea, useToast } from 'primevue';
import { ref } from 'vue';
import * as z from "zod"; 

const toast = useToast()

const email = ref('mail123@mail.ru')
const subject = ref('Тема')
const message = ref('Текст')

const parseError = ref<null | string>(null)
    
// todo: 
// send user to jobs page or update jobs list on success

const MsgSchema = z.object({
    email: z
        .email("Некорректный email")
        .trim(),
    subject: z
        .string()
        .trim()
        .nonempty("Введите тему сообщения"),
    message: z
        .string()
        .nonempty("Введите текст сообщения")
})

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
            toast.add({ 
                severity: 'success', 
                summary: 'Успех', 
                detail: 'Сообщение успешно зарегистрировано', 
                life: 3000 
            })
        }
    },
)

const sendMsg = async () => {
    parseError.value = null
    const result = MsgSchema.safeParse({
        email: email.value,
        subject: subject.value,
        message: message.value
    })

    if (!result.success) {
        parseError.value = result.error.issues[0]?.message as string ?? 'Ошибка валидации'
    } else {
        try {
            await sendMsgAsync(0, {
                subject: subject.value,
                storageTimeAfterSendingInHours: 24,
                messageBody: message.value,
                recipients: [
                    {
                        contactTypeId: 1,
                        contactData: email.value
                    }
                ]
            })
        } catch (error) {
            toast.add({ severity: 'error', summary: 'Неизвестная ошибка', detail: `${error}` });
        }
    }
}

</script>

<template>
    <Panel header="Отправить сообщение">
        <form class="sendForm" @submit.prevent="sendMsg">
            <InputText inputmode="email" placeholder="Email получателя" v-model="email" />
            <InputText placeholder="Тема" v-model="subject" />
            <Textarea autoResize placeholder="Текст сообщения" v-model="message" />
    
            <p v-if="parseError" class="error">{{ parseError }}</p>

            <Button 
                label="Отправить" 
                class="sendButton" 
                type="submit" 
                :disabled="isMsgSending"
            />
        </form>
    </Panel>
</template>

<style scoped>
.p-panel {
    width: fit-content;
}

.sendForm {
    display: flex;
    flex-direction: column;
    width: fit-content;
    gap: 8px;
}

.sendButton {
    margin-top: 8px;
}
</style>
