import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerAccountComponent } from './customer-account/customer-account.component';
import { Title } from '@angular/platform-browser';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProductComponent } from './product/product.component';
import { InputInvoiceComponent } from './input-invoice/input-invoice.component';
import { SaleInvoiceComponent } from './sale-invoice/sale-invoice.component';
import { ReceiptComponent } from './receipt/receipt.component';
import { BookReportComponent } from './book-report/book-report.component';
import { DeptReportComponent } from './dept-report/dept-report.component';

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
