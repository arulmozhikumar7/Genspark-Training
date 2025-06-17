import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-child-details',
  imports: [CommonModule],
  template: `
    <h3>Child Details</h3>
    <p>Showing details for ID: {{ id }}</p>
  `
})
export class ChildDetailsComponent {
  id: string | null = null;

  constructor(private route: ActivatedRoute) {
    this.id = this.route.snapshot.paramMap.get('id');
  }
}
