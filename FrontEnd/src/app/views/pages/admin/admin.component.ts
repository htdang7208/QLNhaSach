import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { AdminService, Admin, AdminResponse } from 'src/app/services/admin.service';

declare var $: any;

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  admin: Admin = {} as Admin;
  list: Admin[] = [];
  res: AdminResponse = {} as AdminResponse;
  message: String;
  fileChosen: string = "Choose file";
  form: FormGroup;

  constructor(private service: AdminService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.createForm();
    this.loadData();
  }
  createForm() {
    this.form = this.formBuilder.group({
      name: [''],
      email: [''],
      username: [''],
      password: [''],
      file: null
    });
  }
  setUser() {
    this.admin.name = this.form.get('name').value;
    this.admin.email = this.form.get('email').value;
    this.admin.username = this.form.get('username').value;
    this.admin.password = this.form.get('password').value;
    this.admin.imageName = this.form.get('file').value;
  }
  public prepareSave(): any {
    const formModel = this.form.value;
    let fd = new FormData();

    fd.append("id", this.admin.id.toString());
    fd.append("name", formModel.name);
    fd.append("email", formModel.email);
    fd.append("username", formModel.username);
    fd.append("password", formModel.password);
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
    // Đọc lấy đường dẫn file đã chọn (base64String), tạo fragment rồi hiển thị lên
    // Vì trình duyệt ngăn việc lấy full path image
    //
    // for (var i = 0, f; f = files[i]; i++) {
    //   if (!f.type.match('image.*')) {
    //     continue;
    //   }
    //   let reader = new FileReader();
    //   reader.onload = (function (theFile) {
    //     return function (e) {
    //       var span = document.createElement('span');
    //       span.innerHTML = [
    //         '<img class="w-100 rounded img-fluid img-thumbnail" ',
    //         'src="', e.target.result, '" ',
    //         'title="', escape(theFile.name), '"/>'
    //       ].join('');
    //       document.getElementById('imageFrame').innerHTML = "";
    //       document.getElementById('imageFrame').insertBefore(span, null);
    //     };
    //   })(f);
    //   reader.readAsDataURL(f);
    // }
  }
  onSubmit() {
    this.setUser();

    if (this.admin.id === undefined || this.admin.id === 0) {
      this.service.add(this.prepareSave()).subscribe(
        response => {
          switch (response.errorCode) {
            case 0: this.message = "Thêm admin thành công!"; break;
            case 1: this.message = "Admin này đã tồn tại rồi!"; break;
            case 6: this.message = "Username này đã tồn tại!"; break;
          }
          this.loadData();
        }
      );
    } else {
      this.service.update(this.prepareSave(), this.admin.id).subscribe(
        response => {
          switch (response.errorCode) {
            case 0: this.message = "Cập nhật admin thành công!"; break;
            case 2: this.message = "Không tìm thấy admin"; break;
            case 6: this.message = "Username này đã tồn tại"; break;
          }
          this.loadData();
        }
      );
    }
    // $.ajax({
    //   type: "POST",
    //   url: "/api/images",
    //   contentType: false,
    //   processData: false,
    //   data: data,
    //   success: function (results) {
    //     for (i = 0; i < results.length; i++) {
    //       alert(results[i]);
    //     }
    //   }
    // });
  }
  loadData() {
    this.service.getAll().subscribe(response => { this.list = response.data; });
  }
  getAdmin(id: number) {
    this.service.get(id).subscribe(response => { this.admin = response.data; });
  }
  OK($event: any) {
    // $('#modal-edit').modal('hide');
    $('#modal-inform').modal('hide');
  }
  removeAdmin(id: number) {
    this.admin.id = id;
    this.message = "Bạn có thật sự muốn xóa Admin này?";
  }
  remove(id: number) {
    this.service.remove(id).subscribe(response => {
      switch (response.errorCode) {
        case 0: this.message = "Xóa admin thành công!"; break;
        case 2: this.message = "Không tìm thấy admin"; break;
      }
      this.loadData();
      $('#modal-remove').modal('hide');
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
  handleFileSelect($event: any) {
    this.fileChosen = $event.target.files[0].name;
    var files = $event.target.files;
    for (var i = 0, f; f = files[i]; i++) {
      // Only process image files.
      if (!f.type.match('image.*')) {
        continue;
      }

      var reader = new FileReader();

      // Closure to capture the file information.
      reader.onload = (function (theFile) {
        return function (e) {
          var span = document.createElement('span');
          span.innerHTML = [
            '<img class="w-100 rounded img-fluid img-thumbnail" ',
            'src="', e.target.result, '" ',
            'title="', escape(theFile.name), '"/>'
          ].join('');
          document.getElementById('imageFrame').innerHTML = "";
          document.getElementById('imageFrame').insertBefore(span, null);
        };
      })(f);
      // Read in the image file as a data URL.
      reader.readAsDataURL(f);
    }
    // console.log(document.querySelector('#imageFrame span > img'));
  }
}
