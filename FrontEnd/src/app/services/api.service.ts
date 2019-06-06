import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor() { }

  baseUrl = "https://localhost:44327/api/";
  apiUrl = {
    admins: this.baseUrl + "admins"
  }
}
