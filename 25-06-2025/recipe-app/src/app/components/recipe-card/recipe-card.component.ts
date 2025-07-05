// recipe-card.component.ts
import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Recipe } from '../../models/recipe.model';

@Component({
  selector: 'app-recipe-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './recipe-card.component.html',
  styleUrls: ['./recipe-card.component.css']
})
export class RecipeCardComponent {
  @Input() recipe!: Recipe;

  getTotalCookingTime(): number {
    return this.recipe.prepTimeMinutes + this.recipe.cookTimeMinutes;
  }

  getIngredientsText(): string {
    return this.recipe.ingredients.join(', ');
  }
}
