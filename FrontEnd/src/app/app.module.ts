import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';

import { RouterModule } from '@angular/router'
import { FormsModule } from '@angular/forms';
import { DashboardComponent } from './views/dashboard/dashboard.component';
import { AdminComponent } from './views/pages/admin/admin.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { CustomerComponent } from './views/pages/customer/customer.component';
import { BookComponent } from './views/pages/book/book.component';
import { InputComponent } from './views/pages/input/input.component';
import { ReceiptComponent } from './views/pages/receipt/receipt.component';
import { BookReportComponent } from './views/pages/book-report/book-report.component';
import { LoginComponent } from './views/login/login.component';
import { SaleComponent } from './views/pages/sale/sale.component';
import { DeptReportComponent } from './views/pages/dept-report/dept-report.component';
import { RoleComponent } from './views/pages/role/role.component';

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    DashboardComponent,
    CustomerComponent,
    BookComponent,
    InputComponent,
    ReceiptComponent,
    BookReportComponent,
    LoginComponent,
    SaleComponent,
    DeptReportComponent,
    RoleComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      {path: 'admin', component: AdminComponent, data: {title: "Admin"}},
      {path: 'customer', component: CustomerComponent, data: {title: "Customer"}},
      {path: 'book', component: BookComponent, data: {title: "Book"}},
      {path: 'input', component: InputComponent, data: {title: "Input"}},
      {path: 'sale', component: SaleComponent, data: {title: "Sale"}},
      {path: 'receipt', component: ReceiptComponent, data: {title: "Receipt"}},
      {path: 'book-report', component: BookReportComponent, data: {title: "Book Report"}},
      {path: 'dept-report', component: BookReportComponent, data: {title: "Dept Report"}},
      {path: 'role', component: RoleComponent, data: {title: "Role"}}
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
