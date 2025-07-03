import { Budget } from '@shared/models/budget.model';

export interface BudgetState {
  budgets: Budget[];
  loading: boolean;
  error: string | null;
  totalCount: number;
}
