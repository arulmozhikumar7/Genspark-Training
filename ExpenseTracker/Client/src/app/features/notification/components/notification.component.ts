import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { BudgetNotificationService } from '@features/notification/services/budget-notification.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-notification',
  templateUrl: './notification.component.html',
})
export class NotificationComponent implements OnInit {
  alerts: any[] = [];

  constructor(private notificationService: BudgetNotificationService) {}

  ngOnInit(): void {
    this.notificationService.alerts$.subscribe((alerts) => {
      this.alerts = alerts;
    });
  }
}
