import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, of, switchMap,tap } from 'rxjs';
import * as ExpenseActions from './expense.actions';
import { ExpenseApiService } from '@features/expense/services/expense-api.service';
import { NotificationService } from '@core/services/notification.service';
import { ExpenseRefreshService } from '@core/services/expense-refresh.service';
@Injectable()
export class ExpenseEffects {
  private actions$ = inject(Actions);
  private api = inject(ExpenseApiService);
  private toast = inject(NotificationService);
  private refresh = inject(ExpenseRefreshService); 
    
loadExpenses$ = createEffect(() =>
  this.actions$.pipe(
    ofType(ExpenseActions.getExpenses),
    switchMap((params) =>
      this.api.getAll(params).pipe(
        map((response) =>
          ExpenseActions.getExpensesSuccess({
            expenses: response.data.items,
            totalCount: response.data.totalCount,
          })
        ),
        catchError((error) =>
          of(
            ExpenseActions.getExpensesFailure({
              error: error?.message || 'Failed to load expenses',
            })
          )
        )
      )
    )
  )
);




createExpense$ = createEffect(() =>
  this.actions$.pipe(
    ofType(ExpenseActions.createExpense),
    switchMap(({ payload }) =>
      this.api.create(payload).pipe(
        map((expense) => ExpenseActions.createExpenseSuccess({ expense })),
        catchError((err) =>
          of(
            ExpenseActions.createExpenseFailure({
              error: err?.error?.message || 'Failed to create expense',
            })
          )
        )
      )
    )
  )
);

// Optional toast
createExpenseSuccessToast$ = createEffect(
  () =>
    this.actions$.pipe(
      ofType(ExpenseActions.createExpenseSuccess),
      tap(() => this.toast.success('Expense added!')),
      tap(() => this.refresh.trigger())
    ),
  { dispatch: false }
);


deleteExpense$ = createEffect(() =>
  this.actions$.pipe(
    ofType(ExpenseActions.deleteExpense),
    switchMap(({ id }) =>
      this.api.delete(id).pipe(
        map(() => ExpenseActions.deleteExpenseSuccess({ id })),
        catchError((err) =>
          of(
            ExpenseActions.deleteExpenseFailure({
              error: err?.error?.message || 'Failed to delete expense',
            })
          )
        )
      )
    )
  )
);

// Optional toast
deleteExpenseSuccessToast$ = createEffect(
  () =>
    this.actions$.pipe(
      ofType(ExpenseActions.deleteExpenseSuccess),
      tap(() => this.toast.success('Expense deleted!')),
      tap(() => this.refresh.trigger())
    ),
  { dispatch: false }
);


updateExpense$ = createEffect(() =>
  this.actions$.pipe(
    ofType(ExpenseActions.updateExpense),
    switchMap(({ id, payload }) =>
      this.api.update(id, payload).pipe(
        map((expense) => ExpenseActions.updateExpenseSuccess({ expense })),
        catchError((err) =>
          of(
            ExpenseActions.updateExpenseFailure({
              error: err?.error?.message || 'Failed to update expense',
            })
          )
        )
      )
    )
  )
);

// Optional toast
updateExpenseSuccessToast$ = createEffect(
  () =>
    this.actions$.pipe(
      ofType(ExpenseActions.updateExpenseSuccess),
      tap(() => this.toast.success('Expense updated!')),
       tap(() => this.refresh.trigger())
    ),
  { dispatch: false }
);

}
