import { Component, OnInit, inject } from '@angular/core';
import {
  AsyncPipe,
  NgIf,
  NgFor,
  DatePipe,
  CurrencyPipe,
  CommonModule,
} from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import {
  getExpenses,
  deleteExpense,
  updateExpense,
} from '@store/expense/expense.actions';
import {
  selectAllExpenses,
  selectExpenseLoading,
  selectExpenseError,
  selectExpenseTotalCount,
  selectHasNextPage,
} from '@store/expense/expense.selectors';
import { AddExpenseFormComponent } from '@features/expense/components/add-expense-form/add-expense-form.component';
import { Expense } from '@features/expense/models/expense.model';
import { Subject } from 'rxjs';
import { timer, combineLatest } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ReceiptApiService } from '@features/receipt/services/receipt-api.service';
import { getCategories } from '@store/category/category.actions';
import { selectAllCategories } from '@store/category/category.selectors';
import { Category } from '@shared/models/category.model';
import { Observable, take } from 'rxjs';
import { ExpenseChartComponent } from '@features/dashboard/components/expense-chart/expense-chart.component';
import { ExpensePieChartComponent } from '@features/dashboard/components/expense-pie-chart/expense-pie-chart.component';
import { ExpenseApiService } from '@features/expense/services/expense-api.service';
import { ExpenseRefreshService } from '@core/services/expense-refresh.service';
import { AddExpenseModalComponent } from '@shared/components/add-expense-modal/add-expense-modal.component';
import { NotificationService } from '@core/services/notification.service';
import { driver } from 'driver.js';
import 'driver.js/dist/driver.css';
import { TourService } from '@core/services/tour.service';
@Component({
  standalone: true,
  selector: 'app-expense-list-page',
  imports: [
    CommonModule,
    AsyncPipe,
    NgIf,
    NgFor,
    DatePipe,
    CurrencyPipe,
    FormsModule,
    AddExpenseModalComponent,
    ExpensePieChartComponent
  ],
  templateUrl: './expense-list.page.html',
})
export class ExpenseListPage implements OnInit {
  private store = inject(Store);
  private api = inject(ExpenseApiService);
  private expenseRefresh = inject(ExpenseRefreshService);
  private toast = inject(NotificationService);
  private tourService = inject(TourService);
  private hasTriggeredFullTour = false;
  loading$ = this.store.select(selectExpenseLoading);
  error$ = this.store.select(selectExpenseError);
  expenses$ = this.store.select(selectAllExpenses);
  categories$: Observable<Category[]> = this.store.select(selectAllCategories);
  totalCount$ = this.store.select(selectExpenseTotalCount);

  currentPage = 1;
  pageSize = 10;
  pages: number[] = [];

  hasNextPage$ = this.store.select(selectHasNextPage(this.currentPage, this.pageSize));

  editingExpense: any = null;
  showAddExpense: boolean = false;
  filters: {
    categoryId?: string;
    minAmount?: number;
    maxAmount?: number;
    fromDate?: string;
    toDate?: string;
    search?: string;
    sortBy?: string;
    sortDirection?: string;
  } = {};
searchTerm = '';
  openExpenseModal() {
    this.showAddExpense = true;
  }

  closeExpenseModal() {
    this.showAddExpense = false;
  }
summaryData: { categoryName: string; totalAmount: number }[] = [];
private fetchSummary() {
  const { fromDate, toDate, ...rest } = this.filters;

  const utcFromDate = fromDate ? new Date(fromDate + 'T00:00:00').toISOString() : undefined;
  const utcToDate = toDate ? new Date(toDate + 'T23:59:59').toISOString() : undefined;

  const params = {
    ...rest,
    fromDate: utcFromDate,
    toDate: utcToDate,
  };

  this.api.getExpenseSummary(params).subscribe((res: any) => {
    this.summaryData = res.data || [];
  });
}
private searchSubject = new Subject<string>();
  ngOnInit(): void {
    this.loadExpenses();
    this.store.dispatch(getCategories());

    this.totalCount$.subscribe((count) => {
      this.updatePaginationPages(count);
    });
     this.searchSubject.pipe(
    debounceTime(400),
    distinctUntilChanged()
  ).subscribe((term) => {
    this.filters.search = term;
    this.currentPage = 1;
    this.loadExpenses();
  });
  this.expenseRefresh.refresh$.subscribe(() => {
    this.loadExpenses();       
    this.fetchSummary();      
  });
  }

