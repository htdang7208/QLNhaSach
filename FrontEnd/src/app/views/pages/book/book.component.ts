import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { BookService, Book } from 'src/app/services/book.service';
import { isString } from 'util';
declare var $: any;
@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit {
  form: any;
  book: Book = {} as Book;
  fileChosen: string;
  imageTemplate: HTMLElement;
  message: string;
  selectedName: string = "";
  selectedKind: string = "";
  listSearch: Book[];
  listRemoved: Book[];
  list: Book[];

  constructor(private formBuilder: FormBuilder, private service: BookService) { }

  ngOnInit() {
    this.createForm();
    this.loadData();
    this.loadDataRemoved();
  }
  loadData() {
    this.service.getAll().subscribe(response => {
      this.list = response.data;
      // console.log("loaddata: ", this.list);
    });
  }
  loadDataRemoved() {
    this.service.getAllRemoved().subscribe(response => {
      this.listRemoved = response.data;
      // console.log("loadRemoveddata: ", this.listRemoved);
    });
  }
  createForm() {
    this.form = this.formBuilder.group({
      name: [''],
      kind: [''],
      stock: [''],
      author: [''],
      price: 0,

      file: null
    });
  }
  setUser() {
    this.book.name = this.form.get('name').value;
    this.book.kind = this.form.get('kind').value;
    this.book.author = this.form.get('author').value;
    this.book.stock = this.form.get('stock').value;
    this.book.price = this.form.get('price').value;
    this.book.imageName = this.form.get('file').value;
  }
  prepareSave(): any {
    const formModel = this.form.value;
    let fd = new FormData();

    if (this.book.id !== undefined && this.book.id !== 0) {
      fd.append("id", this.book.id.toString());
    }

    fd.append("name", formModel.name);
    fd.append("kind", formModel.kind);
    fd.append("author", formModel.author);
    fd.append("stock", formModel.stock);
    fd.append("price", formModel.price);
    fd.append("file", formModel.file);
    // fd.forEach(function (value, key, parent) {
    //   console.log(key + ':' + value);
    // });

    return fd;
  }
  search(val: any) {
    let queryString = this.selectedName + "_" + this.selectedKind;
    // console.log("queryString:", queryString);
    this.service.search(queryString).subscribe(
      response => {
        this.listSearch = response.data;
        // console.log("listSearch: ", this.listSearch);
        if (response.errorCode === 404) {
          this.message = "Không tìm thấy!";
          $('#modal-inform').modal('show');
          $('#modal-search').modal('hide');
          return;
        }
        $('#modal-search').modal('show');
      }
    );
  }
  onFileSelect(files: FileList) {
    if (files && files[0].size > 0) {
      this.form.get('file').patchValue(files[0]);
      this.fileChosen = files[0].name;
    }
    if (document.getElementById('img-1') != null) {
      this.imageTemplate = document.getElementById('img-1');
      document.getElementById('img-1').hidden;
    }
    if (document.getElementById('img-2') != null) {
      document.getElementById('img-2').parentElement.hidden;
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
            '<img id="img-2" class="w-100 rounded img-fluid img-thumbnail" ',
            'src="', e.target.result, '" ',
            'title="', escape(theFile.name), '"/>'
          ].join('');
          document.getElementById('imageFrame').innerHTML = "";
          document.getElementById('imageFrame').insertBefore(span, null);
        };
      })(f);
      reader.readAsDataURL(f);
    }
  }
  showModalEdit(id: number) {
    this.fileChosen = "Choose file";
    if (event) {
      event.preventDefault();
    }
    this.service.get(id).subscribe(response => { this.book = response.data; });
  }
  showModalRemove(id: number) {
    this.book.id = id;
    this.message = "Bạn có thật sự muốn xóa khách hàng này?";
  }
  showModalRestore(id: number) {
    this.book.id = id;
    this.message = "Bạn có chắc chắn muốn khôi phục khách hàng này?";
  }
  submitFormEditModal() {
    this.setUser();
    this.service.update(this.prepareSave(), this.book.id).subscribe(
      response => {
        console.log(this.book);
        switch (response.errorCode) {
          case 200: this.message = "Lưu thành công!"; break;
          case 404: this.message = "Không tìm thấy!"; break;
          case 9: this.message = "Lượng tồn kho phải ít hơn " + response.message + "!"; break;
          case 10: this.message = "Số lượng sách nhập phải nhập ít nhất là " + response.message + "!"; break;
          case 11: this.message = "Tên sách không được để trống!"; break;
          case 12: this.message = "Giá sách không được để trống!"; break;
          case 13: this.message = "Thể loại sách không được để trống!"; break;
          case 14: this.message = "Tên tác giả không được để trống!"; break;
          case 15: this.message = "Số lượng sách trong kho không được để trống!"; break;
          case 16: this.message = "Đã tồn tại quyển sách khác với tên này!"; break;
        }
        this.loadData();
      }
    );
  }
  closeEditModal(id: number) {
    if (document.getElementById('img-2') != null) {
      document.getElementById('img-2').parentElement.remove();
    }
    if (this.imageTemplate != null) {
      document.getElementById('imageFrame').appendChild(this.imageTemplate);
    }
    var val;
    this.loadData();
    this.loadDataRemoved();
    this.search(val);
  }
  restoreBook(id: number) {
    this.service.restoreBook(id).subscribe(response => {
      this.loadData();
      this.loadDataRemoved();
      if (response.errorCode === 200) {
        this.message = "Khôi phục thành công!";
      } else {
        this.message = "Khôi phục thất bại!";
      }
      $('#modal-inform').modal('show');
    });
  }
  removeBook(id: number) {
    this.service.remove(id).subscribe(response => {
      $('#modal-remove').modal('hide');
      this.loadData();
      this.loadDataRemoved();
    });
  }
}