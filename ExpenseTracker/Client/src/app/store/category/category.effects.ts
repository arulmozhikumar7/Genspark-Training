import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as CategoryActions from './category.actions';
import { CategoryApiService } from '@features/category/services/category-api.service';
import { catchError, map, of, switchMap } from 'rxjs';

@Injectable()
export class CategoryEffects {
  private actions$ = inject(Actions);
  private api = inject(CategoryApiService);

  loadCategories$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CategoryActions.getCategories),
      switchMap(() =>
        this.api.getCategories().pipe(
          map((res) =>
            CategoryActions.getCategoriesSuccess({ categories: res.data })
          ),
          catchError((err) =>
            of(
              CategoryActions.getCategoriesFailure({
                error: err?.error?.message || 'Failed to fetch categories',
              })
            )
          )
        )
      )
    )
  );
}
