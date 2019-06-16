import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

// Tạo interface hứng dữ liệu từ server
export interface Admin {
  id: number,
  email: string,
  name: string,
  username: string,
  password: string,
  imageName: string,
  url: string
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
export interface ListRemovedResponse {
  errorCode: number,
  message: string,
  data: Admin[]
}
export interface ChangePassword {
  oldPassword: string,
  newPassword: string,
  confirmPassword: string
}
@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private api: ApiService, private http: HttpClient) { }
  getAll(): Observable<ListAdminResponse> {
    return this.http.get<ListAdminResponse>(`${this.api.apiUrl.admins}`);
  }
  getAllRemoved(): Observable<ListRemovedResponse> {
    return this.http.get<ListRemovedResponse>(`${this.api.apiUrl.admins}/${'adminRemoved'}`);
  }
  get(id: number): Observable<AdminResponse> {
    return this.http.get<AdminResponse>(`${this.api.apiUrl.admins}/${id}`);
  }
  getPhotoUrl(id: number): Observable<AdminResponse> {
    return this.http.get<AdminResponse>(`${this.api.apiUrl.admins}/${"getPhotoUrl"}/${id}`);
  }
  add(data: FormData): Observable<AdminResponse> {
    return this.http.post<AdminResponse>(`${this.api.apiUrl.admins}`, data);
  }
  update(data: FormData, id: number): Observable<AdminResponse> {
    return this.http.put<AdminResponse>(`${this.api.apiUrl.admins}/${id}`, data);
  }
  updatePassword(p: ChangePassword, id: number): Observable<AdminResponse> {
    return this.http.put<AdminResponse>(`${this.api.apiUrl.admins}/${'changePassword'}/${id}`, p);
  }
  restoreAdmin(id: number, data: Admin = null): Observable<AdminResponse> {
    return this.http.put<AdminResponse>(`${this.api.apiUrl.admins}/${'restore'}/${id}`, data);
  }
  remove(id: number): Observable<AdminResponse> {
    return this.http.delete<AdminResponse>(`${this.api.apiUrl.admins}/${id}`);
  }
}
