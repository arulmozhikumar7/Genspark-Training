import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecipeService } from '../../services/recipe.service';
import { Recipe } from '../../models/recipe.model';

@Component({
  selector: 'app-recipe-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.css']
})
export class RecipeListComponent {
  private recipeService = inject(RecipeService);

  get recipes() {
    return this.recipeService.recipes;
  }

  get isLoading() {
    return this.recipeService.isLoading;
  }

  get error() {
    return this.recipeService.error;
  }

  loadRecipes(): void {
    this.recipeService.fetchRecipes();
  }

  getTotalCookingTime(recipe: Recipe): number {
    return recipe.prepTimeMinutes + recipe.cookTimeMinutes;
  }

  getIngredientsText(ingredients: string[]): string {
    return ingredients.join(', ');
  }

  clearRecipes(): void {
    this.recipeService.clearRecipes();
  }
}

