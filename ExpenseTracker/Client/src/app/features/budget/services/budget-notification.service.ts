import { inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { TokenService } from '@core/services/token.service';
@Injectable({
  providedIn: 'root',
})
export class BudgetNotificationService {
  private hubConnection!: signalR.HubConnection;
  private alertsSubject = new BehaviorSubject<any[]>([]);
  public alerts$ = this.alertsSubject.asObservable();
  public tokenservice = inject(TokenService);

  private readonly BASE_URL = 'http://localhost:5169';

  constructor() {
    console.log("🔔 BudgetNotificationService initialized");
  }

  public startConnection(): void {
    const jwtToken = this.tokenservice.getAccessToken();

    if (!jwtToken) {
      console.warn("⚠️ No JWT token found for SignalR");
      return;
    }

    const decoded = this.parseJwt(jwtToken);
    const userId = decoded?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

    if (!userId) {
      console.warn("⚠️ User ID not found in JWT");
      return;
    }

    console.log("📡 Attempting to start SignalR connection...");

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.BASE_URL}/budgetHub`, {
        accessTokenFactory: () => jwtToken,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information) // Add for debug
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('✅ SignalR connected');
        return this.hubConnection.invoke('JoinUserGroup', userId);
      })
      .then(() => {
        console.log('👥 Joined user group for:', userId);
      })
      .catch((err: any) => console.error('❌ SignalR connection error:', err));

    this.hubConnection.on('BudgetAlert', (alert: any) => {
      const current = this.alertsSubject.getValue();
      this.alertsSubject.next([alert, ...current]);
      console.log("📢 Received BudgetAlert:", alert);
    });
  }

  private parseJwt(token: string) {
    try {
      return JSON.parse(atob(token.split('.')[1]));
    } catch {
      return null;
    }
  }
}
