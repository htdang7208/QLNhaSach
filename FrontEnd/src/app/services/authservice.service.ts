import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthserviceService {

  private loggedInStatus = JSON.parse(localStorage.getItem('loggedIn' || 'false'));
  constructor() { }
  setLoggIn(value: boolean) {
    this.loggedInStatus = value;
    localStorage.setItem('loggedIn', value.toString());
  }
  get isLoggedIn() {
    return this.loggedInStatus;
  }
}
