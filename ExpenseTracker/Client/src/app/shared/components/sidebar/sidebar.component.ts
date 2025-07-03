import { Component, inject, OnInit, Renderer2 } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { logout } from '@store/auth/auth.actions';
import { UserService } from '@core/services/user.service';
import { TokenService } from '@core/services/token.service'; 
@Component({
  standalone: true,
  selector: 'app-sidebar',
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent implements OnInit {
  private store = inject(Store);
  private userService = inject(UserService);
  private tokenService = inject(TokenService); // ✅
  private renderer = inject(Renderer2);

  userName: any = 'Guest';
  isDarkMode = false;
  isAdmin = false; // ✅

  ngOnInit(): void {
    this.userName = this.userService.getUsername();
    this.isDarkMode = document.body.classList.contains('dark');

    const user = this.tokenService.getUserFromToken(); // ✅
    this.isAdmin = user?.role === 'Admin'; // ✅
  }

  onLogout() {
    this.store.dispatch(logout());
  }

  toggleDarkMode(): void {
    this.isDarkMode = !this.isDarkMode;

    if (this.isDarkMode) {
      this.renderer.addClass(document.body, 'dark');
    } else {
      this.renderer.removeClass(document.body, 'dark');
    }
  }
}
