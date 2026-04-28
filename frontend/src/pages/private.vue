<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { Button, useToast } from 'primevue';

const authStore = useAuthStore();
const toast = useToast();

const onLogout = async () => {
  try {
    await authStore.logout();
  } catch (error) {
    toast.add({ severity: 'error', summary: 'Ошибка при выходе', detail: `${error}` });
  }
}

definePage({
  meta: {
    needAuth: true,
  }
});
</script>

<template>
  private

  <Button label="Выйти" @click="onLogout" />
</template>

<style scoped></style>
