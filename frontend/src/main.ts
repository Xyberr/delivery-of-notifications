import { createApp } from 'vue'
import App from './App.vue'
import router from './router/index.ts'
import PrimeVue from 'primevue/config'
import { MyPreset } from './primevue-styles.ts'

const app = createApp(App)

app.use(PrimeVue, {
  theme: {
    preset: MyPreset,
    options: {
      darkModeSelector: false,
    }
  }
})

app.use(router)

app.mount('#app')
