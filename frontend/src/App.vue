<script setup lang="ts">
import { Toast } from 'primevue';
import { RouterView } from 'vue-router';
import { useToast } from 'primevue'
import { setToast } from './toastService'
import SideBarMenu from './components/common/SideBarMenu.vue';
import { useAuthStore } from './stores/auth';

const toast = useToast()

setToast(toast.add)

const authStore = useAuthStore()
</script>

<template>
  <div class="layout">
        <SideBarMenu v-if="authStore.isAuthed.value" />
        <div class="content">
          <Toast />
          <RouterView />
        </div>
    </div>
</template>

<style>
@import url('https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap');
@import 'primeicons/primeicons.css';

* {
  font-family: "Roboto", sans-serif;
  padding: 0px;
  margin: 0px;
}

.layout {
  height: 100vh;
  display: flex;
}

.content {
  padding: 16px;
  flex: 1;
  display: flex;
}

.error {
  color: #fc7b86;
}
</style>
