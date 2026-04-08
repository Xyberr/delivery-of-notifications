<script setup lang="ts">
import { useUserStore } from '@/stores/auth';
import { Button, FloatLabel, InputText, Panel } from 'primevue';
import { ref } from 'vue';

const apiKey = ref('')
const userStore = useUserStore();

const onLogin = async () => {
  console.log('Login...')

  try {
    await userStore.loginAsync(0, apiKey.value);
    console.log('Login successful, redirecting to home page...');
  } catch (error) {
    console.error('Login failed:', error);
  }
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
</style>
