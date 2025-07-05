import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecipeService } from '../../services/recipe.service';
import { RecipeCardComponent } from '../recipe-card/recipe-card.component';

@Component({
  selector: 'app-recipe-list',
  standalone: true,
  imports: [CommonModule, RecipeCardComponent],
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

  clearRecipes(): void {
    this.recipeService.clearRecipes();
  }
}
