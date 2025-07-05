import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { PaymentFormComponent } from './payment-form';
import { ReactiveFormsModule } from '@angular/forms';
import { RazorpayService } from '../services/razorpay.service';

describe('PaymentFormComponent', () => {
  let component: PaymentFormComponent;
  let fixture: ComponentFixture<PaymentFormComponent>;
  let razorpaySpy: jasmine.SpyObj<RazorpayService>;

  const mockCourses = [
    { id: 'course1', name: 'Test Course 1', amount: 500 },
    { id: 'course2', name: 'Test Course 2', amount: 750 }
  ];

  beforeEach(waitForAsync(() => {
    razorpaySpy = jasmine.createSpyObj('RazorpayService', ['initiatePayment']);

    spyOn(window, 'fetch').and.returnValue(Promise.resolve({
      json: () => Promise.resolve(mockCourses),
    } as Response));

    TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, PaymentFormComponent],
      providers: [{ provide: RazorpayService, useValue: razorpaySpy }]
    }).compileComponents().then(() => {
      fixture = TestBed.createComponent(PaymentFormComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
    });
  }));

  it('should render the form', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('form')).toBeTruthy();
  });

  it('should validate required fields', () => {
    component.form.setValue({
      name: '',
      email: '',
      contact: '',
      courseId: ''
    });
    component.onSubmit();
    expect(component.message).toContain('fill out the form');
  });

  it('should validate invalid email format', () => {
    component.form.setValue({
      name: 'John',
      email: 'invalid-email',
      contact: '1234567890',
      courseId: 'course1'
    });
    component.onSubmit();
    expect(component.message).toContain('fill out the form');
  });

  it('should call RazorpayService if form is valid', waitForAsync(async () => {
    razorpaySpy.initiatePayment.and.returnValue(Promise.resolve('Payment Success'));

    await fixture.whenStable();
    component.courses = mockCourses;

    component.form.setValue({
      name: 'John',
      email: 'john@example.com',
      contact: '1234567890',
      courseId: 'course1'
    });

    component.onSubmit();

    expect(razorpaySpy.initiatePayment).toHaveBeenCalledWith(jasmine.objectContaining({
      name: 'John',
      email: 'john@example.com',
      contact: '1234567890',
      courseId: 'course1',
      amount: 500
    }));
  }));
});
