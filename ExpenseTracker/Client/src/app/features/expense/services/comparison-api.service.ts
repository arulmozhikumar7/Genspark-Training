import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from '@core/services/http.service'; 
import {
  MonthlyExpenseComparisonSummaryDto,
} from '@features/dashboard/models/monthly-expense-comparison.model';

@Injectable({ providedIn: 'root' })
export class ComparisonApiService {
  constructor(private http: HttpService) {}

  compareTwoMonths(
    year1: number,
    month1: number,
    year2: number,
    month2: number
  ): Observable<{data: MonthlyExpenseComparisonSummaryDto}> {
    return this.http.get<{data: MonthlyExpenseComparisonSummaryDto}>('/Expense/compare-months', {
      year1,
      month1,
      year2,
      month2,
    });
  }
}
