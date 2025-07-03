import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddBudgetFormComponent } from './add-budget-form.component';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { selectAllCategories } from '@store/category/category.selectors';
import { getCategories } from '@store/category/category.actions';
import { createBudget } from '@store/budget/budget.actions';
import { By } from '@angular/platform-browser';
import { Category } from '@shared/models/category.model';

describe('AddBudgetFormComponent', () => {
  let component: AddBudgetFormComponent;
  let fixture: ComponentFixture<AddBudgetFormComponent>;
  let store: MockStore;
  let dispatchSpy: jasmine.Spy;

  const mockCategories: Category[] = [
    { id: '1', name: 'Food' },
    { id: '2', name: 'Travel' }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddBudgetFormComponent],
      providers: [
        provideMockStore({
          initialState: {},
        }),
      ],
    }).compileComponents();

    store = TestBed.inject(MockStore);
    store.overrideSelector(selectAllCategories, mockCategories);
    fixture = TestBed.createComponent(AddBudgetFormComponent);
    component = fixture.componentInstance;
    dispatchSpy = spyOn(store, 'dispatch').and.callThrough();
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should dispatch getCategories on init', () => {
    expect(dispatchSpy).toHaveBeenCalledWith(getCategories());
  });

  it('should render form fields and submit button', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('select[formControlName="categoryId"]')).toBeTruthy();
    expect(compiled.querySelector('input[formControlName="Name"]')).toBeTruthy();
    expect(compiled.querySelector('input[formControlName="limitAmount"]')).toBeTruthy();
    expect(compiled.querySelector('input[formControlName="startDate"]')).toBeTruthy();
    expect(compiled.querySelector('input[formControlName="endDate"]')).toBeTruthy();
    expect(compiled.querySelector('button[type="submit"]')).toBeTruthy();
  });

  it('should disable submit button when form is invalid', () => {
    component.form.patchValue({
      Name: '',
      categoryId: '',
      limitAmount: 0,
      startDate: '',
      endDate: ''
    });
    fixture.detectChanges();

    const submitButton = fixture.debugElement.query(By.css('button[type="submit"]'));
    expect(submitButton.nativeElement.disabled).toBeTrue();
  });

  it('should dispatch createBudget and reset form when submitted with valid values', () => {
    component.form.setValue({
      Name: 'Test Budget',
      categoryId: '1',
      limitAmount: 5000,
      startDate: '2025-06-01',
      endDate: '2025-06-30'
    });

    fixture.detectChanges();

    const formResetSpy = spyOn(component.form, 'reset');

    component.onSubmit();

    const expectedPayload = {
      Name: 'Test Budget',
      categoryId: '1',
      limitAmount: 5000,
      startDate: new Date('2025-06-01T00:00:00').toISOString(),
      endDate: new Date('2025-06-30T23:59:59').toISOString(),
    };

    expect(dispatchSpy).toHaveBeenCalledWith(createBudget({ payload: expectedPayload }));
    expect(formResetSpy).toHaveBeenCalled();
  });

  it('should not dispatch createBudget if form is invalid', () => {
    component.form.patchValue({
      Name: '',
      categoryId: '',
      limitAmount: 0,
      startDate: '',
      endDate: ''
    });

    component.onSubmit();

  
    expect(dispatchSpy.calls.allArgs().filter(([action]) => action.type === '[Budget] Create Budget').length).toBe(0);
  });
});
