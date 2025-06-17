import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class ProductService {
  constructor(private http: HttpClient) {}

  searchProducts(query: string, skip: number = 0) {
    return this.http.get<any>(`https://dummyjson.com/products/search?q=${query}&limit=10&skip=${skip}`)
      .pipe(map(res => res.products));
  }
}
