import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Input {
  id: number;
  stt: number;
  bookId: number;
  name: string;
  kind: string;
  author: string;
  amount: number;
  isRemove: boolean;
}
export interface ListInput {
  data: Input[];
}
export interface InputResponse {
  errorCode: number;
  message: string;
  data: Input;
}
export interface ListInputResponse {
  errorCode: number;
  message: string;
  data: ListInput;
}
export interface ListRemovedResponse {
  errorCode: number;
  message: string;
  data: ListInput;
}
@Injectable({
  providedIn: 'root'
})
export class InputService {

  constructor(private api: ApiService, private http: HttpClient) { }
  getAll(): Observable<ListInputResponse> {
    return this.http.get<ListInputResponse>(`${this.api.apiUrl.inputs}`);
  }
  getAllRemoved(): Observable<ListRemovedResponse> {
    return this.http.get<ListRemovedResponse>(`${this.api.apiUrl.inputs}/${'inputRemoved'}`);
  }
  get(stt: number): Observable<ListInputResponse> {
    return this.http.get<ListInputResponse>(`${this.api.apiUrl.inputs}/${stt}`);
  }
  add(data: ListInput): Observable<ListInputResponse> {
    return this.http.post<ListInputResponse>(`${this.api.apiUrl.inputs}`, data);
  }
  update(data: ListInput, stt: number): Observable<ListInputResponse> {
    return this.http.put<ListInputResponse>(`${this.api.apiUrl.inputs}/${stt}`, data);
  }
  restoredInput(stt: number, data: FormData = null): Observable<ListInputResponse> {
    return this.http.put<ListInputResponse>(`${this.api.apiUrl.inputs}/${'restore'}/${stt}`, data);
  }
  remove(stt: number): Observable<ListInputResponse> {
    return this.http.delete<ListInputResponse>(`${this.api.apiUrl.inputs}/${stt}`);
  }
}
