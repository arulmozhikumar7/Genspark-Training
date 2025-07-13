import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ExpenseReportPage } from './expense-report.page';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { ExpenseReportApiService } from '../services/expense-report-api.service';
import { FeatureFlagService } from '@core/services/feature-flag.service';

// Helper to normalize date strings to 'YYYY-MM-DD'
function dateToYYYYMMDD(dateStr: string | undefined): string | undefined {
  if (!dateStr) return undefined;
  return new Date(dateStr).toISOString().split('T')[0];
}

describe('ExpenseReportPage', () => {
  let component: ExpenseReportPage;
  let fixture: ComponentFixture<ExpenseReportPage>;
  let storeSpy: jasmine.SpyObj<Store>;
  let reportApiSpy: jasmine.SpyObj<ExpenseReportApiService>;
  let featureFlagSpy: jasmine.SpyObj<FeatureFlagService>;

  beforeEach(async () => {
    storeSpy = jasmine.createSpyObj('Store', ['dispatch', 'select']);
    storeSpy.select.and.returnValue(of([])); // categories$

    reportApiSpy = jasmine.createSpyObj('ExpenseReportApiService', ['downloadCSV']);
    featureFlagSpy = jasmine.createSpyObj('FeatureFlagService', ['getFlags']);
    featureFlagSpy.getFlags.and.returnValue(of({ enableCsvExport: true }));

    await TestBed.configureTestingModule({
      imports: [ExpenseReportPage],
      providers: [
        { provide: Store, useValue: storeSpy },
        { provide: ExpenseReportApiService, useValue: reportApiSpy },
        { provide: FeatureFlagService, useValue: featureFlagSpy },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ExpenseReportPage);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('ngOnInit should dispatch getCategories and set exportEnabled', () => {
    component.ngOnInit();
    expect(storeSpy.dispatch).toHaveBeenCalledWith(jasmine.any(Object));
    expect(component.exportEnabled).toBeTrue();
  });

 
  it('exportReport should prompt for filename if needed and use default fallback', () => {
    spyOn(window, 'prompt').and.returnValue('my-report');

    component.filters = {
      fromDate: '2023-02-05',
      toDate: '2023-02-15',
    };

    component.exportReport();

    const filename = reportApiSpy.downloadCSV.calls.mostRecent().args[1];
    expect(filename).toBe('my-report.csv');
  });

  it('exportReport should use default filename if prompt returns empty', () => {
    spyOn(window, 'prompt').and.returnValue('   ');

    component.filters = {
      fromDate: '2023-02-05',
      toDate: '2023-02-15',
    };

    component.exportReport();

    const filename = reportApiSpy.downloadCSV.calls.mostRecent().args[1];
    expect(filename).toMatch(/^expense-report-\d{4}-\d{2}-\d{2}\.csv$/);
  });

  it('resetFilters should clear filters', () => {
    component.filters = { categoryId: 'cat1' };
    component.resetFilters();
    expect(component.filters).toEqual({});
  });

 
  
});
