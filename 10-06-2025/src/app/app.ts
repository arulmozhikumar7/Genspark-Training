import { Component } from '@angular/core';
import { CustomerDetails } from './customer-details/customer-details';
import { Products } from './products/products';

@Component({
  selector: 'app-root',
  imports: [CustomerDetails,Products],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'angularapp';
}
