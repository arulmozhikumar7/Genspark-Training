import { Routes } from '@angular/router';
import { authRoutes } from '@features/auth/auth.routes';
import { authGuardFn } from '@core/guards/auth.guard';
import { adminGuardFn } from '@core/guards/admin.guard';
import { userGuardFn } from '@core/guards/user.guard';

import { LayoutComponent } from '@shared/components/layout/layout.component'; 

import { HomePage } from '@features/home/home.page';
import { BudgetListPage } from '@features/budget/pages/budget-list/budget-list.page';
import { BudgetDetailPageComponent } from '@features/budget/pages/budget-detail/budget-detail.page';
import { ExpenseListPage } from '@features/expense/pages/expense-list/expense-list.page';
import { NotificationComponent } from '@features/notification/components/notification.component';
import { ExpenseReportPage } from '@features/expense-report/pages/expense-report.page';

import { categoryRoutes } from '@features/Admin/category/category.routes';
import { RoleRedirectComponent } from '@shared/components/role-redirect.component';
import { DashboardComponent } from '@features/dashboard/components/dashboard.component';

export const routes: Routes = [
  {
    path: 'auth',
    children: authRoutes,
  },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuardFn],
    children: [
      { path: '', component:RoleRedirectComponent},
      { path: 'home', component: HomePage, canActivate: [userGuardFn] },
      { path: 'expenses', component: ExpenseListPage, canActivate: [userGuardFn] },
      { path: 'budget', component: BudgetListPage, canActivate: [userGuardFn] },
      { path: 'budgets/:id', component: BudgetDetailPageComponent, canActivate: [userGuardFn] },
      { path: 'notifications', component: NotificationComponent, canActivate: [userGuardFn] },
      { path: 'report', component: ExpenseReportPage, canActivate: [userGuardFn] },
      { path: 'compare-expense',component:DashboardComponent,canActivate:[userGuardFn]},
     
      {
        path: 'category',
        canActivate: [adminGuardFn],
        loadChildren: () => import('@features/Admin/category/category.routes').then(m => m.categoryRoutes),
      },
    ],
  },
  {
    path: '',
    redirectTo: 'auth/login',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'auth/login',
  },
];
