import { Component } from '@angular/core';
import { Product } from './components/product/product';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [Product],
  templateUrl: './app.html'
})
export class App {
  title = 'product-app';
}