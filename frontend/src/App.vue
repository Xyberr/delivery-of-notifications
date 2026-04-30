<script setup lang="ts">
import { Toast } from 'primevue';
import { RouterView } from 'vue-router';
import { useToast } from 'primevue'
import { setToast } from './toastService'

const toast = useToast()

setToast(toast.add)
</script>

<template>
  <Toast />
  <RouterView />
</template>

<style>
@import url('https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap');

* {
  font-family: "Roboto", sans-serif;
  padding: 0px;
  margin: 0px;
}
</style>
