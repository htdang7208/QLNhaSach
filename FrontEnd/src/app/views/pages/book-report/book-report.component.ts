import { Component, OnInit } from '@angular/core';
import { BookReport, ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-book-report',
  templateUrl: './book-report.component.html',
  styleUrls: ['./book-report.component.css']
})
export class BookReportComponent implements OnInit {
  list: BookReport = {} as BookReport;
  constructor(private service: ReportService) { }

  ngOnInit() {
    this.loadData();
  }
  loadData() {
    this.service.getBookReport().subscribe(response=>{
      this.list = response.data;
    })
  }

}
