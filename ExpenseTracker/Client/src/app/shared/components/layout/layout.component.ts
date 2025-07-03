import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from '@shared/components/sidebar/sidebar.component'; 
import { BottomNavComponent } from '../bottom-nav/bottom-nav.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, SidebarComponent,BottomNavComponent],
  template: `
<div class="min-h-screen flex flex-col lg:flex-row">
  <!-- Fixed Sidebar for desktop -->
  <app-sidebar class="hidden lg:block fixed top-0 left-0 h-screen w-64 z-50"></app-sidebar>

  <!-- Main Content Area -->
  <div class="flex-1 lg:ml-64 overflow-y-auto text-[var(--text-primary)] lg:pb-0">
    <router-outlet></router-outlet>
  </div>
</div>

<app-bottom-nav></app-bottom-nav>
  `,
})
export class LayoutComponent {}
