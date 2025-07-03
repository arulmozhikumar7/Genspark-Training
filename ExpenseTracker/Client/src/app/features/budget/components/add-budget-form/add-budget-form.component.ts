import { Component, EventEmitter, inject, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { createBudget } from '@store/budget/budget.actions';
import { getCategories } from '@store/category/category.actions';
import { selectAllCategories } from '@store/category/category.selectors';
import { Category } from '@shared/models/category.model';
import { Observable } from 'rxjs';

@Component({
  standalone: true,
  selector: 'app-add-budget-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-budget-form.component.html',
})
export class AddBudgetFormComponent implements OnInit {
  @Output() formSubmitted = new EventEmitter<void>();

  private fb = inject(FormBuilder);
  private store = inject(Store);

  categories$: Observable<Category[]> = this.store.select(selectAllCategories);

  form = this.fb.group({
    Name: ['', Validators.required],
    categoryId: ['', Validators.required],
    limitAmount: [0, [Validators.required, Validators.min(1)]],
    startDate: ['', Validators.required],
    endDate: ['', Validators.required],
  });

  ngOnInit(): void {
    this.store.dispatch(getCategories());
  }

  get f() {
    return this.form.controls;
  }

  onSubmit() {
    if (this.form.invalid) return;

    const { Name, startDate, endDate, categoryId, limitAmount } = this.form.value;

    const payload = {
      Name: Name ?? undefined,
      categoryId: categoryId ?? undefined,
      limitAmount: limitAmount ?? undefined,
      startDate: new Date(startDate + 'T00:00:00').toISOString(),
      endDate: new Date(endDate + 'T23:59:59').toISOString(),
    };

    this.store.dispatch(createBudget({ payload }));
    this.form.reset();
    this.formSubmitted.emit();
  }
}
