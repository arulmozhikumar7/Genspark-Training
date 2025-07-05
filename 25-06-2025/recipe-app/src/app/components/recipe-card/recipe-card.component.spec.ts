import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RecipeCardComponent } from './recipe-card.component';
import { Recipe } from '../../models/recipe.model';
import { By } from '@angular/platform-browser';

describe('RecipeCardComponent', () => {
  let component: RecipeCardComponent;
  let fixture: ComponentFixture<RecipeCardComponent>;

  const mockRecipe: Recipe = {
    id: 1,
    name: 'Test Recipe',
    cuisine: 'Italian',
    prepTimeMinutes: 10,
    cookTimeMinutes: 20,
    ingredients: ['Tomato', 'Pasta', 'Cheese'],
    image: 'https://dummy.com/image.jpg'
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecipeCardComponent] 
    }).compileComponents();

    fixture = TestBed.createComponent(RecipeCardComponent);
    component = fixture.componentInstance;
    component.recipe = mockRecipe;
    fixture.detectChanges(); 
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should display the recipe name', () => {
    const nameEl = fixture.debugElement.query(By.css('.recipe-name')).nativeElement;
    expect(nameEl.textContent).toContain('Test Recipe');
  });

  it('should display the recipe cuisine', () => {
    const cuisineEl = fixture.debugElement.query(By.css('.value')).nativeElement;
    expect(cuisineEl.textContent).toContain('Italian');
  });

  it('should display correct total cooking time', () => {
    const time = component.getTotalCookingTime();
    expect(time).toBe(30);
  });

  it('should return formatted ingredient text', () => {
    const text = component.getIngredientsText();
    expect(text).toBe('Tomato, Pasta, Cheese');
  });

  it('should display the recipe image with correct src and alt', () => {
    const img = fixture.debugElement.query(By.css('img')).nativeElement as HTMLImageElement;
    expect(img.src).toContain('https://dummy.com/image.jpg');
    expect(img.alt).toBe('Test Recipe');
  });
});
