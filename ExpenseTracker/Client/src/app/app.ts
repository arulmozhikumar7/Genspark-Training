import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { inject } from '@angular/core';
import { BudgetNotificationService } from '@features/notification/services/budget-notification.service';

@Component({
  standalone: true,
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html'
})
export class App implements OnInit {
  constructor(private notificationService: BudgetNotificationService) {}

  ngOnInit(): void {
    this.notificationService.startConnection();
  }

}
