import { createFeatureSelector, createSelector } from '@ngrx/store';
import { BudgetState } from './budget.state';

export const selectBudgetState = createFeatureSelector<BudgetState>('budget');

export const selectAllBudgets = createSelector(
  selectBudgetState,
  (state) => state.budgets
);

export const selectBudgetLoading = createSelector(
  selectBudgetState,
  (state) => state.loading
);

export const selectBudgetError = createSelector(
  selectBudgetState,
  (state) => state.error
);

export const selectBudgetTotalCount = createSelector(
  selectBudgetState,
  (state) => state.totalCount
);

export const selectHasNextPage = (currentPage: number, pageSize: number) =>
  createSelector(selectBudgetTotalCount, (totalCount) => {
    const loadedCount = currentPage * pageSize;
    return loadedCount < totalCount;
  });