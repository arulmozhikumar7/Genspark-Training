import { Injectable } from '@angular/core';
import { HttpService } from '@core/services/http.service';
import { Observable } from 'rxjs';
import { Budget } from '@shared/models/budget.model';

@Injectable({ providedIn: 'root' })
export class BudgetApiService {
  constructor(private http: HttpService) {}

getAll(params: {
  page: number;
  pageSize: number;
  categoryId?: string;
  search?: string;
  month?: number;
  year?: number;
}): Observable<{ data: { items: Budget[]; totalCount: number } }> {
  return this.http.get<{ data: { items: Budget[]; totalCount: number } }>('/Budget', params);
}



  create(payload: Partial<Budget>): Observable<Budget> {
    return this.http.post<Budget>('/Budget', payload);
  }

  update(id: string, payload: Partial<Budget>): Observable<Budget> {
    return this.http.put<Budget>(`/Budget/${id}`, payload);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`/Budget/${id}`);
  }

  getById(id: string): Observable<{ data: Budget }> {
  return this.http.get<{ data: Budget }>(`/Budget/${id}`);
}

}
