import { Component, OnInit } from '@angular/core';
import { Customer, CustomerResponse, CustomerService } from 'src/app/services/customer.service';
import { FormGroup, FormBuilder } from '@angular/forms';

declare var $: any;

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {

  customer: Customer = {} as Customer;
  list: Customer[] = [];
  res: CustomerResponse = {} as CustomerResponse;
  message: String;
  fileChosen: string = "Choose file";
  form: FormGroup;

  constructor(private service: CustomerService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.createForm();
    this.loadData();
  }
  createForm() {
    this.form = this.formBuilder.group({
      firstName: [''],
      lastName: [''],
      phone: [''],
      email: [''],
      address: [''],
      username: [''],
      password: [''],
      oldDept: 0,
      nowDept: 0,

      file: null
    });
  }
  setUser() {
    this.customer.firstName = this.form.get('firstName').value;
    this.customer.lastName = this.form.get('lastName').value;
    this.customer.phone = this.form.get('phone').value;
    this.customer.email = this.form.get('email').value;
    this.customer.address = this.form.get('address').value;
    this.customer.username = this.form.get('username').value;
    this.customer.password = this.form.get('password').value;
    this.customer.oldDept = this.form.get('oldDept').value;
    this.customer.nowDept = this.form.get('nowDept').value;

    this.customer.imageName = this.form.get('file').value;
  }
  public prepareSave(): any {
    const formModel = this.form.value;
    let fd = new FormData();

    fd.append("id", this.customer.id.toString());
    fd.append("firstName", formModel.firstName);
    fd.append("lastName", formModel.lastName);
    fd.append("phone", formModel.phone);
    fd.append("email", formModel.email);
    fd.append("address", formModel.address);
    fd.append("username", formModel.username);
    fd.append("password", formModel.password);
    fd.append("oldDept", formModel.oldDept);
    fd.append("nowDept", formModel.nowDept);
    fd.append("file", formModel.file);
    // fd.forEach(function (value, key, parent) {
    //   console.log(key + ':' + value);
    // });

    return fd;
  }
  // thay đổi sau mỗi lần nhập file
  public onFileSelect(files: FileList) {
    if (files && files[0].size > 0) {
      this.form.get('file').patchValue(files[0]);
    }
  }
  onSubmit() {
    this.setUser();

    if (this.customer.id === undefined || this.customer.id === 0) {
      this.service.add(this.prepareSave()).subscribe(
        response => {
          switch (response.errorCode) {
            case 200: this.message = response.message; break;
            case 404: this.message = response.message; break;
            case 601: this.message = response.message; break;
            case 602: this.message = response.message; break;
            case 603: this.message = response.message; break;
          }
          this.loadData();
        }
      );
    } else {
      this.service.update(this.prepareSave(), this.customer.id).subscribe(
        response => {
          switch (response.errorCode) {
            case 200: this.message = response.message; break;
            case 404: this.message = response.message; break;
            case 601: this.message = response.message; break;
            case 602: this.message = response.message; break;
            case 603: this.message = response.message; break;
          }
          this.loadData();
        }
      );
    }
  }
  loadData() {
    this.service.getAll().subscribe(response => { this.list = response.data; });
  }
  getCustomer(id: number) {
    this.service.get(id).subscribe(response => { this.customer = response.data; });
  }
  OK($event: any) {
    $('#modal-inform').modal('hide');
  }
  removeCustomer(id: number) {
    this.customer.id = id;
    this.message = "Bạn có thật sự muốn xóa customer này?";
  }
  remove(id: number) {
    this.service.remove(id).subscribe(response => {
      switch (response.errorCode) {
        case 200: this.message = response.message; break;
      }
      this.loadData();
      $('#modal-remove').modal('hide');
    });
  }
  search($event: any) {
    let content = $('#search-info').val();
    let item = 0;
    for (item; item < this.list.length; item++) {
      if (this.list[item].firstName === content ||
        this.list[item].lastName === content ||
        this.list[item].username === content) {
        this.customer = this.list[item];
        break;
      }
    }
    if (item === this.list.length) {
      this.message = "Không tìm thấy.";
      $('#modal-inform').modal('show');
    } else {
      $('#modal-search').modal('show');
    }
  }
}