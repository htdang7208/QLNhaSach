import { Component, OnInit } from '@angular/core';
import { Customer, CustomerResponse, CustomerService, ChangePassword } from 'src/app/services/customer.service';
import { FormGroup, FormBuilder } from '@angular/forms';

declare var $: any;

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {

  kindSelected: string;
  customer: Customer = {} as Customer;
  list: Customer[] = [];
  listRemoved: Customer[] = [];
  message: String;
  fileChosen: string;
  form: FormGroup;
  imageTemplate: Node;
  passwordObject: ChangePassword = {} as ChangePassword;
  hasInform: boolean = false;

  constructor(private service: CustomerService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.createForm();
    this.loadData();
    this.loadDataRemoved();
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
      confirmPassword: [''],
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
    if (this.customer.id === undefined && this.customer.id === 0) {
      this.customer.password = this.form.get('password').value;
      this.customer.confirmPassword = this.form.get('confirmPassword').value;
    }
    this.customer.oldDept = this.form.get('oldDept').value;
    this.customer.nowDept = this.form.get('nowDept').value;

    this.customer.imageName = this.form.get('file').value;
  }
  prepareSave(): any {
    const formModel = this.form.value;
    let fd = new FormData();

    if (this.customer.id !== undefined && this.customer.id !== 0) {
      fd.append("id", this.customer.id.toString());
    } else {
      fd.append("password", formModel.password);
      fd.append("confirmPassword", formModel.confirmPassword);
    }

    fd.append("firstName", formModel.firstName);
    fd.append("lastName", formModel.lastName);
    fd.append("phone", formModel.phone);
    fd.append("email", formModel.email);
    fd.append("address", formModel.address);
    fd.append("username", formModel.username);
    // fd.append("password", formModel.password);
    fd.append("oldDept", formModel.oldDept);
    fd.append("nowDept", formModel.nowDept);
    fd.append("file", formModel.file);
    // fd.forEach(function (value, key, parent) {
    //   console.log(key + ':' + value);
    // });

    return fd;
  }
  onFileSelectCreate(files: FileList) {
    if (files && files[0].size > 0) {
      this.form.get('file').patchValue(files[0]);
      this.fileChosen = files[0].name;
    }
    if (document.getElementById('img-create-1') != null) {
      this.imageTemplate = document.getElementById('img-create-1');
      document.getElementById('img-create-1').hidden;
    }
    if (document.getElementById('img-create-2') != null) {
      document.getElementById('img-create-2').parentElement.hidden;
    }

    // Đọc lấy đường dẫn file đã chọn (base64String), tạo fragment rồi hiển thị lên
    // Vì trình duyệt ngăn việc lấy full path image

    for (var i = 0, f; f = files[i]; i++) {
      if (!f.type.match('image.*')) {
        continue;
      }
      let reader = new FileReader();
      reader.onload = (function (theFile) {
        return function (e) {
          var span = document.createElement('span');
          span.innerHTML = [
            '<img id="img-create-2" class="w-100 rounded img-fluid img-thumbnail" ',
            'src="', e.target.result, '" ',
            'title="', escape(theFile.name), '"/>'
          ].join('');
          document.getElementById('imageCreateFrame').innerHTML = "";
          document.getElementById('imageCreateFrame').insertBefore(span, null);
        };
      })(f);
      reader.readAsDataURL(f);
    }
  }
  onFileSelect(files: FileList) {
    if (files && files[0].size > 0) {
      this.form.get('file').patchValue(files[0]);
      this.fileChosen = files[0].name;
    }
    if (document.getElementById('img-update-1') != null) {
      this.imageTemplate = document.getElementById('img-update-1');
      document.getElementById('img-update-1').hidden;
    }
    if (document.getElementById('img-update-2') != null) {
      document.getElementById('img-update-2').parentElement.hidden;
    }

    // Đọc lấy đường dẫn file đã chọn (base64String), tạo fragment rồi hiển thị lên
    // Vì trình duyệt ngăn việc lấy full path image

    for (var i = 0, f; f = files[i]; i++) {
      if (!f.type.match('image.*')) {
        continue;
      }
      let reader = new FileReader();
      reader.onload = (function (theFile) {
        return function (e) {
          var span = document.createElement('span');
          span.innerHTML = [
            '<img id="img-update-2" class="w-100 rounded img-fluid img-thumbnail" ',
            'src="', e.target.result, '" ',
            'title="', escape(theFile.name), '"/>'
          ].join('');
          document.getElementById('imageUpdateFrame').innerHTML = "";
          document.getElementById('imageUpdateFrame').insertBefore(span, null);
        };
      })(f);
      reader.readAsDataURL(f);
    }
  }
  showModalEditCustomer(event: any = null, id: number = 0) {
    this.fileChosen = "Choose file";
    if (event) {
      event.preventDefault();
    }
    if (id > 0) {
      this.service.get(id).subscribe(response => { this.customer = response.data; });
    }
    else {
      this.customer = {} as Customer;
    }
  }
  showModalRemoveCustomer(id: number) {
    this.customer.id = id;
    this.message = "Bạn có thật sự muốn xóa khách hàng này?";
  }
  showModalChangePassword($event: any) {
    this.passwordObject = {} as ChangePassword;
  }
  showModalRestoreCustomer(id: number) {
    this.customer.id = id;
    this.message = "Bạn có chắc chắn muốn khôi phục khách hàng này?";
  }
  submitFormEditModal() {
    this.setUser();

    if (this.customer.id === undefined || this.customer.id === 0) {
      console.log("this.customer", this.customer);
      this.service.add(this.prepareSave()).subscribe(
        response => {
          switch (response.errorCode) {
            case 200: this.message = "Lưu thành công!"; break;
            case 404: this.message = "Không tìm thấy!"; break;
            case 601: this.message = "Username này đã tồn tại!"; break;
            case 602: this.message = "Số điện thoại này đã tồn tại!"; break;
            case 603: this.message = "Username Phone không được phép bỏ trống!"; break;
            case 605: this.message = "Mật khẩu không khớp!"; break;
          }
          this.loadData();
        }
      );
    } else {
      this.service.update(this.prepareSave(), this.customer.id).subscribe(
        response => {
          // console.log(this.customer);
          switch (response.errorCode) {
            case 200: this.message = "Lưu thành công!"; break;
            case 404: this.message = "Không tìm thấy!"; break;
            case 601: this.message = "Username này đã tồn tại!"; break;
            case 602: this.message = "Số điện thoại này đã tồn tại!"; break;
            case 603: this.message = "Username Phone không được phép bỏ trống!"; break;
          }
          this.loadData();
        }
      );
    }
  }
  closeCreateModal($event: any = null) {
    if (document.getElementById('img-create-2') != null) {
      document.getElementById('img-create-2').parentElement.remove();
    }
    if (this.imageTemplate != null) {
      document.getElementById('imageCreateFrame').appendChild(this.imageTemplate);
    }
  }
  closeEditModal($event: any = null) {
    if (document.getElementById('img-update-2') != null) {
      document.getElementById('img-update-2').parentElement.remove();
    }
    if (this.imageTemplate != null) {
      document.getElementById('imageUpdateFrame').appendChild(this.imageTemplate);
    }
  }
  loadData() {
    this.service.getAll().subscribe(response => { this.list = response.data; });
  }
  loadDataRemoved() {
    this.service.getAllRemoved().subscribe(response => { this.listRemoved = response.data; });
  }
  restoreCustomer(id: number) {
    this.service.restoreCustomer(id).subscribe(response => {
      this.loadData();
      this.loadDataRemoved();
    });
  }
  removeCustomer(id: number) {
    this.service.remove(id).subscribe(response => {
      $('#modal-remove').modal('hide');
      this.loadData();
      this.loadDataRemoved();
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
  changePasword($event: any) {
    this.service.updatePassword(this.passwordObject, this.customer.id).subscribe(
      response => {
        switch (response.errorCode) {
          case 200: this.hasInform = true; this.message = "Cập nhật mật khẩu thành công!"; break;
          case 603: this.hasInform = true; this.message = "Không được để trống!"; break;
          case 604: this.hasInform = true; this.message = "Mật khẩu cũ không đúng!"; break;
          case 605: this.hasInform = true; this.message = "Xác nhận mật khẩu không khớp!"; break;
        }
        console.log(this.message);
      }
    );
  }
  changeStageInformPasword($event = null) {
    this.hasInform = false;
  }
}