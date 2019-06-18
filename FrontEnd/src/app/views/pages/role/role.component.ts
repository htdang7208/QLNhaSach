import { Component, OnInit } from '@angular/core';
import { ReportService, Roles } from 'src/app/services/report.service';
declare var $: any;
@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.css']
})
export class RoleComponent implements OnInit {

  roles: Roles = {} as Roles;
  message: string;
  constructor(private service: ReportService) { }

  ngOnInit() {
    this.loadData();
  }
  loadData() {
    this.service.getRoles().subscribe(response => {
      this.roles = response;
    });
  }
  change(val: any) {
    this.service.changeRoles(this.roles).subscribe(response => {
      this.loadData();
      this.message = "Thay đổi quy định thành công!";
      $('#modal-inform').modal('show');
    });
  }
}
