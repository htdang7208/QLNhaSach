import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http'

// Tạo interface hứng dữ liệu từ server
export interface Admin {
  id: number,
  firstName: string,
  lastName: string,
  phone: string,
  email: string,
  address: string,
  username: string,
  password: string,
  dept: number,
  isAdmin: boolean
}
export interface AdminResponse {
  errorCode: number,
  message: string,
  data: Admin
}
export interface ListAdminResponse {
  errorCode: number,
  message: string,
  data: Admin[]
}
@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private api: ApiService, private http: HttpClient) { }
  getAll(): Observable<ListAdminResponse> {
    return this.http.get<ListAdminResponse> (`${this.api.apiUrl.admins}`);
  }
  get(id: number): Observable<AdminResponse> {
    return this.http.get<AdminResponse> (`${this.api.apiUrl.admins}/${id}`);
  }
  add(data: Admin): Observable<AdminResponse> {
    return this.http.post<AdminResponse> (`${this.api.apiUrl.admins}`, data);
  }
  update(data: Admin): Observable<AdminResponse> {
    return this.http.put<AdminResponse> (`${this.api.apiUrl.admins}/${data.id}`, data);
  }
  remove(id: number): Observable<AdminResponse> {
    return this.http.delete<AdminResponse> (`${this.api.apiUrl.admins}/${id}`);
  }
}
