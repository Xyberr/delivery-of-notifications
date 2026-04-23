<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { Button, FloatLabel, InputText, Panel } from 'primevue';
import { ref } from 'vue';
import * as z from "zod"; 

// todo: remove api key
const apiKey = ref('wai0H4Qe5qLtHYbd7E3UDvObhEEMBrla')
const authStore = useAuthStore();
const parseError = ref<null | string>('')

const LoginScheme = z.object({
  apiKey: z.string().regex(/^[A-Za-z0-9]+$/, "Ключ должен содержать только латинские буквы и цифры"),
})

const onLogin = () => {
  parseError.value = null
  const result = LoginScheme.safeParse({apiKey: apiKey.value})

  if (!result.success) {
    parseError.value = result.error.issues[0]?.message as string
  } else {
    authStore.loginAsync(apiKey.value)
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
        <InputText id="on_label" v-model="apiKey" type="text" :disabled="authStore.isLoginLoading" />
        <label for="on_label">API Key</label>
      </FloatLabel>

      <p v-if="authStore.loginError" class="error">{{ authStore.loginError }}</p>
      <p v-if="parseError" class="error">{{ parseError }}</p>

      <Button label="Войти" @click="onLogin" :disabled="authStore.isLoginLoading" />
    </form>
  </Panel>
</template>

<style scoped>
.p-panel {
  width: fit-content;
  height: fit-content;
  max-width: 237px;
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
