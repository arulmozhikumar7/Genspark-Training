import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Category } from '@shared/models/category.model';
import { FormsModule, NgForm } from '@angular/forms';
import { CategoryApiService } from '@features/Admin/category/services/category-api.service';
import { NotificationService } from '@core/services/notification.service';
@Component({
  selector: 'app-category-table',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './category-table.component.html',
})
export class CategoryTableComponent {
  private toast = inject(NotificationService);
  categories = signal<Category[]>([]);
  editingCategory = signal<Category | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);


  isAddModalOpen = signal(false);
  newCategoryName = ''; 


  constructor(private categoryService: CategoryApiService) {
    this.loadCategories();
  }

  loadCategories() {
    this.loading.set(true);
    this.categoryService.getAllCategories().subscribe({
      next: (data) => {
        this.categories.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load categories');
        this.loading.set(false);
      },
    });
  }

  onEdit(cat: Category) {
    this.editingCategory.set({ ...cat });
  }

  onCancelEdit() {
    this.editingCategory.set(null);
  }

  onSaveEdit() {
    const edited = this.editingCategory();
    if (!edited) return;

    this.categoryService.updateCategory(edited.id, edited).subscribe(() => {
      const updatedList = this.categories().map((c) =>
        c.id === edited.id ? edited : c
      );
      this.categories.set(updatedList);
      this.toast.success('Category Updated Successfully')
      this.editingCategory.set(null);
    });
  }

  onDelete(id: string) {
    if (!confirm('Are you sure you want to delete this category?')) return;
    this.categoryService.deleteCategory(id).subscribe(() => {
      this.categories.set(this.categories().filter((c) => c.id !== id));
      this.toast.success('Category Deleted Successfully')
    });
  }


  openAddModal() {
    this.newCategoryName = '';
    this.isAddModalOpen.set(true);
  }

  closeAddModal() {
    this.isAddModalOpen.set(false);
  }

 onSubmitNewCategory(form: NgForm) {
  if (form.invalid) return;

  this.categoryService.createCategory({ name: this.newCategoryName }).subscribe({
    next: (created) => {
      this.categories.set([...this.categories(), created]);
      this.closeAddModal();
      this.toast.success('Category Added Successfully')
      this.newCategoryName = ''; 
    },
    error: () => {
      alert('Failed to add category');
    },
  });
}

}
