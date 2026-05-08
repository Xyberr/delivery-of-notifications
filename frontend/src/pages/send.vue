<script setup lang="ts">
import SendForm from '@/components/SendForm.vue';

definePage({
  meta: {
    needAuth: true,
  }
});
</script>

<template>
    <main class="sendPage">
        <SendForm />
    </main>
</template>

<style scoped>
.sendPage {}
</style>
