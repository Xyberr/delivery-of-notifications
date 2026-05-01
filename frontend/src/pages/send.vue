<script setup lang="ts">
import SendForm from '@/components/SendForm.vue';

definePage({
  meta: {
    needAuth: true,
  }
});
</script>

<template>
    <div class="sendPage">
        <SendForm />
    </div>
</template>

<style scoped>
.sendPage {}
</style>
