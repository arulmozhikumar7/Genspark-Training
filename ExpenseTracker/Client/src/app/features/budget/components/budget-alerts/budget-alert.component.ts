import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BudgetNotificationService } from '@features/budget/services/budget-notification.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-budget-alert',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './budget-alert.component.html',
})
export class BudgetAlertComponent implements OnInit {
  alerts$!: Observable<any[]>;

  constructor(private notificationService: BudgetNotificationService) {}

  ngOnInit(): void {
    this.notificationService.startConnection(); // optional if not started globally
    this.alerts$ = this.notificationService.alerts$;
  }
}
