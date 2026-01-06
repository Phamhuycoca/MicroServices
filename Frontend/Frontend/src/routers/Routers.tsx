import { AccountPage } from '../layout/account/AccountPage'
import LoginSuccessPage from '../layout/auth/LoginSuccessPage'
import type { RouteConfig } from './RouteConfig'

export const routes: RouteConfig[] = [
  {
    path: '/account',
    protected: false,
    element: <AccountPage />
  },
  {
    path: '/login-success',
    protected: false,
    element: <LoginSuccessPage />
  }
]
