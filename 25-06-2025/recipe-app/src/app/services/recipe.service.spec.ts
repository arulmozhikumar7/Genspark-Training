import { TestBed } from '@angular/core/testing';
import { RecipeService } from './recipe.service';
import { RecipeResponse } from '../models/recipe.model';

describe('RecipeService (with Signals)', () => {
  let service: RecipeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RecipeService);
  });

  it('should initialize with empty recipes', () => {
    expect(service.recipes()).toEqual([]);
    expect(service.isLoading()).toBeFalse();
    expect(service.error()).toBeNull();
  });

  it('should fetch and set recipes successfully', async () => {
    const mockResponse: RecipeResponse = {
      recipes: [
        {
          id: 1,
          name: 'Mock Dish',
          cuisine: 'Mock Cuisine',
          prepTimeMinutes: 5,
          cookTimeMinutes: 10,
          ingredients: ['ingredient1', 'ingredient2'],
          image: 'image.jpg'
        }
      ],
    };

    spyOn(window, 'fetch').and.resolveTo(new Response(JSON.stringify(mockResponse), {
      status: 200,
      headers: { 'Content-type': 'application/json' }
    }));

    await service.fetchRecipes();

    expect(service.recipes().length).toBe(1);
    expect(service.recipes()[0].name).toBe('Mock Dish');
    expect(service.error()).toBeNull();
    expect(service.isLoading()).toBeFalse();
  });

  it('should handle fetch error', async () => {
    spyOn(window, 'fetch').and.resolveTo(new Response('Internal Server Error', {
      status: 500,
      statusText: 'Internal Server Error'
    }));

    await service.fetchRecipes();

    expect(service.recipes()).toEqual([]);
    expect(service.error()).toContain('HTTP error');
    expect(service.isLoading()).toBeFalse();
  });

  it('should clear recipes and error', () => {
    service.recipes.set([{ id: 1, name: 'Test', cuisine: '', prepTimeMinutes: 0, cookTimeMinutes: 0, ingredients: [], image: '' }]);
    service.error.set('Some error');

    service.clearRecipes();

    expect(service.recipes()).toEqual([]);
    expect(service.error()).toBeNull();
  });
});
