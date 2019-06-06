import { Component, OnInit } from '@angular/core';
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
  data: FormData = {} as FormData;
  message: String;

  constructor(private service: AdminService) { }

  ngOnInit() {
    this.loadData();
  }
  loadData() {
    this.service.getAll().subscribe(response => { this.list = response.data; });
  }
  getAdmin(id: number) {
    this.service.get(id).subscribe(response => { this.admin = response.data; });
  }
  save($event = null) {
    if (this.admin.id === undefined || this.admin.id === 0) {

      this.admin.dept = 0;
      this.admin.isAdmin = true;
      this.service.add(this.admin).subscribe(response => {
        if (response.errorCode === 1) { this.message = "Admin này đã tồn tại rồi!" }
        else if (response.errorCode === 0) { this.message = "Thêm admin thành công!" }
        this.loadData();
      });
    } else {
      this.service.update(this.admin).subscribe(response => {
        if (response.errorCode === 2) { this.message = "Không tìm thấy admin" }
        else if (response.errorCode === 0) { this.message = "Cập nhật admin thành công!" }
        this.loadData();
      });
    }
  }
  OK($event: any) {
    $('#modal-edit').modal('hide');
    $('#modal-inform').modal('hide');
  }
  removeAdmin(id: number) {
    this.admin.id = id;
    this.message = "Bạn có thật sự muốn xóa Admin này?";
  }
  remove(id: number) {
    this.service.remove(id).subscribe(response => {
      if (response.errorCode === 2) { this.message = "Không tìm thấy admin này!" }
      else if (response.errorCode === 0) { this.message = "Xóa admin thành công!" }
      this.loadData();
      $('#modal-remove').modal('hide');
    });
  }
  search($event: any) {
    let content = $('#search-info').val();
    let item = 0;
    for (item; item < this.list.length; item++) {
      if (this.list[item].firstName === content || this.list[item].username === content) {
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
}
