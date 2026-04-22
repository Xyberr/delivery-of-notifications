<script setup lang="ts">
import { useUserStore } from '@/stores/auth';
import { Button } from 'primevue';

const userStore = useUserStore();

const onLogout = async () => {
  try {
    await userStore.logOut();
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
