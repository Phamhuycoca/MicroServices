import { from, Observable } from 'rxjs'
import axiosBase from './axiosBase'

export class ApiBase {
  get<T>(url: string, params?: Record<string, unknown>): Observable<T> {
    return from(axiosBase.get<T>(url, { params }).then((res) => res.data))
  }

  post<T, B = unknown>(url: string, body?: B): Observable<T> {
    return from(axiosBase.post<T>(url, body).then((res) => res.data))
  }

  put<T, B = unknown>(url: string, body?: B): Observable<T> {
    return from(axiosBase.put<T>(url, body).then((res) => res.data))
  }

  delete<T>(url: string): Observable<T> {
    return from(axiosBase.delete<T>(url).then((res) => res.data))
  }
}

export const apiBase = new ApiBase()
