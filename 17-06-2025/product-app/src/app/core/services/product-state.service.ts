import { Injectable, signal } from '@angular/core';
import { Product } from '../../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductStateService {
  searchTerm = signal<string>('');
  products = signal<Product[]>([]);
  skip = signal<number>(0);
  total = signal<number>(0);
  currentQuery = signal<string>('');

  reset(): void {
    this.searchTerm.set('');
    this.products.set([]);
    this.skip.set(0);
    this.total.set(0);
    this.currentQuery.set('');
  }
}
