import { TestBed } from '@angular/core/testing';

import { DeptReportService } from './dept-report.service';

describe('DeptReportService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DeptReportService = TestBed.get(DeptReportService);
    expect(service).toBeTruthy();
  });
});
