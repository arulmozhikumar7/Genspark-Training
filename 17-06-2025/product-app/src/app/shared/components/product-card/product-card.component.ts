import { Component, Input,Output,EventEmitter } from '@angular/core';
import { Product } from '../../../models/product.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css']
})
export class ProductCardComponent {
  @Input() product!: Product;
  @Output() clicked = new EventEmitter<number>();

  onCardClick(): void{
    this.clicked.emit(this.product.id);
  }
}
