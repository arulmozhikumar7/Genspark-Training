import {
  Component,
  OnInit,
  OnDestroy
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, fromEvent, map, Subject, switchMap, takeUntil, tap, filter } from 'rxjs';
import { ProductService } from '../../services/product.service';
import { ProductCardComponent } from '../../components/product-card/product-card.component';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [CommonModule, FormsModule, ProductCardComponent],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class HomeComponent implements OnInit, OnDestroy {
  search = '';
  products: any[] = [];
  private skip = 0;
  private readonly limit = 10;
  loading = false;
  hasMore = true;

  private destroy$ = new Subject<void>();
  private searchSubject = new Subject<string>();
  private loadMoreSubject = new Subject<void>();

  constructor(private productService: ProductService) {}

  ngOnInit() {
    this.setupDebouncedSearch();
    this.setupScrollListener();
    this.setupLoadMoreHandler();
  }

  private setupDebouncedSearch() {
    this.searchSubject.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      tap(() => {
        this.skip = 0;
        this.products = [];
        this.hasMore = true;
        this.loading = true;
      }),
      switchMap(query =>
        this.productService.searchProducts(query, this.skip).pipe(
          tap((res) => {
            this.products = res;
            this.hasMore = res.length === this.limit;
            this.loading = false;
          })
        )
      ),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  private setupLoadMoreHandler() {
    this.loadMoreSubject.pipe(
      filter(() => this.hasMore && !this.loading),
      tap(() => {
        this.loading = true;
        this.skip += this.limit;
      }),
      switchMap(() =>
        this.productService.searchProducts(this.search, this.skip).pipe(
          tap((res) => {
            this.products = [...this.products, ...res];
            this.hasMore = res.length === this.limit;
            this.loading = false;
          })
        )
      ),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  onSearchChange(value: string) {
    this.searchSubject.next(value.trim());
  }

  private setupScrollListener() {
    fromEvent(window, 'scroll').pipe(
      map(() => window.innerHeight + window.scrollY >= document.body.offsetHeight - 300),
      filter(atBottom => atBottom),
      tap(() => {
        this.loadMoreSubject.next();
      }),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  highlight(text: string): string {
    const term = this.search.trim();
    if (!term) return text;
    return text.replace(
      new RegExp(`(${term})`, 'gi'),
      `<mark>$1</mark>`
    );
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
