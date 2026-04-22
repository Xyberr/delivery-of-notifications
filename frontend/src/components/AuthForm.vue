<script setup lang="ts">
import { useUserStore } from '@/stores/auth';
import { Button, FloatLabel, InputText, Panel } from 'primevue';
import { ref } from 'vue';

// todo: remove api key
const apiKey = ref('wai0H4Qe5qLtHYbd7E3UDvObhEEMBrla')
const userStore = useUserStore();

const onLogin = () => {
  userStore.loginAsync(apiKey.value)
}
</script>

<template>
  <Panel>
    <template #header>
      <h1>Вход</h1>
    </template>

    <form class="authPanelContent" @submit.prevent="onLogin">
      <FloatLabel variant="on">
        <InputText id="on_label" v-model="apiKey" type="text" />
        <label for="on_label">API Key</label>
      </FloatLabel>

      <p v-if="userStore.loginError" class="error">{{ userStore.loginError }}</p>

      <Button label="Войти" @click="onLogin" />
    </form>
  </Panel>
</template>

<style scoped>
.p-panel {
  width: fit-content;
  height: fit-content;
}

.authPanelContent {
  display: flex;
  flex-direction: column;
  width: fit-content;
  gap: 16px;
}

.error {
  color: #fc7b86;
}
</style>
