import { AfterViewInit, Component, OnInit, inject } from '@angular/core';
import {
  CommonModule,
  AsyncPipe,
  NgIf,
  NgFor,
  CurrencyPipe,
  DatePipe,
} from '@angular/common';
import { Store } from '@ngrx/store';
import {
  getBudgets,
  deleteBudget,
  updateBudget,
} from '@store/budget/budget.actions';
import {
  selectAllBudgets,
  selectBudgetLoading,
  selectBudgetError,
  selectBudgetTotalCount,
  selectHasNextPage,
} from '@store/budget/budget.selectors';
import { timer, combineLatest } from 'rxjs';
import { Budget } from '@shared/models/budget.model';
import { Observable, Subject, take } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { FormsModule } from '@angular/forms';
import { AddBudgetFormComponent } from '@features/budget/components/add-budget-form/add-budget-form.component';
import { selectAllCategories } from '@store/category/category.selectors';
import { getCategories } from '@store/category/category.actions';
import { Category } from '@shared/models/category.model';
import { AddBudgetModalComponent } from '@shared/components/add-budget-modal/add-budget-modal.component';
import { driver } from 'driver.js';
import 'driver.js/dist/driver.css';
import { RouterModule } from '@angular/router';
import { BudgetRefreshService } from '@core/services/budget-refresh.service';
import { TourService } from '@core/services/tour.service';

@Component({
  standalone: true,
  selector: 'app-budget-list-page',
  imports: [
    CommonModule,
    AsyncPipe,
    NgIf,
    NgFor,
    CurrencyPipe,
    DatePipe,
    FormsModule,
    AddBudgetModalComponent,
    RouterModule
  ],
  templateUrl: './budget-list.page.html',
})
export class BudgetListPage implements OnInit , AfterViewInit{
  private store = inject(Store);
  private budgetRefresh = inject(BudgetRefreshService);
  private tourService = inject(TourService);
  private hasTriggeredFullTour = false;
  budgets$ = this.store.select(selectAllBudgets);
  loading$ = this.store.select(selectBudgetLoading);
  error$ = this.store.select(selectBudgetError);
  totalCount$ = this.store.select(selectBudgetTotalCount);
  categories$ = this.store.select(selectAllCategories);
 
  editingBudget: Budget | null = null;
 showAddBudget = false;
 tourKey = 'budgetTourCompleted'
 ngAfterViewInit(): void {
  
combineLatest([this.budgets$, timer(300)])
  .pipe(take(1))
  .subscribe(([budgets]) => {
    if (!budgets || budgets.length === 0 && !this.tourService.isCompleted(this.tourKey)) {
      this.startAddBudget();
    } else {
       if ( budgets.length > 0 && !this.tourService.isCompleted(this.tourKey))
            this.startDriverTour();
    }
  });
  

   this.budgets$.subscribe((budgets) => {
    if (
      budgets.length >= 1 && 
      !this.tourService.isCompleted(this.tourKey) &&
      !this.hasTriggeredFullTour
    ) {
      this.hasTriggeredFullTour = true;
      setTimeout(() => this.startDriverTour(), 300); 
    }
  });

}
 startAddBudget(){
   const driverObj = driver({
    showProgress: false,
    popoverClass: 'driverjs-theme',
    steps: [
      {
        element: '#budget-add',
        popover: {
          title: 'Add Budget',
          description: 'Add a new budget to get started.',
          side: 'top',
          align: 'start',
        },
      }]
    ,
  });

  driverObj.drive();
 } 

 startDriverTour() {
  const driverObj = driver({
    showProgress: true,
    popoverClass: 'driverjs-theme',
    steps: [
      {
        element: '#budget-table',
        popover: {
          title: 'Budget Table',
          description: 'Here’s your list of budgets with details.',
          side: 'top',
          align: 'start',
        },
      },
      {
        element: '#budget-detail',
        popover: {
          title: 'Budget Detail',
          description: 'Click on the budget name to get more insights',
          side: 'top',
          align: 'start',
        },
      },
      {
        element: '#search-input',
        popover: {
          title: 'Search',
          description: 'Type here to search budgets by name.',
          side: 'bottom',
          align: 'start',
        },
      },
      {
        element: '#category-select',
        popover: {
          title: 'Category',
          description: 'Select a category to filter.',
          side: 'bottom',
          align: 'start',
        },
      },
      {
        element: '#month-select',
        popover: {
          title: 'Month',
          description: 'Pick a month to view budgets in that period.',
          side: 'bottom',
          align: 'start',
        },
      },
      {
        element: '#year-select',
        popover: {
          title: 'Year',
          description: 'Choose a year to filter budgets.',
          side: 'bottom',
          align: 'start',
        },
      },
      {
        element: '#reset-button',
        popover: {
          title: 'Reset Filters',
          description: 'Click here to clear all filters and restore the full budget list.',
          side: 'top',
          align: 'start',
        },
      },
      {
        element: '#budget-edit',
        popover: {
          title: 'Edit Budget',
          description: 'Use the pencil icon to edit a budget entry.',
          side: 'top',
          align: 'start',
        },
      },
      {
        element: '#budget-delete',
        popover: {
          title: 'Delete Budget',
          description: 'Use the bin icon to delete a budget entry.',
          side: 'top',
          align: 'start',
        },
      },
    ],
    onDestroyed: ()=>{
       this.tourService.markCompleted(this.tourKey);
    }
  });

  driverObj.drive();
}
  openBudgetModal() {
    this.showAddBudget = true;
  }

