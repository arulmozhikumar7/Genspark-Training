import { Component, OnInit, inject , EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import { createExpense } from '@store/expense/expense.actions';
import { getCategories } from '@store/category/category.actions';
import { selectAllCategories } from '@store/category/category.selectors';
import { Category } from '@shared/models/category.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  standalone: true,
  selector: 'app-add-expense-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-expense-form.component.html',
})
export class AddExpenseFormComponent implements OnInit {
  @Output() formSubmitted = new EventEmitter<void>();
  private fb = inject(FormBuilder);
  private store = inject(Store);

    categories$: Observable<Category[]> = this.store.select(selectAllCategories).pipe(
    map(categories => categories.filter(cat => cat.name !== 'All'))
  );

  form = this.fb.nonNullable.group({
    description: ['', Validators.required],
    categoryId: ['', Validators.required], 
    amount: 0,
    expenseDate: new Date().toISOString().substring(0, 10),
  });

  ngOnInit(): void {
    this.store.dispatch(getCategories());
  }

  get f() {
    return this.form.controls;
  }

  onSubmit() {
    if (this.form.invalid) return;

    const formValue = this.form.value;

    const utcDate = new Date(formValue.expenseDate + 'T00:00:00').toISOString();

    const payload = {
      ...formValue,
      expenseDate: utcDate,
    };

    this.store.dispatch(createExpense({ payload }));
    this.form.reset();
    this.formSubmitted.emit(); 
  }
}
