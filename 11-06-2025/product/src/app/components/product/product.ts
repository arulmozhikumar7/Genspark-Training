import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product.service';
import { ProductModel } from '../../models/product.model';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product.html',
  styleUrl: './product.css'
})
export class Product implements OnInit {
  product: ProductModel | null = null;
  loading = true;
  error = false;
  private productService = inject(ProductService);

  ngOnInit() {
    this.loadProduct();
  }

  loadProduct() {
    this.loading = true;
    this.error = false;
    
    this.productService.getProduct(3).subscribe({
      next: (data) => {
        this.product = data;
        this.loading = false;
        console.log('Product loaded:', this.product);
      },
      error: (err) => {
        console.error('Error loading product:', err);
        this.error = true;
        this.loading = false;
      },
      complete: () => {
        console.log('Product loading completed');
      }
    });
  }

  getDiscountedPrice(): number {
    if (!this.product) return 0;
    return this.product.price * (1 - this.product.discountPercentage / 100);
  }

  getStars(): number[] {
    if (!this.product) return [];
    return Array(Math.floor(this.product.rating)).fill(0);
  }

  getEmptyStars(): number[] {
    if (!this.product) return [];
    return Array(5 - Math.floor(this.product.rating)).fill(0);
  }

  buyProduct() {
    if (this.product) {
      alert(`Buying ${this.product.title} for ₹${this.getDiscountedPrice().toFixed(2)}`);
    }
  }
}