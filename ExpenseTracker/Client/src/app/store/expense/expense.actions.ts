import { createAction, props } from '@ngrx/store';
import { Expense } from '@features/expense/models/expense.model';

export const getExpenses = createAction(
  '[Expense] Get Expenses',
  props<{
    page: number;
    pageSize?: number;
    categoryId?: string;
    minAmount?: number;
    maxAmount?: number;
    fromDate?: string;
    toDate?: string;
    search?: string;
    sortBy?: string;
    sortDirection?: string;
  }>()
);


export const getExpensesSuccess = createAction(
  '[Expense] Get Expenses Success',
  props<{ expenses: Expense[]; totalCount: number }>()
);


export const getExpensesFailure = createAction(
  '[Expense] Get Expenses Failure',
  props<{ error: string }>()
);

export const createExpense = createAction(
  '[Expense] Create Expense',
  props<{ payload: Partial<Expense> }>()
);

export const createExpenseSuccess = createAction(
  '[Expense] Create Expense Success',
  props<{ expense: Expense }>()
);

export const createExpenseFailure = createAction(
  '[Expense] Create Expense Failure',
  props<{ error: string }>()
);

export const deleteExpense = createAction(
  '[Expense] Delete Expense',
  props<{ id: string }>()
);

export const deleteExpenseSuccess = createAction(
  '[Expense] Delete Expense Success',
  props<{ id: string }>()
);

export const deleteExpenseFailure = createAction(
  '[Expense] Delete Expense Failure',
  props<{ error: string }>()
);


export const updateExpense = createAction(
  '[Expense] Update Expense',
  props<{ id: string; payload: Partial<Expense> }>()
);

export const updateExpenseSuccess = createAction(
  '[Expense] Update Expense Success',
  props<{ expense: Expense }>()
);

export const updateExpenseFailure = createAction(
  '[Expense] Update Expense Failure',
  props<{ error: string }>()
);
