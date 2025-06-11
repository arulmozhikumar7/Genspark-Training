import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
interface Product {
  id: number;
  name: string;
  price: number;
  image: string;
  description: string;
}

@Component({
  selector: 'app-products',
  templateUrl: './products.html',
  styleUrls: ['./products.css'],
  imports: [CommonModule], 
})
export class Products {
  cartCount : number = 0;

  products: Product[] = [
    {
      id: 1,
      name: 'Wireless Headphones',
      price: 100,
      image: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=300&h=250&fit=crop',
      description: 'Premium wireless headphones with noise cancellation'
    },
    {
      id: 2,
      name: 'Smart Watch',
      price: 150,
      image: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=300&h=250&fit=crop',
      description: 'Advanced smartwatch with fitness tracking'
    },
    {
      id: 3,
      name: 'Bluetooth Speaker',
      price: 300,
      image: 'https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?w=300&h=250&fit=crop',
      description: 'Portable speaker with crystal clear sound'
    }
  ];

  addToCart(product: Product) : void {
    this.cartCount++;
    console.log(`${product.name} added to cart! Total items: ${this.cartCount}`);
  }
}