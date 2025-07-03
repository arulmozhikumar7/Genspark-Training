import { createAction, props } from '@ngrx/store';
import { Category } from '@shared/models/category.model';

export const getCategories = createAction('[Category] Get Categories');

export const getCategoriesSuccess = createAction(
  '[Category] Get Categories Success',
  props<{ categories: Category[] }>()
);

export const getCategoriesFailure = createAction(
  '[Category] Get Categories Failure',
  props<{ error: string }>()
);
