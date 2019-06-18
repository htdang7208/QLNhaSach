import { Component, OnInit } from '@angular/core';
import { InputService, Input, ListInput } from 'src/app/services/input.service';

declare var $: any;
@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.css']
})
export class InputComponent implements OnInit {

  message: string;
  input: Input = {} as Input;
  listStt: ListInput = {} as ListInput;
  list: ListInput = {} as ListInput;
  listRemoved: ListInput = {} as ListInput;
  stt: number;

  constructor(private service: InputService) { }

  ngOnInit() {
    this.loadData();
    this.loadDataRemoved();
  }
  loadData() {
    this.service.getAll().subscribe(
      response => {
        this.list = response.data;
      });
  }
  loadDataRemoved() {
    this.service.getAllRemoved().subscribe(
      response => {this.listRemoved = response.data}
    );
  }
  showModalEdit(stt: number = 0) {
    if (event) {
      event.preventDefault();
    }
    if (stt > 0) {
      this.service.get(stt).subscribe(
        response => this.listStt = response.data);
    }
    else {
      this.input = {} as Input;
      this.listStt = {} as ListInput;
    }
  }
  update(stt: number = 0) {
    if (stt > 0) {
      return this.service.update(this.listStt, stt).subscribe(
        response => {
          switch (response.errorCode) {

            case 200: this.message = "Cập nhật thành công!"; break;
            case 9: this.message = "Lượng tồn kho phải ít hơn " + response.message + "!"; break;
            case 10: this.message = "Số lượng sách nhập phải nhập ít nhất là " + response.message + "!"; break;
            case 11: this.message = "Tên sách không được để trống!"; break;
            case 13: this.message = "Thể loại sách không được để trống!"; break;
            case 14: this.message = "Tên tác giả không được để trống!"; break;
            case 15: this.message = "Số lượng sách trong kho không được để trống!"; break;
            case 16: this.message = "Đã tồn tại quyển sách khác với tên này!"; break;
          }
        }
      );
    }
    // else {
    //   return this.service.update(this.listStt).subscribe(
    //     response => {
    //       switch (response.errorCode) {

    //         case 200: this.message = "Cập nhật thành công!"; break;
    //         case 9: this.message = "Lượng tồn kho phải ít hơn " + response.message + "!"; break;
    //         case 10: this.message = "Số lượng sách nhập phải nhập ít nhất là " + response.message + "!"; break;
    //         case 11: this.message = "Tên sách không được để trống!"; break;
    //         case 13: this.message = "Thể loại sách không được để trống!"; break;
    //         case 14: this.message = "Tên tác giả không được để trống!"; break;
    //         case 15: this.message = "Số lượng sách trong kho không được để trống!"; break;
    //         case 16: this.message = "Đã tồn tại quyển sách khác với tên này!"; break;
    //       }
    //     }
    //   );
    // }
  }

  showModalRemove(stt: number) {
    console.log(stt);
    this.stt = stt;
    this.message = "Bạn có thật sự muốn xóa khách hàng này?";
  }
  showModalRestore(stt: number) {
    console.log(stt);
    this.stt = stt;
    this.message = "Bạn có chắc chắn muốn khôi phục khách hàng này?";
  }

  // closeEditModal($event: any = null) {

  // }

  restore(stt: number) {
    this.service.restoredInput(stt).subscribe(response => {
      this.loadData();
      this.loadDataRemoved();
    });
  }
  remove(stt: number) {
    this.service.remove(stt).subscribe(response => {
      $('#modal-remove').modal('hide');
      this.loadData();
      this.loadDataRemoved();
    });
  }
}