import { Component, OnInit } from '@angular/core';
import { SaleService, Sale, SaleDetail } from 'src/app/services/sale.service';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';

declare var $: any;
@Component({
  selector: 'app-sale',
  templateUrl: './sale.component.html',
  styleUrls: ['./sale.component.css']
})
export class SaleComponent implements OnInit {
  formSale: FormGroup;
  message: string;
  listRemoved: Sale[];

  constructor(private service: SaleService, private formBuilder: FormBuilder) { }

  listSale: Sale[] = [];
  sale: Sale = {} as Sale;
  saleDetail: SaleDetail = {} as SaleDetail;
  listSaleDetail: SaleDetail[] = [];
  ngOnInit() {
    this.loadData();
    this.loadDataRemoved();
    this.createForm();
  }
  loadData() {
    this.service.getAll().subscribe(response => {
      this.listSale = response.data;
    });
  }
  showModalEdit(saleId: number = 0) {
    if (saleId > 0) {
      this.service.get(saleId).subscribe(response => {
        this.sale = response.data;
      });
    }
    this.sale = {} as Sale;
  }
  showModalRemove(saleId: number) {
    this.sale.saleId = saleId;
    this.message = "Bạn có thật sự muốn xóa hóa đơn này?";
  }
  showModalRestore(saleId: number) {
    this.sale.saleId = saleId;
    this.message = "Bạn có chắc chắn muốn khôi phục hóa đơn này?";
  }
  restore(saleId: number) {
    this.service.restore(saleId).subscribe(response => {
      this.loadData();
      this.loadDataRemoved();
    });
  }
  remove(saleId: number) {
    this.service.remove(saleId).subscribe(response => {
      $('#modal-remove').modal('hide');
      this.loadData();
      this.loadDataRemoved();
    });
  }
  loadDataRemoved() {
    this.service.getAllRemoved().subscribe(response => { 
      this.listRemoved = response.data; console.log(this.listRemoved);
    });
  }
  createForm() {
  //   this.formSale = new FormGroup({
  //     firstName: new FormControl()
  //  });
  //   this.formSale = this.formBuilder.group({
  //     name: [''],
  //     email: [''],
  //     username: [''],
  //     password: [''],
  //     file: null
  //   });
  }
  setForm() {
    // binding dữ liệu với formControlName
    // this.sale.firstName = this.formSale.get('firstName').value;
    // this.sale.lastName = this.formSale.get('lastName').value;
    // this.sale.dateCreated = this.formSale.get('dateCreated').value;
  }
}
