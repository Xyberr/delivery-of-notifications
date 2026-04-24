import type { ToastMessageOptions } from 'primevue/toast'

let toastInstance: ((msg: ToastMessageOptions) => void) | null = null

export const setToast = (fn: (msg: ToastMessageOptions) => void) => {
  toastInstance = fn
}

export const showToast = (msg: ToastMessageOptions) => {
  if (!toastInstance) return
  toastInstance(msg)
}