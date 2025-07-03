import { Injectable, inject } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { TokenService } from '@core/services/token.service';
import { NotificationService } from '@core/services/notification.service';
@Injectable({ providedIn: 'root' })
export class BudgetNotificationService {
  private hubConnection!: signalR.HubConnection;
  private alertsSubject = new BehaviorSubject<any[]>([]);
  public alerts$ = this.alertsSubject.asObservable();
  public tokenservice = inject(TokenService);
  private readonly BASE_URL = 'http://localhost:5169';
  private toast = inject(NotificationService);
  constructor() {
    this.loadFromLocalStorage();
  }

  public startConnection(): void {
    const jwtToken = this.tokenservice.getAccessToken();
    if (!jwtToken) return;

    const decoded = this.parseJwt(jwtToken);
    const userId = decoded?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    if (!userId) return;

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.BASE_URL}/budgetHub`, {
        accessTokenFactory: () => jwtToken,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => this.hubConnection.invoke('JoinUserGroup', userId))
      .catch((err: any) => console.error('SignalR error:', err));

    this.hubConnection.on('BudgetAlert', (alert: any) => {
      const newAlert = { ...alert, receivedAt: Date.now() };
      const current = this.alertsSubject.getValue();

      const updated = [newAlert, ...current]
        .filter((a) => !this.isExpired(a))
        .slice(0, 10);

      this.alertsSubject.next(updated);
      this.updateLocalStorage(updated);

      this.toast.warn(
  `⚠️ ${alert.categoryName} – ${alert.name}: ${alert.message}`,
  'Budget Alert',
  { timeOut: 7000 } 
);

    });
  }

  private loadFromLocalStorage(): void {
    const raw = localStorage.getItem('budget_alerts');
    const stored = raw ? JSON.parse(raw) : [];
    const valid = stored.filter((a: any) => !this.isExpired(a));
    this.alertsSubject.next(valid);
  }

  private updateLocalStorage(alerts: any[]) {
    localStorage.setItem('budget_alerts', JSON.stringify(alerts));
  }

  private isExpired(alert: any): boolean {
    const now = Date.now();
    const sevenDays = 7 * 24 * 60 * 60 * 1000;
    return now - alert.receivedAt > sevenDays;
  }

  private parseJwt(token: string) {
    try {
      return JSON.parse(atob(token.split('.')[1]));
    } catch {
      return null;
    }
  }
}
