import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/services/admin.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  ngOnInit(): void {
    
  }

  message = '';
  username = '';
  password = '';

  constructor(private userService: AdminService, private router: Router,
    ) { }
  // login() {
  //   this.userService.login(this.username, this.password)
  //     .subscribe(result => {

  //       if (result.errorCode === 1) {
  //         this.message = result.errorMessage;
  //       } else {
  //         this.message = '';
  //         this.authService.setLoggIn(true);
  //         //save cookie
  //         this.cookieService.set('userid', result.data.userId + '');
  //         this.cookieService.set('token', result.data.accessToken);
  //         this.router.navigate(['/dashboard']);
  //       }
  //     });
  // }
}
