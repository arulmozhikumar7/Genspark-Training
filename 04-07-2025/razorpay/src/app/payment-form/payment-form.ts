import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RazorpayService } from '../services/razorpay.service';

interface Course {
  id: string;
  name: string;
  amount: number;
}

@Component({
  selector: 'app-payment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './payment-form.html',
  styleUrls: ['./payment-form.css'],
})
export class PaymentFormComponent {
  form: FormGroup;
  courses: Course[] = [];
  message: string | null = null;

  private fb = inject(FormBuilder);
  private razorpayService = inject(RazorpayService);

  constructor() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contact: ['', Validators.required],
      courseId: ['', Validators.required],
    });

    this.loadCourses();
  }

  private loadCourses() {
    fetch('courses.json')
      .then((res) => res.json())
      .then((data: Course[]) => {
        this.courses = data;
      })
      .catch(() => {
        this.message = 'Failed to load courses.';
      });
  }

  trackByCourseId(index: number, course: Course): string {
  return course.id;
  }

  onSubmit() {
    if (this.form.invalid) {
      this.message = 'Please fill out the form correctly.';
      return;
    }

    const { name, email, contact, courseId } = this.form.value;
    const selectedCourse = this.courses.find((c) => c.id == courseId);

    if (!selectedCourse) {
      this.message = 'Selected course not found.';
      return;
    }

    this.razorpayService
      .initiatePayment({
        courseId,
        amount: selectedCourse.amount,
        name,
        email,
        contact,
      })
      .then((msg: string) => {
        this.message = msg;
        this.form.reset();
      })
      .catch((err: any) => {
        this.message = `Payment failed or cancelled: ${err}`;
      });
  }
}
