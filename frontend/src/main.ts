import { createApp } from 'vue'
import App from './App.vue'
import router from './router/index.ts'
import PrimeVue from 'primevue/config'
import { MyPreset } from './primevue-styles.ts'
import ToastService from 'primevue/toastservice';

const app = createApp(App)

app.use(ToastService)

app.use(router)

app.use(PrimeVue, {
  theme: {
    preset: MyPreset,
    options: {
      darkModeSelector: false,
    }
  }
})


app.mount('#app')
