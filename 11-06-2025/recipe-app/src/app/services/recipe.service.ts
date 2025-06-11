import { Injectable, signal } from '@angular/core';
import { Recipe, RecipeResponse } from '../models/recipe.model';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private readonly API_BASE_URL = 'https://dummyjson.com/recipes';
  
 
  recipes = signal<Recipe[]>([]);
  isLoading = signal<boolean>(false);
  error = signal<string | null>(null);

  
  async fetchRecipes(): Promise<void> {
    this.isLoading.set(true);
    this.error.set(null);
    
    try {
      const response = await fetch(`${this.API_BASE_URL}?limit=10`);
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data: RecipeResponse = await response.json();
      this.recipes.set(data.recipes);
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Failed to fetch recipes';
      this.error.set(errorMessage);
      console.error('Error fetching recipes:', error);
    } finally {
      this.isLoading.set(false);
    }
  }

  clearRecipes(): void {
    this.recipes.set([]);
    this.error.set(null);
  }
}