import { Injectable } from '@angular/core';
import { HttpService } from '@core/services/http.service';
import { Observable } from 'rxjs';
import { Expense } from '@features/expense/models/expense.model';
import { ExpenseListResponse } from '@shared/models/expense.model';
import { HttpParams } from '@angular/common/http';
@Injectable({ providedIn: 'root' })
export class ExpenseApiService {
  constructor(private http: HttpService) {}

    getAll(params: {
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
}): Observable<ExpenseListResponse> {
  return this.http.get<ExpenseListResponse>('/Expense/filtered', params); 
}


  getById(id: string): Observable<Expense> {
    return this.http.get<Expense>(`/Expense/${id}`);
  }

  create(payload: Partial<Expense>): Observable<Expense> {
    return this.http.post<Expense>('/Expense', payload);
  }

  update(id: string, payload: Partial<Expense>): Observable<Expense> {
    return this.http.put<Expense>(`/Expense/${id}`, payload);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`/Expense/${id}`);
  }
  getExpenseSummary(filters: any = {}) {
  return this.http.get<{ categoryName: string; totalAmount: number }[]>(
  '/Expense/summary',
  { ...filters }
);
}

}
