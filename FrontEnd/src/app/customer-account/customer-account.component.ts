import { Component, OnInit } from '@angular/core';
import { Customer, CustomerService } from '../services/customer.service';

@Component({
  selector: 'app-customer-account',
  templateUrl: './customer-account.component.html',
  styleUrls: ['./customer-account.component.css']
})
export class CustomerAccountComponent implements OnInit {

  customer: Customer = {} as Customer;
  listCustomer: Customer[] = [];
  constructor(private service: CustomerService) { }

  ngOnInit() { this.loadData(); }

  loadData() {
    this.service.getAll().subscribe(
      result => {
        this.listCustomer = result.data;
      });
  }
  editCustomer(id: number = 0) {
    if (id > 0)
      this.service.get(id).subscribe(
        result => {
          this.customer = result.data;
          console.log(this.customer);
        });
    else this.customer = {} as Customer;
  }
}
