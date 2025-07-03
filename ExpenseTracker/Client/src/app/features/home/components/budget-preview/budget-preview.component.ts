import { Component, OnInit, inject } from '@angular/core';
import {
  CommonModule,
  AsyncPipe,
  NgIf,
  NgFor,
  CurrencyPipe,
  DatePipe,
} from '@angular/common';
import { Store } from '@ngrx/store';
import { getBudgets } from '@store/budget/budget.actions';
import { selectAllBudgets, selectBudgetLoading } from '@store/budget/budget.selectors';
import { Budget } from '@shared/models/budget.model';
import { RouterModule } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-budget-preview',
  templateUrl: './budget-preview.component.html',
  imports: [CommonModule, AsyncPipe, NgIf, NgFor, CurrencyPipe, DatePipe,RouterModule],
})
export class BudgetPreviewComponent implements OnInit {
  private store = inject(Store);

  budgets$ = this.store.select(selectAllBudgets);
  loading$ = this.store.select(selectBudgetLoading);

  ngOnInit(): void {
    this.store.dispatch(getBudgets({ page: 1, pageSize: 10 }));
  }
}
