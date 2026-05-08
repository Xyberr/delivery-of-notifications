<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { Button, FloatLabel, InputText, Panel, useToast } from 'primevue';
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import * as z from "zod"; 

const apiKey = ref<string>(import.meta.env.VITE_API_KEY || '');
const authStore = useAuthStore();
const parseError = ref<null | string>(null)
const toast = useToast();
const router = useRouter();

const LoginSchema = z.object({
  apiKey: z
  .string()
  .trim()
  .nonempty("Введите API Key")
  .regex(/^[A-Za-z0-9]+$/, "Ключ должен содержать только латинские буквы и цифры"),
})

const onLogin = async () => {
  parseError.value = null
  const result = LoginSchema.safeParse({apiKey: apiKey.value})

  if (!result.success) {
    parseError.value = result.error.issues[0]?.message as string ?? 'Ошибка валидации'
  } else {
    try {
      await authStore.loginAsync(0, apiKey.value)
      router.push('/send')
    } catch (error) {
      toast.add({ severity: 'error', summary: 'Неизвестная ошибка', detail: `${error}` });
    }
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
        <InputText id="on_label" v-model="apiKey" type="text" :disabled="authStore.isLoginLoading.value" />
        <label for="on_label">API Key</label>
      </FloatLabel>

      <p v-if="parseError" class="error">{{ parseError }}</p>

      <Button 
        label="Войти" 
        type="submit"
        :disabled="authStore.isLoginLoading.value" 
      />
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
</style>
