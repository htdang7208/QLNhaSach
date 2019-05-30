import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ListCustomerResponse {
  errorCode: number;
  message: string;
  data: Customer[];
}
export interface CustomerResponse {
  errorCode: number;
  message: string;
  data: Customer;
}
export interface Customer {
  id: number,
  firstName: string,
  lastName: string,
  phone: string,
  email: string,
  address: string,
  username: string,
  password: string,
  isAdmin: boolean
}
@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  constructor(private api: ApiService, private http: HttpClient) { }

  getAll(): Observable<ListCustomerResponse> {
    const url = `${this.api.apiUrl.customers}`;
    return this.http.get<ListCustomerResponse>(url);
  }
  get(id): Observable<CustomerResponse> {
    const url = `${this.api.apiUrl.customers}/${id}`;
    return this.http.get<CustomerResponse>(url);
  }
  add(data: FormData): Observable<CustomerResponse> {
    const url = `${this.api.apiUrl.customers}`;
    return this.http.post<CustomerResponse>(url, data);
  }
  update(data: Customer): Observable<CustomerResponse> {
    const url = `${this.api.apiUrl.customers}/${data.id}`;
    return this.http.put<CustomerResponse>(url, data);
  }
  delete(id): Observable<CustomerResponse> {
    const url = `${this.api.apiUrl.customers}/${id}`;
    return this.http.delete<CustomerResponse>(url);
  }
}
