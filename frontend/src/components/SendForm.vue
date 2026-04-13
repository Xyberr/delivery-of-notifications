<script setup lang="ts">
import { Button, InputText, Panel, Textarea } from 'primevue';
import { ref } from 'vue';

const email = ref('')
const subject = ref('')
const message = ref('')

// todo: 
// show toast on success
// send user to jobs page or update jobs list on success

const sendMsg = () => {}
</script>

<template>
    <Panel header="Отправить сообщение">
        <form class="sendForm">
            <InputText inputmode="email" placeholder="Email получателя" v-model="email" />
            <InputText placeholder="Тема" v-model="subject" />
            <Textarea autoResize placeholder="Текст сообщения" v-model="message" />
    
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
