import { Injectable } from '@angular/core';

declare var Razorpay: any;

interface PaymentData {
  courseId: string;
  amount: number;
  name: string;
  email: string;
  contact: string;
}

@Injectable({
  providedIn: 'root',
})
export class RazorpayService {
  initiatePayment(data: PaymentData): Promise<string> {
    return new Promise((resolve, reject) => {
      if (typeof Razorpay === 'undefined') {
        return reject('Razorpay script not loaded. Make sure it is included in index.html');
      }

      if (this.hasAlreadyPurchased(data.email, data.courseId)) {
        return reject('You have already purchased this course.');
      }

      const options = {
        key: 'rzp_test_1RKKP5rJBqKpT4',
        amount: data.amount * 100,
        currency: 'INR',
        name: 'Course Purchase',
        description: 'Test Transaction',
        image: 'https://yourcompany.com/logo.png',
        prefill: {
          name: data.name,
          email: data.email,
          contact: data.contact,
        },
        theme: {
          color: '#3399cc',
        },
        handler: (response: any) => {
          this.savePurchase({
            courseId: data.courseId,
            email: data.email,
            amount: data.amount,
            paymentId: response.razorpay_payment_id,
            timestamp: new Date().toISOString(),
          });
          resolve(`Payment Successful! ID: ${response.razorpay_payment_id}`);
        },
        modal: {
          ondismiss: () => reject('Payment Cancelled'),
        },
      };

      const rzp = new Razorpay(options);
      rzp.open();
    });
  }

  private hasAlreadyPurchased(email: string, courseId: string): boolean {
    const purchases = this.getPurchases();
    return purchases.some((p) => p.email === email && p.courseId == courseId);
  }

  private savePurchase(purchase: {
    courseId: string;
    email: string;
    amount: number;
    paymentId: string;
    timestamp: string;
  }): void {
    const purchases = this.getPurchases();
    purchases.push(purchase);
    localStorage.setItem('purchases', JSON.stringify(purchases));
  }

  private getPurchases(): any[] {
    return JSON.parse(localStorage.getItem('purchases') || '[]');
  }
}
