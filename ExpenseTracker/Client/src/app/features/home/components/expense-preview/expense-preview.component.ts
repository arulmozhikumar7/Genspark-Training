import { Component, OnInit, inject } from '@angular/core';
import { AsyncPipe, CurrencyPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { Store } from '@ngrx/store';
import { getExpenses } from '@store/expense/expense.actions';
import { selectAllExpenses, selectExpenseLoading } from '@store/expense/expense.selectors';
import { ExpenseApiService } from '@features/expense/services/expense-api.service';
import { Expense } from '@features/expense/models/expense.model';
import { Observable } from 'rxjs';
import { RouterModule } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-expense-preview',
  imports: [NgIf, NgFor, AsyncPipe, CurrencyPipe, DatePipe,RouterModule],
  templateUrl: './expense-preview.component.html',
})
export class ExpensePreviewComponent implements OnInit {
  private store = inject(Store);
  private api = inject(ExpenseApiService);

  expenses$!: Observable<Expense[]>;
  loading$ = this.store.select(selectExpenseLoading);
  totalExpenseAmount = 0;

  startDate = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
  endDate = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0);

  ngOnInit(): void {
    const start = this.startDate.toISOString();
    const end = new Date(this.endDate.setHours(23, 59, 59, 999)).toISOString();

    // Load top 5 current month expenses
    this.store.dispatch(
      getExpenses({
        page: 1,
        pageSize: 5,
        fromDate: start,
        toDate: end,
      })
    );
    this.expenses$ = this.store.select(selectAllExpenses);

    // Fetch total summary for current month
    this.api.getExpenseSummary({ fromDate: start, toDate: end }).subscribe((res: any) => {
      const all = res.data || [];
      this.totalExpenseAmount = all.reduce((sum: number, item: any) => sum + item.totalAmount, 0);
    });
  }
}
