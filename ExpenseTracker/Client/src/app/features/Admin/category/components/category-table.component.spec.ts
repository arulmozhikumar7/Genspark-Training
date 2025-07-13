import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CategoryTableComponent } from './category-table.component';
import { CategoryApiService } from '@features/Admin/category/services/category-api.service';
import { NotificationService } from '@core/services/notification.service';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Category } from '@shared/models/category.model';

describe('CategoryTableComponent', () => {
  let component: CategoryTableComponent;
  let fixture: ComponentFixture<CategoryTableComponent>;
  let mockCategoryService: jasmine.SpyObj<CategoryApiService>;
  let mockNotificationService: jasmine.SpyObj<NotificationService>;

  const mockCategories: Category[] = [
    { id: '1', name: 'Food' },
    { id: '2', name: 'Travel' },
  ];

  beforeEach(async () => {
    mockCategoryService = jasmine.createSpyObj<CategoryApiService>(
      'CategoryApiService',
      ['getAllCategories', 'createCategory', 'updateCategory', 'deleteCategory']
    );
    mockNotificationService = jasmine.createSpyObj<NotificationService>('NotificationService', ['success']);

    // IMPORTANT: Mock getAllCategories before component creation
    mockCategoryService.getAllCategories.and.returnValue(of(mockCategories));

    await TestBed.configureTestingModule({
      imports: [CategoryTableComponent, FormsModule, CommonModule],
      providers: [
        { provide: CategoryApiService, useValue: mockCategoryService },
        { provide: NotificationService, useValue: mockNotificationService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(CategoryTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges(); // trigger change detection and subscriptions
  });

  it('should load categories on init', () => {
    expect(mockCategoryService.getAllCategories).toHaveBeenCalled();
    expect(component.categories()).toEqual(mockCategories);
    expect(component.loading()).toBeFalse();
  });

  it('should handle error when loading categories', () => {
    // Overwrite getAllCategories to throw error for this test
    mockCategoryService.getAllCategories.and.returnValue(throwError(() => new Error('fail')));

    // Call loadCategories manually
    component.loadCategories();

    expect(component.error()).toBe('Failed to load categories');
    expect(component.loading()).toBeFalse();
  });

  it('should edit a category', () => {
    component.onEdit(mockCategories[0]);
    expect(component.editingCategory()).toEqual(mockCategories[0]);
  });

  it('should cancel edit', () => {
    component.editingCategory.set(mockCategories[0]);
    component.onCancelEdit();
    expect(component.editingCategory()).toBeNull();
  });

  it('should save an edited category', () => {
    const updatedCategory = { ...mockCategories[0], name: 'Groceries' };
    component.categories.set(mockCategories);
    component.editingCategory.set(updatedCategory);
    mockCategoryService.updateCategory.and.returnValue(of(updatedCategory));

    component.onSaveEdit();

    expect(mockCategoryService.updateCategory).toHaveBeenCalledWith(updatedCategory.id, updatedCategory);
    expect(component.categories()[0].name).toBe('Groceries');
    expect(mockNotificationService.success).toHaveBeenCalledWith('Category Updated Successfully');
    expect(component.editingCategory()).toBeNull();
  });

  it('should delete a category when confirmed', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    component.categories.set(mockCategories);
    mockCategoryService.deleteCategory.and.returnValue(of(undefined));

    component.onDelete('1');

    expect(mockCategoryService.deleteCategory).toHaveBeenCalledWith('1');
    expect(component.categories().length).toBe(1);
    expect(mockNotificationService.success).toHaveBeenCalledWith('Category Deleted Successfully');
  });

  it('should not delete a category when not confirmed', () => {
    spyOn(window, 'confirm').and.returnValue(false);

    component.onDelete('1');

    expect(mockCategoryService.deleteCategory).not.toHaveBeenCalled();
  });

  it('should open and close the add modal', () => {
    component.openAddModal();
    expect(component.isAddModalOpen()).toBeTrue();
    expect(component.newCategoryName).toBe('');

    component.closeAddModal();
    expect(component.isAddModalOpen()).toBeFalse();
  });


  it('should alert when adding new category fails', () => {
    component.newCategoryName = 'Books';
    mockCategoryService.createCategory.and.returnValue(throwError(() => new Error('fail')));
    spyOn(window, 'alert');

    const mockForm = {
      invalid: false,
    } as any;

    component.onSubmitNewCategory(mockForm);

    expect(mockCategoryService.createCategory).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith('Failed to add category');
  });

  it('should not add new category if form is invalid', () => {
    const mockForm = {
      invalid: true,
    } as any;

    component.onSubmitNewCategory(mockForm);

    expect(mockCategoryService.createCategory).not.toHaveBeenCalled();
  });
});
