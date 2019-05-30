import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CustomerAccountComponent } from './customer-account/customer-account.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProductComponent } from './product/product.component';
import { InputInvoiceComponent } from './input-invoice/input-invoice.component';
import { SaleInvoiceComponent } from './sale-invoice/sale-invoice.component';
import { ReceiptComponent } from './receipt/receipt.component';
import { BookReportComponent } from './book-report/book-report.component';
import { DeptReportComponent } from './dept-report/dept-report.component';
import { RouterModule } from '@angular/router';
import { NOTFOUND } from 'dns';

@NgModule({
  declarations: [
    AppComponent,
    CustomerAccountComponent,
    DashboardComponent,
    ProductComponent,
    InputInvoiceComponent,
    SaleInvoiceComponent,
    ReceiptComponent,
    BookReportComponent,
    DeptReportComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: DashboardComponent, data: { title: "Home" } },
      { path: 'customer-account', component: CustomerAccountComponent, data: { title: "CustomerAccountComponent" } },
      { path: 'product', component: ProductComponent, data: { title: "ProductComponent" } },
      { path: 'inputInvoice', component: InputInvoiceComponent, data: { title: "InputInvoiceComponent" } },
      { path: 'saleInvoice', component: SaleInvoiceComponent, data: { title: "SaleInvoiceComponent" } },
      { path: 'receipt', component: ReceiptComponent, data: { title: "ReceiptComponent" } },
      { path: 'book-report', component: BookReportComponent, data: { title: "BookReportComponent" } },
      { path: 'dept-report', component: DeptReportComponent, data: { title: "DeptReportComponent" } }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
