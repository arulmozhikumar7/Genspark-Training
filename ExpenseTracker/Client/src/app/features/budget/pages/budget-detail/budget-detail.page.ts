import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule, CurrencyPipe, DatePipe, NgIf } from '@angular/common';
import { ExpenseApiService } from '@features/expense/services/expense-api.service';
import { BudgetApiService } from '@features/budget/services/budget-api.service';
import { ExpensePieChartComponent } from '@features/dashboard/components/expense-pie-chart/expense-pie-chart.component';

@Component({
  standalone: true,
  selector: 'app-budget-detail-page',
  imports: [CommonModule, NgIf, CurrencyPipe, DatePipe, ExpensePieChartComponent],
  templateUrl: './budget-detail.page.html',
})
export class BudgetDetailPageComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private budgetApi = inject(BudgetApiService);
  private expenseApi = inject(ExpenseApiService);

  budgetId!: string;
  budget: any = null;
  expenses: any[] = [];
  loading = true;

  spentAmount = 0;

  ngOnInit(): void {
    this.budgetId = this.route.snapshot.params['id'];
    this.loadData();
  }

  loadData() {
  this.loading = true;

  this.budgetApi.getById(this.budgetId).subscribe((res) => {
    this.budget = res.data; 

    const { categoryId, startDate, endDate } = this.budget;

    this.expenseApi.getAll({
      categoryId,
      fromDate: startDate,
      toDate: endDate,
      page: 1,
      pageSize: 1000, 
    }).subscribe((res) => {
      this.expenses = res.data.items;
      this.spentAmount = this.expenses.reduce((sum, e) => sum + e.amount, 0);
      this.loading = false;
    });
  });
}


  getRemainingAmount(): number {
  return Math.max(this.budget.limitAmount - this.spentAmount, 0);
}
}