  onSearchChange(value: string) {
  this.searchSubject.next(value);
}
  loadExpenses() {
     const { fromDate, toDate, ...restFilters } = this.filters;

  const utcFromDate = fromDate
    ? new Date(fromDate + 'T00:00:00').toISOString()
    : undefined;

  const utcToDate = toDate
    ? new Date(toDate + 'T23:59:59').toISOString()
    : undefined;

  const payload = {
    page: this.currentPage,
    pageSize: this.pageSize,
    ...restFilters,
    fromDate: utcFromDate,
    toDate: utcToDate,
  };

  Object.keys(payload).forEach((key) => {
    const val = payload[key as keyof typeof payload];
    if (val === null || val === undefined || val === '') {
      delete payload[key as keyof typeof payload];
    }
  });

  console.log('Fetching expenses with:', payload);
  this.store.dispatch(getExpenses(payload));
  this.fetchSummary(); 
  }

  onNextPage() {
    this.hasNextPage$.pipe(take(1)).subscribe((hasNext) => {
      if (hasNext) {
        this.currentPage++;
        this.loadExpenses();
      }
    });
  }

  onPrevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadExpenses();
    }
  }

  onPageClick(page: number) {
    if (page === this.currentPage) return;
    this.currentPage = page;
    this.loadExpenses();
  }

  updatePaginationPages(totalCount: number) {
    const totalPages = Math.ceil(totalCount / this.pageSize);
    this.pages = Array.from({ length: totalPages }, (_, i) => i + 1);
  }

  applyFilters() {
  const { fromDate, toDate } = this.filters;

  const utcFromDate = fromDate
    ? new Date(fromDate + 'T00:00:00').toISOString()
    : undefined;

  const utcToDate = toDate
    ? new Date(toDate + 'T23:59:59').toISOString()
    : undefined;

  this.currentPage = 1; 

  this.store.dispatch(
    getExpenses({
      page: this.currentPage,
      pageSize: this.pageSize,
      ...this.filters,
      fromDate: utcFromDate,
      toDate: utcToDate,
    })
  );
  this.fetchSummary(); 
}


  resetFilters() {
    this.filters = {};
    this.applyFilters();
  }

  onDeleteExpense(id: string) {
    if (confirm('Are you sure you want to delete this expense?')) {
      this.store.dispatch(deleteExpense({ id }));
    }
  }

  onEditExpense(expense: Expense) {
    this.store.select(selectAllCategories).pipe(take(1)).subscribe((categories) => {
      const matchedCategory = categories.find((cat) => cat.name === expense.categoryName);
      const categoryId = matchedCategory?.id || '';

      this.editingExpense = {
        ...expense,
        categoryId,
        expenseDate: expense.expenseDate?.split('T')[0],
      };
    });
  }

  onCancelEdit() {
    this.editingExpense = null;
  }

  onUpdateExpense(formValue: Partial<Expense>) {
    if (!this.editingExpense) return;

    this.store.dispatch(
      updateExpense({
        id: this.editingExpense.id,
        payload: {
          ...formValue,
          expenseDate: new Date(formValue.expenseDate + 'T00:00:00').toISOString(),
        },
      })
    );

    this.editingExpense = null;

  }
  private receiptApi = inject(ReceiptApiService);
  selectedFileMap: Record<string, File> = {};
  onFileSelected(event: Event, expenseId: string, input: HTMLInputElement): void {
  if (input.files?.length) {
    this.selectedFileMap[expenseId] = input.files[0];
    this.uploadReceipt(expenseId, input);
  }
}

  uploadReceipt(expenseId: string, input: HTMLInputElement): void {
  const file = this.selectedFileMap[expenseId];
  if (!file) return alert('Please select a file first');


  this.receiptApi.uploadReceipt(expenseId, file).subscribe({
    next: () => {
      delete this.selectedFileMap[expenseId];
      input.value = '';
      this.toast.success("Receipt uploaded successfully")
    },
    error: () => {
    this.toast.error("Failed to upload.")
    }
  });
}
receiptViewMap: Record<string, boolean> = {};
receiptsMap: Record<string, { id: string; fileName: string }[]> = {};

