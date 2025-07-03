import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddExpenseFormComponent } from '@features/expense/components/add-expense-form/add-expense-form.component';

@Component({
  standalone: true,
  selector: 'app-add-expense-modal',
  imports: [CommonModule, AddExpenseFormComponent],
  template: `
 <div
  class="fixed inset-0 z-100 flex items-center justify-center bg-black/40 backdrop-blur-sm p-4 sm:p-8"
>
  <div
    class="relative w-full max-w-xl bg-[var(--bg)] text-[var(--text-primary)] rounded-2xl shadow-xl p-6 sm:p-8"
  >
    <!-- Close Button -->
    <button
      (click)="close.emit()"
      class="absolute top-3 right-3 text-[var(--muted)] hover:text-[var(--text-primary)] text-xl font-bold"
    >
      ✖
    </button>

    <!-- Title -->
    <h2 class="text-2xl font-semibold mb-6 text-center">Add Expense</h2>

    <!-- Form Component -->
    <app-add-expense-form (formSubmitted)="close.emit()"></app-add-expense-form>
  </div>
</div>

  `,
})
export class AddExpenseModalComponent {
  @Output() close = new EventEmitter<void>();
}
