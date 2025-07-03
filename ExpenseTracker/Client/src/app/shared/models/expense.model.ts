import { Expense } from "@features/expense/models/expense.model";

export interface ExpenseListResponse {
  success: boolean;
  message: string;
  data: {
    items: Expense[];
    totalCount: number;
  };
  errors: any;
}
