import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-reactive-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './reactive-form.html',
  styleUrl: './reactive-form.css'
 
})
export class ReactiveFormComponent implements OnInit {
  userForm!: FormGroup;
  reactiveSubmitted = false;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.userForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.pattern(/^\d{10}$/)]],
      age: ['', [Validators.required, Validators.min(18), Validators.max(100)]]
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.userForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }

  onSubmit() {
    if (this.userForm.valid) {
      this.reactiveSubmitted = true;
      console.log('Reactive Form:', this.userForm.value);
    }
  }
}