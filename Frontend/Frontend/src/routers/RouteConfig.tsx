export interface RouteConfig {
  path: string
  element: React.ReactNode
  protected?: boolean | false
  children?: RouteConfig[]
}
