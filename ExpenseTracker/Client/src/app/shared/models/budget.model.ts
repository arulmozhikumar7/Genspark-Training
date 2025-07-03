export interface Budget {
  id: string;
  name?: string | null;
  categoryId: string;
  categoryName: string;
  limitAmount: number;
  balanceAmount: number;
  startDate: string; 
  endDate: string;  
}

export interface BudgetistResponse {
  success: boolean;
  message: string;
  data: {
    items: Budget[];
    totalCount: number;
  };
  errors: any;
}
