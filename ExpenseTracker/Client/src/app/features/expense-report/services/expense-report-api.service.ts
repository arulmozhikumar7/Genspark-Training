import { Injectable } from '@angular/core';
import { HttpService } from '@core/services/http.service';
import { saveAs } from 'file-saver';
import { HttpParams } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ExpenseReportApiService {
  constructor(private http: HttpService) {}

  private buildHttpParams(params: any): HttpParams {
    let httpParams = new HttpParams();
    Object.keys(params || {}).forEach(key => {
      if (params[key] !== undefined && params[key] !== null) {
        httpParams = httpParams.set(key, params[key].toString());
      }
    });
    return httpParams;
  }

  downloadCSV(params: any, filename: string): void {
    const httpParams = this.buildHttpParams(params);

    this.http.downloadBlob(`/Expense/export/csv`, httpParams)
      .subscribe(blob => {
        saveAs(blob, filename);
      });
  }
}
