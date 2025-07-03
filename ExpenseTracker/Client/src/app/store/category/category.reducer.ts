import { createReducer, on } from '@ngrx/store';
import { initialCategoryState } from './category.state';
import * as CategoryActions from './category.actions';

export const categoryReducer = createReducer(
  initialCategoryState,

  on(CategoryActions.getCategories, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(CategoryActions.getCategoriesSuccess, (state, { categories }) => ({
    ...state,
    loading: false,
    categories,
  })),

  on(CategoryActions.getCategoriesFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  }))
);
