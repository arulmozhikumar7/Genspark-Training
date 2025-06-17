import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ChildAComponent } from './components/dashboard/child-a.component';
import { ChildBComponent } from './components/dashboard/child-b.component';
import { ChildDetailsComponent } from './components/dashboard/child-details.component';
import { AdminComponent } from './components/admin/admin.component';
import { authGuard } from './guards/auth.guard';
import { LoginComponent } from './components/login/login.component';
export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    children: [
      { path: 'child-a', component: ChildAComponent },
      { path: 'child-b', component: ChildBComponent },
      { path: 'details/:id', component: ChildDetailsComponent }
    ]
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [authGuard]
  }, { path: 'login', component: LoginComponent },
  { path: '**', redirectTo: '' }
];
