import { Component, AfterViewInit ,inject} from '@angular/core';
import { CommonModule } from '@angular/common';
import { BudgetPreviewComponent } from './components/budget-preview/budget-preview.component';
import { ExpensePreviewComponent } from './components/expense-preview/expense-preview.component';
import { BudgetBarChartComponent } from './components/budget-bar-chart/budget-bar-chart.component';
import { AddBudgetModalComponent } from '@shared/components/add-budget-modal/add-budget-modal.component';
import { AddExpenseModalComponent } from '@shared/components/add-expense-modal/add-expense-modal.component';
import { TourService } from '@core/services/tour.service';
import { take } from 'rxjs';
import { driver } from 'driver.js';
import 'driver.js/dist/driver.css';

@Component({
  standalone: true,
  selector: 'app-home-page',
  imports: [
    CommonModule,
    BudgetPreviewComponent,
    ExpensePreviewComponent,
    BudgetBarChartComponent,
    AddBudgetModalComponent,
    AddExpenseModalComponent
  ],
  templateUrl: './home.page.html',
})
export class HomePage implements AfterViewInit {
  showAddBudget = false;
  showAddExpense = false;
  private tourService = inject(TourService);
  openBudgetModal() { 
    this.showAddBudget = true;
  }

  closeBudgetModal() {
    this.showAddBudget = false;
  }

  openExpenseModal() {
    this.showAddExpense = true;
  }

  closeExpenseModal() {
    this.showAddExpense = false;
  }

   ngAfterViewInit(): void {
    const tourKey = 'homeTourCompleted';

    this.tourService.progressReady$
      .pipe(take(1))
      .subscribe(() => {
        if (this.tourService.isCompleted(tourKey)) return;

        const driverObj = driver({
          showProgress: true,
          popoverClass: 'driverjs-theme',
          steps: [
            {
              element: '#budget-preview',
              popover: {
                title: 'Budget Preview',
                description: 'This section shows a detailed breakdown of your budgets.',
                side: 'top',
                align: 'start'
              }
            },
            {
              element: '#budget-bar-chart',
              popover: {
                title: 'Budget Chart',
                description: 'Visualize your budgets using this chart.',
                side: 'top',
                align: 'start'
              }
            },
            {
              element: '#expense-preview',
              popover: {
                title: 'Expense Preview',
                description: 'This section lists all your recorded expenses.',
                side: 'top',
                align: 'start'
              }
            },
            {
              popover: {
                title: 'All Set!',
                description: 'That’s it — you’re ready to manage your finances 🎉'
              }
            }
          ],
          onDestroyed: () => {
            this.tourService.markCompleted(tourKey);
          }
        });

        driverObj.drive();
      });
  }
}
