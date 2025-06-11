import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable, catchError, throwError } from "rxjs";
import { ProductModel } from "../models/product.model";

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private http = inject(HttpClient);

  getProduct(id: number = 1): Observable<ProductModel> {
    return this.http.get<ProductModel>(`https://dummyjson.com/products/${id}`)
      .pipe(
        catchError((error) => {
          console.error('Error fetching product:', error);
          return throwError(() => error);
        })
      );
  }
}