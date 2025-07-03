import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { HttpService } from '@core/services/http.service';

export interface Receipt {
  id: string;
  expenseId: string;
  fileName: string;
  contentType: string;
  createdAt: string;
}

@Injectable({ providedIn: 'root' })
export class ReceiptApiService {
  constructor(private http: HttpService) {}

   uploadReceipt(expenseId: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<any>(`/Receipt/upload/${expenseId}`, formData);
  }

  getReceiptsByExpenseId(expenseId: string): Observable<{data:Receipt[]}> {
    return this.http.get<{data:Receipt[]}>(`/Receipt/expense/${expenseId}`);
  }

  downloadReceipt(id: string): Observable<Blob> {
    return this.http.downloadBlob(`/Receipt/download/${id}`);
  }

  deleteReceipt(id: string): Observable<void> {
    return this.http.delete<void>(`/Receipt/${id}`);
  }
}
