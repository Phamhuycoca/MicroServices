import axios from 'axios'

const axiosBase = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000/api',
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json'
  }
})

// ================== REQUEST ==================
axiosBase.interceptors.request.use((config) => {
  const token = localStorage.getItem('access_token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// ================== RESPONSE ==================
let isRefreshing = false
type FailedQueueItem = {
  resolve: (token: string) => void
  reject: (error: unknown) => void
}

let failedQueue: FailedQueueItem[] = []

const processQueue = (error: unknown, token: string | null = null) => {
  failedQueue.forEach((prom) => {
    if (error) prom.reject(error)
    else prom.resolve(token!)
  })
  failedQueue = []
}

axiosBase.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config

    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject })
        }).then((token) => {
          originalRequest.headers.Authorization = `Bearer ${token}`
          return axiosBase(originalRequest)
        })
      }

      originalRequest._retry = true
      isRefreshing = true

      try {
        const refreshToken = localStorage.getItem('refresh_token')
        const res = await axios.post(`${import.meta.env.VITE_API_URL_TOKEN}/refresh-token`, {
          refresh_token: refreshToken
        })
        const { access_token, refresh_token } = res.data
        localStorage.setItem('access_token', access_token)
        localStorage.setItem('refresh_token', refresh_token)
        axiosBase.defaults.headers.common.Authorization = `Bearer ${access_token}`
        processQueue(null, access_token)
        return axiosBase(originalRequest)
      } catch (err) {
        processQueue(err, null)
        localStorage.clear()
        window.location.href = import.meta.env.URL_LOGIN
        return Promise.reject(err)
      } finally {
        isRefreshing = false
      }
    }

    return Promise.reject(error)
  }
)

export default axiosBase
