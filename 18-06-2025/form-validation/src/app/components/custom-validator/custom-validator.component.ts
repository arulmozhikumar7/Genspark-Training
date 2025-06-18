import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../../validators/custom-validators';

@Component({
  selector: 'app-custom-validator',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './custom-validator.html',
  styleUrl: './custom-validator.css'

})
export class CustomValidatorComponent implements OnInit {
  customForm!: FormGroup;
  customSubmitted = false;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.customForm = this.fb.group({
      username: ['', [Validators.required, CustomValidators.usernameValidator()]],
      age: ['', [Validators.required, CustomValidators.ageRange(13, 65)]],
      password: ['', [Validators.required, CustomValidators.strongPassword()]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validators: [CustomValidators.passwordMatch('password', 'confirmPassword')]
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.customForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }

  onSubmit() {
    if (this.customForm.valid) {
      this.customSubmitted = true;
      console.log('Custom Form:', this.customForm.value);
    }
  }

  getSubmittedData() {
    const { confirmPassword, ...data } = this.customForm.value;
    return data;
  }
}
