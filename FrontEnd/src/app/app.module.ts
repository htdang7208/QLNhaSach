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

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    DashboardComponent,
    CustomerComponent,
    BookComponent,
    InputComponent,
    ReceiptComponent
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
      {path: 'receipt', component: ReceiptComponent, data: {title: "Receipt"}}
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
