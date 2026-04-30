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

definePage({
  meta: {
    needAuth: true,
  }
});
</script>

<template>
  <main>
    <p>Private area</p>

    <Button 
      label="Выйти" 
      @click="onLogout"
    />
  </main>

</template>

<style scoped></style>
