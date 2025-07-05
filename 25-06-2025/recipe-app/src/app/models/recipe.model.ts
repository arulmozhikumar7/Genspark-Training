export interface Recipe {
  id: number;
  name: string;
  ingredients: string[];
  prepTimeMinutes: number;
  cookTimeMinutes: number;
  cuisine: string;
  image: string;
}

export interface RecipeResponse {
  recipes: Recipe[];
}