  closeBudgetModal() {
    this.showAddBudget = false;
  }
  
  currentPage = 1;
  pageSize = 10;
  pages: number[] = [];
  hasNextPage$ = this.store.select(selectHasNextPage(this.currentPage, this.pageSize));

  // Filters
  filters = {
    categoryId: undefined as string | undefined,
    search: '',
    month: undefined as number | undefined,
    year: undefined as number | undefined,
  };

  private searchSubject = new Subject<string>();

  months = [
    { name: 'January', value: 1 },
    { name: 'February', value: 2 },
    { name: 'March', value: 3 },
    { name: 'April', value: 4 },
    { name: 'May', value: 5 },
    { name: 'June', value: 6 },
    { name: 'July', value: 7 },
    { name: 'August', value: 8 },
    { name: 'September', value: 9 },
    { name: 'October', value: 10 },
    { name: 'November', value: 11 },
    { name: 'December', value: 12 },
  ];

  years = Array.from({ length: 26 }, (_, i) => 2025 + i);

  ngOnInit(): void {
    this.store.dispatch(getCategories());
    this.loadBudgets();

    this.totalCount$.subscribe((count) => this.updatePaginationPages(count));

    this.searchSubject.pipe(
      debounceTime(400),
      distinctUntilChanged()
    ).subscribe((term) => {
      this.filters.search = term;
      this.currentPage = 1;
      this.loadBudgets();
    });

     this.budgetRefresh.refresh$.subscribe(()=>{
      this.loadBudgets();
    });

    
  }

  loadBudgets() {
    
    this.store.dispatch(
      getBudgets({
        page: this.currentPage,
        pageSize: this.pageSize,
        categoryId: this.filters.categoryId,
        search: this.filters.search,
        month: this.filters.month,
        year: this.filters.year,
      })
    );
  }

  onSearchChange(value: string) {
    this.searchSubject.next(value);
  }

  onCategoryChange(value: string) {
    this.filters.categoryId = value || undefined;
    this.currentPage = 1;
    this.loadBudgets();
  }
  onMonthChange(month: number | undefined) {
  this.filters.month = month;
  this.currentPage = 1;
  this.loadBudgets();
}

  onYearChange(year: number | undefined) {
    this.filters.year = year;
    this.currentPage = 1;
    this.loadBudgets();
  }

  onDelete(id: string) {
    if (confirm('Are you sure you want to delete this budget?')) {
      this.store.dispatch(deleteBudget({ id }));
    }
  }

  onEdit(budget: Budget) {
    this.editingBudget = {
      ...budget,
      startDate: budget.startDate.split('T')[0],
      endDate: budget.endDate.split('T')[0],
    };
   
  }

  onCancelEdit() {
    this.editingBudget = null;
  }

  onUpdateBudget(formValue: Partial<Budget>) {
    if (!this.editingBudget) return;

    const updated: Partial<Budget> = {
      ...formValue,
      startDate: new Date(formValue.startDate! + 'T00:00:00').toISOString(),
      endDate: new Date(formValue.endDate! + 'T23:59:59').toISOString(),
    };

    this.store.dispatch(updateBudget({ id: this.editingBudget.id, payload: updated }));
    this.editingBudget = null;
  }

  applyFilters() {
    this.currentPage = 1;
    this.loadBudgets();
  }

  resetFilters() {
    this.filters = {
      categoryId: undefined,
      search: '',
      month: undefined,
      year: undefined,
    };
    this.applyFilters();
  }

  onNextPage() {
    this.hasNextPage$.pipe(take(1)).subscribe((hasNext) => {
      if (hasNext) {
        this.currentPage++;
        this.loadBudgets();
      }
    });
  }

  onPrevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadBudgets();
    }
  }

  onPageClick(page: number) {
    if (page === this.currentPage) return;
    this.currentPage = page;
    this.loadBudgets();
  }

  updatePaginationPages(totalCount: number) {
    const totalPages = Math.ceil(totalCount / this.pageSize);
    this.pages = Array.from({ length: totalPages }, (_, i) => i + 1);
  }

  getFriendlyErrorMessage(error: string): string {
  if (error?.includes('500') || error?.includes('Internal Server Error')) {
    return 'Server down at the moment. Please try again later';
  }
  return error;
}
}
