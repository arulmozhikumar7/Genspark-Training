import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
@Component({
    standalone: true,
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'] ,
  imports: [RouterModule], 
})
export class DashboardComponent {}
