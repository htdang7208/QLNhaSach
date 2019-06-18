import { Component, OnInit } from '@angular/core';
import { DeptReport, ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-dept-report',
  templateUrl: './dept-report.component.html',
  styleUrls: ['./dept-report.component.css']
})
export class DeptReportComponent implements OnInit {

  list: DeptReport = {} as DeptReport;
  constructor(private service: ReportService) { }

  ngOnInit() {
    this.loadData();
  }
  loadData() {
    this.service.getDeptReport().subscribe(response=>{
      this.list = response.data;
    })
  }

}
