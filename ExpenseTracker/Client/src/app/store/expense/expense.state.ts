import { Expense } from '@features/expense/models/expense.model';

export interface ExpenseState {
  expenses: Expense[];
  loading: boolean;
  error: string | null;
  totalCount: number;
}

export const initialExpenseState: ExpenseState = {
  expenses: [],
  loading: false,
  error: null,
  totalCount: 0
};
