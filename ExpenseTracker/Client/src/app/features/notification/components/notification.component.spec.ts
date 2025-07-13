import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NotificationComponent } from './notification.component';
import { BudgetNotificationService } from '@features/notification/services/budget-notification.service';
import { of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

describe('NotificationComponent', () => {
  let component: NotificationComponent;
  let fixture: ComponentFixture<NotificationComponent>;
  let mockBudgetNotificationService: jasmine.SpyObj<BudgetNotificationService>;

  const mockAlerts = [
    { name: 'Alert 1', message: 'Message 1', categoryName: 'Category 1', receivedAt: Date.now() },
    { name: 'Alert 2', message: 'Message 2', categoryName: 'Category 2', receivedAt: Date.now() }
  ];

  beforeEach(async () => {
    mockBudgetNotificationService = jasmine.createSpyObj('BudgetNotificationService', ['alerts$']);
    Object.defineProperty(mockBudgetNotificationService, 'alerts$', {
      get: () => of(mockAlerts)
    });

    await TestBed.configureTestingModule({
      imports: [NotificationComponent, CommonModule],
      providers: [
        { provide: BudgetNotificationService, useValue: mockBudgetNotificationService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load alerts from the service on init', () => {
    expect(component.alerts.length).toBe(2);
    expect(component.alerts[0].name).toBe('Alert 1');
  });
});
