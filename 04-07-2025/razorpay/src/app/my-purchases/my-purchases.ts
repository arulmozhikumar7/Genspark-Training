import { Component } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-my-purchases',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DatePipe
  ],
  templateUrl: './my-purchases.html',
  styleUrls: ['./my-purchases.css']
})
export class MyPurchasesComponent {
  form: FormGroup;
  purchases: any[] = [];
  allCourses: any[] = [];

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });

    fetch('courses.json')
      .then(res => res.json())
      .then(data => (this.allCourses = data));
  }

  onSubmit() {
    const email = this.form.value.email;
    const allPurchases = JSON.parse(localStorage.getItem('purchases') || '[]');
    this.purchases = allPurchases
      .filter((p: any) => p.email === email)
      .map((p: any) => {
        const course = this.allCourses.find((c) => c.id == p.courseId);
        return {
          ...p,
          courseName: course?.name,
          amount: course?.amount,
        };
      });
  }
}
