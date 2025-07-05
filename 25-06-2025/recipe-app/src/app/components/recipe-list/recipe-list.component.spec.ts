import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RecipeListComponent } from './recipe-list.component';
import { RecipeService } from '../../services/recipe.service';
import { signal } from '@angular/core';
import { Recipe } from '../../models/recipe.model';
import { By } from '@angular/platform-browser';

describe('RecipeListComponent', () => {
  let fixture: ComponentFixture<RecipeListComponent>;
  let component: RecipeListComponent;

  // Signal mocks
  const recipesSignal = signal<Recipe[]>([]);
  const isLoadingSignal = signal<boolean>(false);
  const errorSignal = signal<string | null>(null);

  // Service mock
  const mockService: Partial<RecipeService> = {
    fetchRecipes: jasmine.createSpy('fetchRecipes'),
    clearRecipes: jasmine.createSpy('clearRecipes'),
    get recipes() {
      return recipesSignal;
    },
    get isLoading() {
      return isLoadingSignal;
    },
    get error() {
      return errorSignal;
    }
  };

  beforeEach(async () => {
    recipesSignal.set([]);
    isLoadingSignal.set(false);
    errorSignal.set(null);

    await TestBed.configureTestingModule({
      imports: [RecipeListComponent],
      providers: [{ provide: RecipeService, useValue: mockService }]
    }).compileComponents();

    fixture = TestBed.createComponent(RecipeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should call loadRecipes when Load Recipes button is clicked', () => {
    const loadBtn = fixture.debugElement.query(By.css('.btn-primary')).nativeElement;
    loadBtn.click();
    expect(mockService.fetchRecipes).toHaveBeenCalled();
  });

  it('should call clearRecipes when Clear Recipes button is clicked', () => {
    const clearBtn = fixture.debugElement.query(By.css('.btn-secondary')).nativeElement;
    clearBtn.click();
    expect(mockService.clearRecipes).toHaveBeenCalled();
  });

  it('should show loading indicator if isLoading is true', () => {
    isLoadingSignal.set(true);
    fixture.detectChanges();
    const loadingEl = fixture.debugElement.query(By.css('.loading'));
    expect(loadingEl).toBeTruthy();
    expect(loadingEl.nativeElement.textContent).toContain('Loading recipes...');
  });

  it('should display error message when error exists', () => {
    errorSignal.set('Something went wrong');
    fixture.detectChanges();
    const errorEl = fixture.debugElement.query(By.css('.error'));
    expect(errorEl).toBeTruthy();
    expect(errorEl.nativeElement.textContent).toContain('Something went wrong');
  });

  it('should display recipe cards when recipes are available', () => {
    const mockRecipe: Recipe = {
      id: 1,
      name: 'Paneer Tikka',
      cuisine: 'Indian',
      cookTimeMinutes: 15,
      prepTimeMinutes: 10,
      ingredients: ['Paneer', 'Spices'],
      image: 'image.jpg'
    };

    recipesSignal.set([mockRecipe]);
    fixture.detectChanges();

    const card = fixture.debugElement.query(By.css('app-recipe-card'));
    expect(card).toBeTruthy();
  });
});
