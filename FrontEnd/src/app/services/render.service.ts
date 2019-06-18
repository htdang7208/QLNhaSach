import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RenderService {

  // constructor(private api: ApiService, private renderService: RenderService, private http: HttpClient, private cookie: CookieService) { }
  
  getAll() {
    // const header = new HttpHeaders({
    //   'Authorization': 'Bearer ' + this.cookie.get('token')
    // });
    // return this.http.get(this.api.apiUrl.readers);
    // return this.http.get(this.api.apiUrl.readers, {headers: header});
  }
}
