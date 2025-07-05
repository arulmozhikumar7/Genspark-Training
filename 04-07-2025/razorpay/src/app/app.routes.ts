import { Routes } from '@angular/router';
import { PaymentFormComponent } from './payment-form/payment-form';
import { MyPurchasesComponent } from './my-purchases/my-purchases';
export const routes: Routes = [
  { path: 'purchase', component: PaymentFormComponent },
  { path: 'my-purchases', component: MyPurchasesComponent },
  { path: '', redirectTo: 'purchase', pathMatch: 'full' }
];

