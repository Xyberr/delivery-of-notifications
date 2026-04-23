<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { Button } from 'primevue';

const authStore = useAuthStore();

const onLogout = async () => {
  try {
    await authStore.logOut();
  } catch (error) {
    console.error('Logout failed:', error);
  }
}
</script>

<template>
  private

  <Button label="Выйти" @click="onLogout" />
</template>

<style scoped></style>
