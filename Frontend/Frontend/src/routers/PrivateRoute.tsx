import { Navigate } from 'react-router-dom';
import type { ReactNode } from 'react';

interface PrivateRouteProps {
  element: ReactNode;
}

export const PrivateRoute = ({ element }: PrivateRouteProps) => {
  const isAuthenticated = true;

  return isAuthenticated ? <>{element}</> : <Navigate to={import.meta.env.URL_LOGIN} replace />;
};