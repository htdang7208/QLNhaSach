import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CustomerReceiptInfo {
  id: number,
  customerId: number,
  firstName: string,
  lastName: string,
  phone: string,
  email: string,
  address: string,
  dateCreated: Date,
  customerPaid: number,
  total: number,
  dept: number
}
export interface CustomerReceiptInfoResponse {
  errorCode: number,
  message: string,
  data: CustomerReceiptInfo
}
export interface ListCustomerReceiptInfoResponse {
  errorCode: number,
  message: string,
  data: CustomerReceiptInfo[]
}
export interface ListRemovedResponse {
  errorCode: number,
  message: string,
  data: CustomerReceiptInfo[]
}
@Injectable({
  providedIn: 'root'
})
export class ReceiptService {

  constructor(private api: ApiService, private http: HttpClient) { }
  getAll(): Observable<ListCustomerReceiptInfoResponse> {
    return this.http.get<ListCustomerReceiptInfoResponse> (`${this.api.apiUrl.receipts}`);
  }
  getAllRemoved(): Observable<ListRemovedResponse> {
    return this.http.get<ListRemovedResponse>(`${this.api.apiUrl.receipts}/${'receiptRemoved'}`);
  }
  get(id: number): Observable<CustomerReceiptInfoResponse> {
    return this.http.get<CustomerReceiptInfoResponse> (`${this.api.apiUrl.receipts}/${id}`);
  }
  add(data: FormData): Observable<CustomerReceiptInfoResponse> {
    return this.http.post<CustomerReceiptInfoResponse> (`${this.api.apiUrl.receipts}`, data);
  }
  update(data: FormData, id: number): Observable<CustomerReceiptInfoResponse> {
    return this.http.put<CustomerReceiptInfoResponse> (`${this.api.apiUrl.receipts}/${id}`, data);
  }
  restorereceipt(id: number, data: CustomerReceiptInfo = null): Observable<CustomerReceiptInfoResponse> {
    return this.http.put<CustomerReceiptInfoResponse>(`${this.api.apiUrl.receipts}/${'restore'}/${id}`, data);
  }
  remove(id: number): Observable<CustomerReceiptInfoResponse> {
    return this.http.delete<CustomerReceiptInfoResponse> (`${this.api.apiUrl.receipts}/${id}`);
  }
}
