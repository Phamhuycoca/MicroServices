import { useEffect } from 'react'
import { useSearchParams, useNavigate } from 'react-router-dom'
import axios from 'axios'
import { useDispatch } from 'react-redux'
import { setAccessToken } from '../../stores/slices/userSlice'
import Cookies from 'js-cookie'
import { Spin } from 'antd'
const LoginSuccessPage = () => {
  const [searchParams] = useSearchParams()
  const navigate = useNavigate()
  const dispatch = useDispatch()

  useEffect(() => {
    const code = searchParams.get('code') // lấy one-time code từ URL
    if (!code) {
      redirectToLogin()
      return
    }
    axios
      .get(`${import.meta.env.VITE_API_URL_TOKEN}/one-time-token?code=${code}`)
      .then((res) => {
        const { accessToken, refreshToken } = res.data
        if (!accessToken || !refreshToken) {
          redirectToLogin()
          return
        }
        // Lưu token
        dispatch(setAccessToken(accessToken))
        localStorage.setItem('access_token', accessToken)
        localStorage.setItem('refresh_token', refreshToken)
        navigate('/account')
      })
      .catch((err) => {
        console.error('Lấy token thất bại', err)
        redirectToLogin()
      })
  }, [searchParams, navigate, dispatch])

  const redirectToLogin = () => {
    // Xóa token trên localStorage
    localStorage.removeItem('access_token')
    localStorage.removeItem('refresh_token')
    // Xóa cookie
    Cookies.remove('access_token')
    Cookies.remove('AUTH_COOKIE')
    navigate(import.meta.env.VITE_URL_LOGIN || '/login')
  }

  return <Spin tip="Đang xử lý thông tin" fullscreen></Spin>
}

export default LoginSuccessPage
