import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

// Tạo interface hứng dữ liệu từ server
export interface Customer {
  id: number,
  firstName: string,
  lastName: string,
  phone: string,
  email: string,
  address: string,
  username: string,
  password: string,
  confirmPassword: string,
  oldDept: number,
  nowDept: number,
  imageName: string,
  url: string
}
export interface CustomerResponse {
  errorCode: number,
  message: string,
  data: Customer
}
export interface ListCustomerResponse {
  errorCode: number,
  message: string,
  data: Customer[]
}
export interface ListRemovedResponse {
  errorCode: number,
  message: string,
  data: Customer[]
}
export interface ChangePassword {
  oldPassword: string,
  newPassword: string,
  confirmPassword: string
}
@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  constructor(private api: ApiService, private http: HttpClient) { }
  getAll(): Observable<ListCustomerResponse> {
    return this.http.get<ListCustomerResponse> (`${this.api.apiUrl.customers}`);
  }
  getAllRemoved(): Observable<ListRemovedResponse> {
    return this.http.get<ListRemovedResponse>(`${this.api.apiUrl.customers}/${'customerRemoved'}`);
  }
  get(id: number): Observable<CustomerResponse> {
    return this.http.get<CustomerResponse> (`${this.api.apiUrl.customers}/${id}`);
  }
  add(data: FormData): Observable<CustomerResponse> {
    return this.http.post<CustomerResponse> (`${this.api.apiUrl.customers}`, data);
  }
  update(data: FormData, id: number): Observable<CustomerResponse> {
    return this.http.put<CustomerResponse> (`${this.api.apiUrl.customers}/${id}`, data);
  }
  updatePassword(p: ChangePassword, id: number): Observable<CustomerResponse> {
    return this.http.put<CustomerResponse>(`${this.api.apiUrl.customers}/${'changePassword'}/${id}`, p);
  }
  restoreCustomer(id: number, data: Customer = null): Observable<CustomerResponse> {
    return this.http.put<CustomerResponse>(`${this.api.apiUrl.customers}/${'restore'}/${id}`, data);
  }
  remove(id: number): Observable<CustomerResponse> {
    return this.http.delete<CustomerResponse> (`${this.api.apiUrl.customers}/${id}`);
  }
}
