import { TestBed } from '@angular/core/testing';
import { BudgetNotificationService } from './budget-notification.service';
import { TokenService } from '@core/services/token.service';
import { NotificationService } from '@core/services/notification.service';
import * as signalR from '@microsoft/signalr';

describe('BudgetNotificationService', () => {
  let service: BudgetNotificationService;
  let mockTokenService: jasmine.SpyObj<TokenService>;
  let mockNotificationService: jasmine.SpyObj<NotificationService>;
  let mockHubConnection: jasmine.SpyObj<signalR.HubConnection>;

  const fakeToken = [
    btoa(JSON.stringify({ alg: 'HS256', typ: 'JWT' })),
    btoa(JSON.stringify({
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier': '123'
    })),
    'signature'
  ].join('.');

  beforeEach(() => {
    mockTokenService = jasmine.createSpyObj('TokenService', ['getAccessToken']);
    mockNotificationService = jasmine.createSpyObj('NotificationService', ['warn']);
    mockHubConnection = jasmine.createSpyObj('HubConnection', ['start', 'invoke', 'on']);

    mockHubConnection.start.and.returnValue(Promise.resolve());
    mockHubConnection.invoke.and.returnValue(Promise.resolve());

    // Intercept HubConnectionBuilder.prototype.build to return mock connection
    spyOn(signalR.HubConnectionBuilder.prototype, 'build').and.returnValue(mockHubConnection);

    TestBed.configureTestingModule({
      providers: [
        BudgetNotificationService,
        { provide: TokenService, useValue: mockTokenService },
        { provide: NotificationService, useValue: mockNotificationService },
      ]
    });

    service = TestBed.inject(BudgetNotificationService);
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should not start connection if token is missing', () => {
    mockTokenService.getAccessToken.and.returnValue(null);
    service.startConnection();
    expect(mockHubConnection.start).not.toHaveBeenCalled();
  });

  it('should start connection and join user group if token is valid', async () => {
    mockTokenService.getAccessToken.and.returnValue(fakeToken);
    service.startConnection();
    await Promise.resolve();
    expect(mockHubConnection.start).toHaveBeenCalled();
    expect(mockHubConnection.invoke).toHaveBeenCalledWith('JoinUserGroup', '123');
  });

  it('should handle BudgetAlert event and update alerts', async () => {
    const alert = { categoryName: 'TestCat', name: 'Alert1', message: 'Test alert' };

    mockHubConnection.on.and.callFake((event: string, callback: (data: any) => void) => {
      if (event === 'BudgetAlert') {
        callback(alert);
      }
    });

    mockTokenService.getAccessToken.and.returnValue(fakeToken);
    service.startConnection();
    await Promise.resolve();

    service.alerts$.subscribe(alerts => {
      expect(alerts.length).toBe(1);
      expect(alerts[0].name).toBe('Alert1');
    });

    expect(mockNotificationService.warn).toHaveBeenCalledWith(
      `⚠️ ${alert.categoryName} – ${alert.name}: ${alert.message}`,
      'Budget Alert',
      { timeOut: 7000 }
    );
  });

 
});
