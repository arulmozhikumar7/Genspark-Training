import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddBudgetFormComponent } from '@features/budget/components/add-budget-form/add-budget-form.component';

@Component({
  standalone: true,
  selector: 'app-add-budget-modal',
  imports: [CommonModule, AddBudgetFormComponent],
  template: `
<div class="fixed inset-0 z-[100] flex items-center justify-center bg-black/40 backdrop-blur-sm px-4 py-6">
  <div
    class="w-full max-w-lg bg-[var(--surface)] rounded-2xl shadow-lg border border-[var(--border)] text-[var(--text-primary)] max-h-full flex flex-col relative"
  >
    <!-- Close Button -->
    <button
      (click)="close.emit()"
      class="absolute top-4 right-4 text-[var(--text-primary)] hover:text-red-600 text-2xl transition"
      aria-label="Close"
    >
      ✖
    </button>

    <!-- Header -->
    <div class="px-6 pt-6 pb-3 border-b border-[var(--border)]">
      <h2 class="text-xl font-semibold">➕ Add Budget</h2>
    </div>

    <!-- Form Section -->
    <div class="px-6 py-4 overflow-y-auto max-h-[75vh] custom-scroll">
      <app-add-budget-form (formSubmitted)="close.emit()"></app-add-budget-form>
    </div>
  </div>
</div>

  `,
})
export class AddBudgetModalComponent {
  @Output() close = new EventEmitter<void>();
}
