import { Component } from '@angular/core';


interface Customer {
  name: string;
  email: string;
  phone: string;
  address: string;
}

@Component({
  selector: 'app-customer-details',
  imports: [],
  templateUrl: './customer-details.html',
  styleUrl: './customer-details.css'
})
export class CustomerDetails {
 
  customer: Customer = {
    name: 'Arulmozhikumar K',
    email: 'arulmozhikumar7@gmail.com',
    phone: '+91 8122509442',
    address: '123 Main St, Anytown, India'
  };

  likeCount: number = 0;
  dislikeCount: number = 0;

  onLike(): void {
    this.likeCount++;
  }

  onDislike(): void {
    this.dislikeCount++;
  }
}