export interface MonthlyCategoryComparisonDto {
  categoryName: string;
  month1Amount: number;
  month2Amount: number;
  difference: number;
}

export interface MonthlyExpenseComparisonSummaryDto {
  year1: number;
  month1: number;
  year2: number;
  month2: number;
  totalMonth1: number;
  totalMonth2: number;
totalDifference: number;
  totalPercentageChange: number;
  categoryComparisons: MonthlyCategoryComparisonDto[];
}
