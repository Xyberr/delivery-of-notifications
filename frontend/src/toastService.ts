import type { ToastMessageOptions } from 'primevue/toast'

type ToastFn = (msg: ToastMessageOptions) => void

let toastHandler: ToastFn | null = null

export const setToast = (fn: ToastFn) => {
  toastHandler = fn
}

export const clearToast = () => {
  toastHandler = null
}

export const showToast = (msg: ToastMessageOptions) => {
  if (!toastHandler) {
    console.warn('Toast is not initialized')
    return
  }

  toastHandler(msg)
}