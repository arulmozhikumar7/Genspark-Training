// src/app/app.config.ts
import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app.routes';
import { provideToastr } from 'ngx-toastr';
import { authReducer } from '@store/auth/auth.reducer';
import { AuthEffects } from '@store/auth/auth.effects';
import { ExpenseEffects } from '@store/expense/expense.effects';
import { provideAnimations } from '@angular/platform-browser/animations';
import { expenseReducer } from '@store/expense/expense.reducer';
import { categoryReducer } from '@store/category/category.reducer';
import { CategoryEffects } from '@store/category/category.effects';
import { budgetReducer } from '@store/budget/budget.reducer';
import { BudgetEffects } from '@store/budget/budget.effects';
import {  withInterceptors } from '@angular/common/http';
import { authInterceptor } from '@core/interceptors/auth.interceptor';
export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideStore({
      auth: authReducer,
      expense: expenseReducer,
      category: categoryReducer,
      budget: budgetReducer
    }),
    provideEffects([AuthEffects,ExpenseEffects,CategoryEffects,BudgetEffects]),
    provideHttpClient(),
    provideAnimations(),
    provideToastr({
      positionClass: 'toast-top-center',
      closeButton: true,
      timeOut: 3000,
      progressBar: true,
    }),
     provideHttpClient(
      withInterceptors([authInterceptor])
    )
  ],
};
