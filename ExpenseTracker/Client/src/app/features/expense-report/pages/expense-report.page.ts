import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Category } from '@shared/models/category.model';
import { Store } from '@ngrx/store';
import { getCategories } from '@store/category/category.actions';
import { selectAllCategories } from '@store/category/category.selectors';
import { Observable } from 'rxjs';
import { ExpenseReportApiService } from '../services/expense-report-api.service';
import { FeatureFlagService, FeatureFlags } from '@core/services/feature-flag.service';


@Component({
  standalone: true,
  selector: 'app-expense-report-page',
  imports: [CommonModule, FormsModule],
  templateUrl: './expense-report.page.html',
})
export class ExpenseReportPage implements OnInit {
  private store = inject(Store);
  private reportApi = inject(ExpenseReportApiService);
  private flagService = inject(FeatureFlagService);

  exportEnabled = false;
  categories$: Observable<Category[]> = this.store.select(selectAllCategories);

  filters: {
    categoryId?: string;
    minAmount?: number;
    maxAmount?: number;
    fromDate?: string;
    toDate?: string;
  } = {};

  ngOnInit(): void {
  this.store.dispatch(getCategories());
     this.flagService.getFlags().subscribe(flags => {
      this.exportEnabled = flags.enableCsvExport;
    });
  }

  exportReport(overrideFilename?: string) {
  const { fromDate, toDate, ...rest } = this.filters;

  const utcFromDate = fromDate ? new Date(fromDate + 'T00:00:00').toISOString() : undefined;
  const utcToDate = toDate ? new Date(toDate + 'T23:59:59').toISOString() : undefined;

  const params = {
    ...rest,
    fromDate: utcFromDate,
    toDate: utcToDate,
  };

  let filename = overrideFilename;

  if (!filename) {
    const from = new Date(this.filters.fromDate ?? '');
    const to = new Date(this.filters.toDate ?? '');

    if (this.filters.fromDate && this.filters.toDate) {
      const sameMonth = from.getMonth() === to.getMonth();
      const sameYear = from.getFullYear() === to.getFullYear();

      if (
        from.getDate() === 1 &&
        sameMonth &&
        to.getDate() === new Date(from.getFullYear(), from.getMonth() + 1, 0).getDate()
      ) {
        filename = `${from.toLocaleString('default', { month: 'long' }).toLowerCase()}-${from.getFullYear()}`;
      } else if (sameMonth && sameYear && to.getDate() - from.getDate() <= 6) {
         const weekNumber = Math.floor((from.getDate() - 1) / 7) + 1;
        filename = `week-${weekNumber}-${from.toLocaleString('default', { month: 'long' }).toLowerCase()}-${from.getFullYear()}`;
      } else {
        const userFilename = prompt("Enter a filename for the report (without extension):");
        if (userFilename?.trim()) {
          filename = userFilename.trim();
        } else {
          filename = `expense-report-${new Date().toISOString().slice(0, 10)}`;
        }
      }
    }
  }

  this.reportApi.downloadCSV(params, `${filename}.csv`);
}


  resetFilters() {
    this.filters = {};
  }

  applyMonthlyReport() {
  const now = new Date();
  const firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
  const lastDay = new Date(now.getFullYear(), now.getMonth() + 1, 0);

  this.filters.fromDate = firstDay.toISOString().split('T')[0];
  this.filters.toDate = lastDay.toISOString().split('T')[0];

  const filename = `${firstDay.toLocaleString('default', { month: 'long' }).toLowerCase()}-${firstDay.getFullYear()}`;
  this.exportReport(filename);
}

  applyWeekReport(week: number) {
  const now = new Date();
  const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);

  const start = new Date(startOfMonth);
  start.setDate((week - 1) * 7 + 1);

  const end = new Date(start);
  end.setDate(start.getDate() + 6);

  this.filters.fromDate = start.toISOString().split('T')[0];
  this.filters.toDate = end.toISOString().split('T')[0];

  const filename = `week-${week}-${start.toLocaleString('default', { month: 'long' }).toLowerCase()}-${start.getFullYear()}`;
  this.exportReport(filename);
}
}
