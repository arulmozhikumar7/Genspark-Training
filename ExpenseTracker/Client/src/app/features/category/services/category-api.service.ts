import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from '@core/services/http.service';
import { CategoryListResponse } from '@shared/models/category.model';

@Injectable({ providedIn: 'root' })
export class CategoryApiService {
  constructor(private http: HttpService) {}

  getCategories(): Observable<CategoryListResponse> {
    return this.http.get<CategoryListResponse>('/Category');
  }
}
