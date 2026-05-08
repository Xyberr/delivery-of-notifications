<script setup lang="ts">
import { Button, InputText, Panel, Textarea, useToast } from 'primevue';
import { ref } from 'vue';
import * as z from "zod"; 

const toast = useToast()

const email = ref('')
const subject = ref('')
const message = ref('')

const parseError = ref<null | string>(null)
// todo: 
// show toast on success
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

const sendMsg = () => {
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
            console.log('test')
        } catch (error) {
            toast.add({ severity: 'error', summary: 'Неизвестная ошибка', detail: `${error}` });
        }
    }
}

</script>

<template>
    <Panel header="Отправить сообщение">
        <form class="sendForm">
            <InputText inputmode="email" placeholder="Email получателя" v-model="email" />
            <InputText placeholder="Тема" v-model="subject" />
            <Textarea autoResize placeholder="Текст сообщения" v-model="message" />
    
            <p v-if="parseError" class="error">{{ parseError }}</p>

            <Button label="Отправить" class="sendButton" @click="sendMsg"/>
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
