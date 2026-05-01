<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { Button, useToast } from 'primevue';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const toast = useToast();
const router = useRouter();

const onLogout = async () => {
  try {
    await authStore.logout();
    router.push('/auth')
  } catch (error) {
    toast.add({ severity: 'error', summary: 'Ошибка при выходе', detail: `${error}` });
  }
}
</script>

<template>
    <Button 
      label="Выйти" 
      @click="onLogout"
      icon="pi pi-sign-out"
    />
</template>

<style scoped></style>
