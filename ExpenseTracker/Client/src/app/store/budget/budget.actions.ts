import { createAction, props } from '@ngrx/store';
import { Budget, BudgetistResponse } from '@shared/models/budget.model';

export const getBudgets = createAction(
  '[Budget] Get Budgets',
  props<{ page: number; pageSize: number; categoryId?: string; search?: string; month?: number; year?: number }>()
);

export const getBudgetsSuccess = createAction(
  '[Budget] Get Budgets Success',
  props<{ budgets: Budget[]; totalCount: number }>() 
);

export const getBudgetsFailure = createAction('[Budget] Get Budgets Failure', props<{ error: string }>());

export const createBudget = createAction('[Budget] Create Budget', props<{ payload: Partial<Budget> }>());
export const createBudgetSuccess = createAction('[Budget] Create Budget Success', props<{ budget: Budget }>());
export const createBudgetFailure = createAction('[Budget] Create Budget Failure', props<{ error: string }>());

export const updateBudget = createAction('[Budget] Update Budget', props<{ id: string; payload: Partial<Budget> }>());
export const updateBudgetSuccess = createAction('[Budget] Update Budget Success', props<{ budget: Budget }>());
export const updateBudgetFailure = createAction('[Budget] Update Budget Failure', props<{ error: string }>());

export const deleteBudget = createAction('[Budget] Delete Budget', props<{ id: string }>());
export const deleteBudgetSuccess = createAction('[Budget] Delete Budget Success', props<{ id: string }>());
export const deleteBudgetFailure = createAction('[Budget] Delete Budget Failure', props<{ error: string }>());
