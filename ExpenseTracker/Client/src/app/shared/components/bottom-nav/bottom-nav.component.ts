import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { logout } from '@store/auth/auth.actions';

@Component({
  standalone: true,
  selector: 'app-bottom-nav',
  imports: [CommonModule, RouterModule],
  templateUrl: './bottom-nav.component.html',
})
export class BottomNavComponent {
  private store = inject(Store);

  onLogout() {
    this.store.dispatch(logout());
  }
}
