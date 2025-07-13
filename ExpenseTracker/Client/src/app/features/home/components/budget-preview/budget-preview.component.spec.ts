import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BudgetPreviewComponent } from './budget-preview.component';
import { provideMockStore } from '@ngrx/store/testing';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';

// Mock data
const mockBudgets = [
  {
    name: 'Food',
    startDate: new Date('2024-01-01'),
    endDate: new Date('2024-01-31'),
    balanceAmount: 200,
    limitAmount: 1000,
  },
  {
    name: 'Travel',
    startDate: new Date('2024-02-01'),
    endDate: new Date('2024-02-28'),
    balanceAmount: 100,
    limitAmount: 500,
  },
];

// Selectors
import { selectAllBudgets, selectBudgetLoading } from '@store/budget/budget.selectors';

describe('BudgetPreviewComponent', () => {
  let component: BudgetPreviewComponent;
  let fixture: ComponentFixture<BudgetPreviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        BudgetPreviewComponent,
        RouterTestingModule,
      ],
      providers: [
        provideMockStore({
          selectors: [
            { selector: selectAllBudgets, value: mockBudgets },
            { selector: selectBudgetLoading, value: false },
          ],
        }),
        CurrencyPipe,
        DatePipe,
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({}),
            snapshot: {
              paramMap: {
                get: () => null
              }
            }
          }
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(BudgetPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should display budget items', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelectorAll('li').length).toBeGreaterThan(0);
    expect(compiled.textContent).toContain('Food');
    expect(compiled.textContent).toContain('Travel');
  });
});
