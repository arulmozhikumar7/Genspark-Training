import { createReducer, on } from '@ngrx/store';
import * as ExpenseActions from './expense.actions';
import { ExpenseState, initialExpenseState } from './expense.state';

export const expenseReducer = createReducer(
  initialExpenseState,

  on(ExpenseActions.getExpenses, (state): ExpenseState => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(ExpenseActions.getExpensesSuccess, (state, { expenses,totalCount}): ExpenseState => ({
    ...state,
    loading: false,
    expenses,
    totalCount,
    error: null
  })),

  on(ExpenseActions.getExpensesFailure, (state, { error }): ExpenseState => ({
    ...state,
    loading: false,
    error,
  })),

  on(ExpenseActions.deleteExpenseSuccess, (state, { id }) => ({
  ...state,
  expenses: state.expenses.filter((e) => e.id !== id)
})),

on(ExpenseActions.updateExpenseSuccess, (state, { expense }) => ({
  ...state,
  expenses: state.expenses.map((e) => (e.id === expense.id ? expense : e))
}))


);
