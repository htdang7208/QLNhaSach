import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface SaleDetail {
  stt: number,
  bookId: number,
  saleId: number,
  name: string,
  kind: string,
  amount: number,
  price: number
}
export interface Sale {
  saleId: number,
  customerId: number,
  firstName: string,
  lastName: string,
  dateCreated: Date,
  listSaleDetail: SaleDetail[]
}
export interface SaleResponse {
  errorCode: number,
  message: string,
  data: Sale
}
export interface ListSaleResponse {
  errorCode: number,
  message: string,
  data: Sale[]
}
export interface ListRemoveResponse {
  errorCode: number,
  message: string,
  data: Sale[]
}
@Injectable({
  providedIn: 'root'
})
export class SaleService {

  constructor(private api: ApiService, private http: HttpClient) { }
  getAll(): Observable<ListSaleResponse> {
    console.log("vao roi");
    return this.http.get<ListSaleResponse>(`${this.api.apiUrl.sales}`);
  }
  get(id: number): Observable<SaleResponse> {
    return this.http.get<SaleResponse>(`${this.api.apiUrl.sales}/${id}`);
  }
  add(data: FormData): Observable<SaleResponse> {
    return this.http.post<SaleResponse>(`${this.api.apiUrl.sales}`, data);
  }
  update(data: FormData, id: number): Observable<SaleResponse> {
    return this.http.put<SaleResponse>(`${this.api.apiUrl.sales}/${id}`, data);
  }
  remove(id: number): Observable<SaleResponse> {
    return this.http.delete<SaleResponse>(`${this.api.apiUrl.sales}/${id}`);
  }
  restore(id: number, data: Sale = null): Observable<SaleResponse> {
    return this.http.put<SaleResponse>(`${this.api.apiUrl.sales}/${'restore'}/${id}`, data);
  }
  getAllRemoved(): Observable<ListRemoveResponse> {
    return this.http.get<ListRemoveResponse>(`${this.api.apiUrl.sales}/${'saleRemoved'}`);
  }
}
