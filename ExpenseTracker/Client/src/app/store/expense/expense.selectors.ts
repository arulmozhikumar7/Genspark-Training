import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ExpenseState } from './expense.state';

export const selectExpenseState = createFeatureSelector<ExpenseState>('expense');

export const selectAllExpenses = createSelector(
  selectExpenseState,
  (state) => state.expenses
);

export const selectExpenseLoading = createSelector(
  selectExpenseState,
  (state) => state.loading
);

export const selectExpenseError = createSelector(
  selectExpenseState,
  (state) => state.error
);

export const selectExpenseTotalCount = createSelector(
  selectExpenseState,
  (state) => state.totalCount
);

export const selectHasNextPage = (currentPage: number, pageSize: number) =>
  createSelector(selectExpenseTotalCount, (totalCount) => {
    return currentPage * pageSize < totalCount;
  });