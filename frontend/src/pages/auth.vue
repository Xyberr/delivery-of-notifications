<script setup lang="ts">
import AuthForm from '@/components/AuthForm.vue';

definePage({
  meta: {
    needAuth: false,
  }
});
</script>

<template>
  <main class="authPage">
    <AuthForm />
  </main>
</template>

<style scoped>
.authPage {
  display: flex;
  height: 100vh;
  align-items: center;
  justify-content: center;
}
</style>
