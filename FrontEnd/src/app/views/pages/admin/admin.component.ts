import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { AdminService, Admin, AdminResponse, ChangePassword } from 'src/app/services/admin.service';

declare var $: any;

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  admin: Admin = {} as Admin;
  list: Admin[] = [];
  listRemoved: Admin[] = [];
  message: String;
  fileChosen: string = "Choose file";
  form: FormGroup;
  imageTemplate: Node;
  passwordObject: ChangePassword = {} as ChangePassword;
  hasInform: boolean = false;

  constructor(private service: AdminService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.createForm();
    this.loadData();
    this.loadDataRemoved();
  }
  createForm() {
    this.form = this.formBuilder.group({
      name: [''],
      email: [''],
      username: [''],
      password: [''],
      confirmPassword: [''],
      file: null
    });
  }
  setUser() {
    this.admin.name = this.form.get('name').value;
    this.admin.email = this.form.get('email').value;
    this.admin.username = this.form.get('username').value;
    if (this.admin.id === undefined && this.admin.id === 0) {
      this.admin.password = this.form.get('password').value;
      this.admin.confirmPassword = this.form.get('confirmPassword').value;
    }
    this.admin.imageName = this.form.get('file').value;
  }
  prepareSave(): any {
    const formModel = this.form.value;
    let fd = new FormData();

    if (this.admin.id !== undefined && this.admin.id !== 0) {
      fd.append("id", this.admin.id.toString());
    } else {
      fd.append("password", formModel.password);
      fd.append("confirmPassword", formModel.confirmPassword);
    }

    fd.append("name", formModel.name);
    fd.append("email", formModel.email);
    fd.append("username", formModel.username);
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
  showModalEditAdmin(event: any = null, id: number = 0) {
    this.fileChosen = "Choose file";
    if (event) {
      event.preventDefault();
    }
    if (id > 0) {
      this.service.get(id).subscribe(response => { this.admin = response.data; });
    }
    else {
      this.admin = {} as Admin;
    }
  }
  showModalRemoveAdmin(id: number) {
    this.admin.id = id;
    this.message = "Bạn có thật sự muốn xóa Admin này?";
  }
  showModalChangePassword($event: any) {
    this.passwordObject = {} as ChangePassword;
  }
  showModalRestoreAdmin(id: number) {
    this.admin.id = id;
    this.message = "Bạn có chắc chắn muốn khôi phục admin này?";
  }
  submitFormEditModal() {
    this.setUser();

    if (this.admin.id === undefined || this.admin.id === 0) {
      this.service.add(this.prepareSave()).subscribe(
        response => {
          switch (response.errorCode) {
            case 200: this.message = "Lưu thành công!"; break;
            case 404: this.message = "Không tìm thấy!"; break;
            case 601: this.message = "Username này đã tồn tại!"; break;
            case 603: this.message = "Username\nPassword\nKhông được phép bỏ trống!"; break;
            case 605: this.message = "Password không khớp!"; break;
          }
          this.loadData();
        }
      );
    } else {
      this.service.update(this.prepareSave(), this.admin.id).subscribe(
        response => {
          switch (response.errorCode) {
            case 200: this.message = "Lưu thành công!"; break;
            case 404: this.message = "Không tìm thấy!"; break;
            case 601: this.message = "Username này đã tồn tại!"; break;
            case 603: this.message = "Username\nPassword\nKhông được phép bỏ trống!"; break;
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
    this.service.getAllRemoved().subscribe(response => { this.listRemoved = response.data; console.log(this.listRemoved); });
  }
  restoreAdmin(id: number) {
    this.service.restoreAdmin(id).subscribe(response => {
      this.loadData();
      this.loadDataRemoved();
    });
  }
  removeAdmin(id: number) {
    this.service.remove(id).subscribe(response => {
      this.loadData();
      $('#modal-remove').modal('hide');
      this.loadDataRemoved();
    });
  }
  search($event: any) {
    let content = $('#search-info').val();
    let item = 0;
    for (item; item < this.list.length; item++) {
      if (this.list[item].name === content || this.list[item].username === content) {
        this.admin = this.list[item];
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
    this.service.updatePassword(this.passwordObject, this.admin.id).subscribe(
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