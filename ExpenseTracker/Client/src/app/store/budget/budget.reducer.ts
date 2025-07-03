import { createReducer, on } from '@ngrx/store';
import * as BudgetActions from './budget.actions';
import { BudgetState } from './budget.state';

export const initialState: BudgetState = {
  budgets: [],
  loading: false,
  error: null,
  totalCount: 0
};

export const budgetReducer = createReducer(
  initialState,

  on(BudgetActions.getBudgets, (state) => ({ ...state, loading: true })),
  on(BudgetActions.getBudgetsSuccess, (state, { budgets, totalCount }) => ({
  ...state,
  budgets,
  totalCount,
  loading: false,
  error: null,
})),
  on(BudgetActions.getBudgetsFailure, (state, { error }) => ({ ...state, loading: false, error })),

  on(BudgetActions.createBudgetSuccess, (state, { budget }) => ({
    ...state,
    budgets: [...state.budgets, budget],
  })),

  on(BudgetActions.updateBudgetSuccess, (state, { budget }) => ({
    ...state,
    budgets: state.budgets.map((b) => (b.id === budget.id ? budget : b)),
  })),

  on(BudgetActions.deleteBudgetSuccess, (state, { id }) => ({
    ...state,
    budgets: state.budgets.filter((b) => b.id !== id),
  }))
);
