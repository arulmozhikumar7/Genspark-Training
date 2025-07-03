import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as BudgetActions from './budget.actions';
import { BudgetApiService } from '@features/budget/services/budget-api.service';
import { catchError, map, of, switchMap, tap } from 'rxjs';
import { NotificationService } from '@core/services/notification.service';
import { BudgetRefreshService } from '@core/services/budget-refresh.service';

@Injectable()
export class BudgetEffects {
  private actions$ = inject(Actions);
  private api = inject(BudgetApiService);
  private toast = inject(NotificationService);
  private refresh = inject(BudgetRefreshService);

loadBudgets$ = createEffect(() =>
  this.actions$.pipe(
    ofType(BudgetActions.getBudgets),
    switchMap((action) =>
      this.api.getAll({ ...action }).pipe(  
        map((res) =>
          BudgetActions.getBudgetsSuccess({
            budgets: res.data.items,
            totalCount: res.data.totalCount,
          })
        ),
        catchError((error) =>
          of(BudgetActions.getBudgetsFailure({ error: error?.message || 'Failed to load budgets' }))
        )
      )
    )
  )
);



  createBudget$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BudgetActions.createBudget),
      switchMap(({ payload }) =>
        this.api.create(payload).pipe(
          map((budget) => BudgetActions.createBudgetSuccess({ budget })),
          catchError((err) =>
            of(BudgetActions.createBudgetFailure({ error: err?.message || 'Failed to create budget' }))
          )
        )
      )
    )
  );

  updateBudget$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BudgetActions.updateBudget),
      switchMap(({ id, payload }) =>
        this.api.update(id, payload).pipe(
          map((budget) => BudgetActions.updateBudgetSuccess({ budget })),
          catchError((err) =>
            of(BudgetActions.updateBudgetFailure({ error: err?.message || 'Failed to update budget' }))
          )
        )
      )
    )
  );

  deleteBudget$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BudgetActions.deleteBudget),
      switchMap(({ id }) =>
        this.api.delete(id).pipe(
          map(() => BudgetActions.deleteBudgetSuccess({ id })),
          catchError((err) =>
            of(BudgetActions.deleteBudgetFailure({ error: err?.message || 'Failed to delete budget' }))
          )
        )
      )
    )
  );

  // Toasts
  toastSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(
          BudgetActions.createBudgetSuccess,
          BudgetActions.updateBudgetSuccess,
          BudgetActions.deleteBudgetSuccess
        ),
        tap((action) => {
          if ('budget' in action) this.toast.success('Budget saved!');
          if ('id' in action) this.toast.success('Budget deleted!');
          this.refresh.trigger();
        })
      ),
    { dispatch: false }
  );
}