toggleReceipts(expenseId: string): void {
  if (this.receiptViewMap[expenseId]) {
    this.receiptViewMap[expenseId] = false;
  } else {
    this.fetchReceipts(expenseId);
  }
}

fetchReceipts(expenseId: string): void {
  this.receiptApi.getReceiptsByExpenseId(expenseId).subscribe({
    next: (res) => {
      this.receiptsMap[expenseId] = res.data || [];
      this.receiptViewMap[expenseId] = true;
    },
    error: () => {
      alert('Failed to fetch receipts');
    }
  });
}

downloadReceipt(receiptId: string): void {
  this.receiptApi.downloadReceipt(receiptId).subscribe({
    next: (blob) => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `receipt-${receiptId}.pdf`;
      a.click();
      window.URL.revokeObjectURL(url);
    },
    error: () => {
      alert('Failed to download receipt');
    }
  });
}
deleteReceipt(receiptId: string, expenseId: string): void {
  
  this.receiptApi.deleteReceipt(receiptId).subscribe(() => {
  
    const updatedReceipts = this.receiptsMap[expenseId].filter(r => r.id !== receiptId);
    this.receiptsMap[expenseId] = updatedReceipts;
  });
}

  getFriendlyErrorMessage(error: string): string {
  if (error?.includes('500') || error?.includes('Internal Server Error')) {
    return 'Server down at the moment. Please try again later';
  }
  return error;
}

 startAddExpense(){
   const driverObj = driver({
    showProgress: false,
    popoverClass: 'driverjs-theme',
    steps: [
      {
        element: '#expense-add',
        popover: {
          title: 'Add Expense',
          description: 'Add a new expense to get started.',
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
        element: '#expense-list',
        popover: {
          title: 'Expense Table',
          description: 'Here’s your list of expenses with details.',
          side: 'top',
          align: 'start',
        },
      },
      {
        element: '#upload-receipt',
        popover: {
          title: 'Upload Receipt',
          description: 'Click on the upload icon to get upload receipts for the expense for future references.',
          side: 'top',
          align: 'start',
        },
      },
      {
        element: '#view-receipt',
        popover: {
          title: 'View Receipt',
          description: 'Once you have uploaded receipt , click here to view the receipts.',
          side: 'bottom',
          align: 'start',
        },
      },
      {
        element: '#expense-edit',
        popover: {
          title: 'Edit Expense',
          description: 'Select this icon to update expense',
          side: 'bottom',
          align: 'start',
        },
      },
      {
        element: '#expense-delete',
        popover: {
          title: 'Delete Expense',
          description: 'Select this to delete expense',
          side: 'bottom',
          align: 'start',
        },
      },
      {
        element: '#expense-search',
        popover: {
          title: 'Search Bar',
          description: 'Seach expense with names',
          side: 'bottom',
          align: 'start',
        },
      },
      {
        element: '#expense-filter',
        popover: {
          title: 'Filters',
          description: 'Click here to apply filter based on your needs.',
          side: 'top',
          align: 'start',
        },
      },
      {
        element: '#apply-btn',
        popover: {
          title: 'Apply Filter',
          description: 'Once you have selected the filters make sure to hit the apply button to reflect',
          side: 'top',
          align: 'start',
        },
      },
      {
        element: '#reset-btn',
        popover: {
          title: 'Reset Filter',
          description: 'Hit this button to reset the filters to defaults',
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

 tourKey = 'expenseTourCompleted'
ngAfterViewInit(): void {

combineLatest([this.expenses$, timer(300)])
  .pipe(take(1))
  .subscribe(([expenses]) => {
    if (!expenses || expenses.length === 0 && !this.tourService.isCompleted(this.tourKey)) {
      this.startAddExpense();
    } else {
      if(expenses.length>0&&!this.tourService.isCompleted(this.tourKey))
      this.startDriverTour();
    }
  });
  
   this.expenses$.subscribe((expenses) => {
    if (
      expenses.length >= 1 && 
      !this.tourService.isCompleted(this.tourKey) &&
      !this.hasTriggeredFullTour
    ) {
      this.hasTriggeredFullTour = true;
      setTimeout(() => this.startDriverTour(), 300); 
    }
  });
}
}