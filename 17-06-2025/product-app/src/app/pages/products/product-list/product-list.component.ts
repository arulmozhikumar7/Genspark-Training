import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';
import { FormsModule } from '@angular/forms';
import { ProductCardComponent } from '../../../shared/components/product-card/product-card.component';
import { Subject, debounceTime, distinctUntilChanged, switchMap, tap ,filter} from 'rxjs';
import { InfiniteScrollDirective } from 'ngx-infinite-scroll';
import { NavbarComponent } from '../../../shared/components/navbar/navbar.component';
import { ProductStateService } from '../../../core/services/product-state.service';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ProductCardComponent, InfiniteScrollDirective, NavbarComponent],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent {
  private productService = inject(ProductService);
  private router = inject(Router);
  private state = inject(ProductStateService);

  searchSubject = new Subject<string>();
  limit = 10;
  loading = false;

  get searchTerm() {
    return this.state.searchTerm();
  }
  get products() {
    return this.state.products();
  }

  constructor() {
    this.searchSubject.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      filter(query => query.trim().length > 0), 
      tap(() => {
        this.state.skip.set(0);
        this.state.products.set([]);
        this.loading = true;
      }),
      switchMap(query => {
        this.state.currentQuery.set(query);
        return this.productService.searchProducts(query, this.limit, 0);
      })
    ).subscribe(res => {
      this.state.products.set(res.products);
      this.state.total.set(res.total);
      this.state.skip.set(this.limit);
      this.loading = false;
    });
  }

  onSearchChange(value: string): void {
  const trimmed = value.trim();
  this.state.searchTerm.set(trimmed);

  if (trimmed.length === 0) {
    this.state.products.set([]);
    this.state.total.set(0);
    return;
  }

  this.searchSubject.next(trimmed);
}

  onScroll(): void {
    if (this.loading || this.products.length >= this.state.total()) return;
    this.loading = true;
    this.productService
      .searchProducts(this.state.currentQuery(), this.limit, this.state.skip())
      .subscribe(res => {
        this.state.products.update(p => [...p, ...res.products]);
        this.state.skip.set(this.state.skip() + this.limit);
        this.loading = false;
      });
  }

  navigateToDetail(id: number): void {
    this.router.navigate(['/products', id]);
  }
}
