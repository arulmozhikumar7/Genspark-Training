import { Routes } from '@angular/router';
import { CategoryTableComponent } from '@features/Admin/category/components/category-table.component';

export const categoryRoutes: Routes = [
  {
    path: '',
    component: CategoryTableComponent,
    title: 'Category List',
  }
];
