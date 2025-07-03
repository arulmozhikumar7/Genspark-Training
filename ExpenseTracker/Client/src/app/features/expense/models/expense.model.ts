export interface Expense {
  id: string;
  categoryName: string;
  amount: number;
  description: string;
  expenseDate: string; 
   categoryId?: string;
}
