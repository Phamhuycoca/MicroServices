import { createRoot } from 'react-dom/client'
import './styles/index.scss'
import { StyleProvider } from '@ant-design/cssinjs'
import { ConfigProvider } from 'antd'
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import { Provider } from 'react-redux'
import { store } from './stores/store'
import { routes } from './routers/Routers'
import { PrivateRoute } from './routers/PrivateRoute'
createRoot(document.getElementById('root')!).render(
  <StyleProvider hashPriority='high'>
    <ConfigProvider theme={{ cssVar: { key: 'app' }, hashed: false }}>
      <Provider store={store}>
        <Router>
          <Routes>
            {routes.map((route) => {
              if (route.children) {
                return (
                  <Route key={route.path} path={route.path} element={route.element}>
                    {route.children.map((childRoute) => (
                      <Route key={childRoute.path} path={childRoute.path} element={
                        childRoute.protected ?
                          <PrivateRoute element={childRoute.element} />
                          :
                          childRoute.element
                      } />
                    ))}
                  </Route>
                )
              }
              return <Route key={route.path} path={route.path} element={
                route.protected ?
                  <PrivateRoute element={route.element} />
                  :
                  route.element
              } />
            })}
          </Routes>
        </Router>
      </Provider>
    </ConfigProvider>
  </StyleProvider>
)
