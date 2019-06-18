import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Book {
  id: number;
  name: string;
  kind: string;
  author: string;
  price: number;
  stock: number;
  imageName: string,
  url: string,
  isRemove: boolean
}
export interface BookResponse {
  data: Book;
  errorCode: number;
  message: string;
}
export interface ListBookResponse {
  data: Book[];
  errorCode: number;
  message: string;
}
export interface ListBookResponseRemoved {
  data: Book[];
  errorCode: number;
  message: string;
}
export interface ListBookResponseSearched {
  data: Book[];
  errorCode: number;
  message: string;
}
@Injectable({
  providedIn: 'root'
})
export class BookService {

  constructor(private api: ApiService, private http: HttpClient) { }
  search(queryString: string): Observable<ListBookResponseSearched> {
    let URL = `${this.api.apiUrl.books}/${'search?&q='}${queryString}`;
    return this.http.get<ListBookResponseSearched>(URL);
  }
  getAll(): Observable<ListBookResponse> {
    let URL = `${this.api.apiUrl.books}`;
    return this.http.get<ListBookResponse>(URL);
  }
  getAllRemoved(): Observable<ListBookResponseRemoved> {
    return this.http.get<ListBookResponseRemoved>(`${this.api.apiUrl.books}/${'bookRemoved'}`);
  }
  get(id: number): Observable<BookResponse> {
    return this.http.get<BookResponse>(`${this.api.apiUrl.books}/${id}`);
  }
  update(data: FormData, id: number): Observable<BookResponse> {
    return this.http.put<BookResponse>(`${this.api.apiUrl.books}/${id}`, data);
  }
  remove(id: number): Observable<BookResponse> {
    return this.http.delete<BookResponse>(`${this.api.apiUrl.books}/${id}`);
  }
  restoreBook(id: number, data: Book = null): Observable<BookResponse> {
    return this.http.put<BookResponse>(`${this.api.apiUrl.books}/${'restore'}/${id}`, data);
  }
}
