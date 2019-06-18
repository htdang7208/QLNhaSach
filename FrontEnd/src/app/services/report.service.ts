import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface BookReport {
  name: string,
  oldStock: number,
  nowStock: number,
  additionalStock: number
}
export interface BookReportResponse {
  errorCode: number,
  message: string,
  data: BookReport
}
export interface DeptReport {
  name: string,
  oldDept: number,
  nowDept: number,
  additionalDept: number
}
export interface DeptReportResponse {
  errorCode: number,
  message: string,
  data: DeptReport
}
export interface Roles {
  maxBookStock: number,
  minBookInput: number,
  getOverDept: boolean,
  deptOver: number,
  stockMax: number
}
@Injectable({
  providedIn: 'root'
})
export class ReportService {

  constructor(private api: ApiService, private http: HttpClient) { }
  getBookReport(): Observable<BookReportResponse> {
    return this.http.get<BookReportResponse>(`${this.api.apiUrl.bookReports}`);
  }
  getDeptReport(): Observable<DeptReportResponse> {
    return this.http.get<DeptReportResponse>(`${this.api.apiUrl.deptReports}`);
  }
  getRoles(): Observable<Roles> {
    return this.http.get<Roles>(`${this.api.apiUrl.changeRoles}`);
  }
  changeRoles(data: Roles) {
    return this.http.post(`${this.api.apiUrl.changeRoles}`, data);
  }
}
