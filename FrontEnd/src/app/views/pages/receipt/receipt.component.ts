import { Component, OnInit } from '@angular/core';
import { ReceiptService, CustomerReceiptInfo } from 'src/app/services/receipt.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ChangePassword } from 'src/app/services/admin.service';
declare var $: any;
@Component({
  selector: 'app-receipt',
  templateUrl: './receipt.component.html',
  styleUrls: ['./receipt.component.css']
})
export class ReceiptComponent implements OnInit {
  form: FormGroup;
  receipt: CustomerReceiptInfo = {} as CustomerReceiptInfo;
  message: string;
  imageTemplate: any;
  list: CustomerReceiptInfo[];
  listRemoved: CustomerReceiptInfo[];

  constructor(private service: ReceiptService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.createForm();
    this.loadData();
    this.loadDataRemoved();
  }
  createForm() {
    this.form = this.formBuilder.group({
      customerId: 0,
      firstName: [''],
      lastName: [''],
      phone: [''],
      email: [''],
      address: [''],
      dateCreated: [''],
      customerPaid: 0,
      total: 0,
      dept: 0
    });
  }
  setUser() {
    this.receipt.firstName = this.form.get('firstName').value;
    this.receipt.lastName = this.form.get('lastName').value;
    this.receipt.phone = this.form.get('phone').value;
    this.receipt.email = this.form.get('email').value;
    this.receipt.address = this.form.get('address').value;
    this.receipt.dateCreated = this.form.get('dateCreated').value;
    this.receipt.customerPaid = this.form.get('customerPaid').value;
    this.receipt.total = this.form.get('total').value;
    this.receipt.dept = this.form.get('dept').value;
  }
  prepareSave(): any {
    const formModel = this.form.value;
    let fd = new FormData();

    if (this.receipt.id !== undefined && this.receipt.id !== 0) {
      fd.append("id", this.receipt.id.toString());
    }

    fd.append("firstName", formModel.firstName);
    fd.append("lastName", formModel.lastName);
    fd.append("phone", formModel.phone);
    fd.append("email", formModel.email);
    fd.append("address", formModel.address);
    fd.append("dateCreated", formModel.dateCreated);
    fd.append("customerPaid", formModel.customerPaid);
    fd.append("total", formModel.total);
    fd.append("dept", formModel.dept);

    return fd;
  }
  showModalEditreceipt(event: any = null, id: number = 0) {
    if (event) {
      event.preventDefault();
    }
    if (id > 0) {
      this.service.get(id).subscribe(response => { this.receipt = response.data; });
    }
    else {
      this.receipt = {} as CustomerReceiptInfo;
    }
  }
  showModalRemovereceipt(id: number) {
    this.receipt.id = id;
    this.message = "Bạn có thật sự muốn xóa phiếu này?";
  }
  showModalRestorereceipt(id: number) {
    this.receipt.id = id;
    this.message = "Bạn có chắc chắn muốn khôi phục phiếu này?";
  }
  submitFormEditModal() {
    this.setUser();

    if (this.receipt.id === undefined || this.receipt.id === 0) {
      this.service.add(this.prepareSave()).subscribe(
        response => {
          switch (response.errorCode) {
            case 200: this.message = "Lưu thành công!"; break;
            case 23: this.message = "Số điện thoại này đã tồn tại!"; break;
            case 24: this.message = "Số tiền thanh toán không được bỏ trống!"; break;
            case 25: this.message = "Số tiền khách đưa không được bỏ trống!"; break;
            case 20: this.message = "Số tiền thu không được vượt quá số tiền khách đang nợ!"; break;
          }
          this.loadData();
        }
      );
    } else {
      this.service.update(this.prepareSave(), this.receipt.id).subscribe(
        response => {
          console.log(this.receipt);
          switch (response.errorCode) {
            case 200: this.message = "Lưu thành công!"; break;
            case 23: this.message = "Số điện thoại này đã tồn tại!"; break;
            case 24: this.message = "Số tiền thanh toán không được bỏ trống!"; break;
            case 25: this.message = "Số tiền khách đưa không được bỏ trống!"; break;
            case 20: this.message = "Số tiền thu không được vượt quá số tiền khách đang nợ!"; break;
          }
          this.loadData();
        }
      );
    }
  }
  loadData() {
    this.service.getAll().subscribe(response => { 
      this.list = response.data;
     });
  }
  loadDataRemoved() {
    this.service.getAllRemoved().subscribe(response => { this.listRemoved = response.data; console.log("aa:", this.listRemoved); });
  }
  restorereceipt(id: number) {
    this.service.restorereceipt(id).subscribe(response => {
      this.loadData();
      this.loadDataRemoved();
    });
  }
  removereceipt(id: number) {
    this.service.remove(id).subscribe(response => {
      $('#modal-remove').modal('hide');
      this.loadData();
      this.loadDataRemoved();
    });
  }
}
