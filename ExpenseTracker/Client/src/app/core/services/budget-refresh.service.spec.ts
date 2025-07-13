import { TestBed } from '@angular/core/testing';
import { BudgetRefreshService } from './budget-refresh.service';

describe('BudgetRefreshService', () => {
  let service: BudgetRefreshService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BudgetRefreshService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should emit when trigger() is called', (done) => {
    service.refresh$.subscribe(() => {
      expect(true).toBeTrue();
      done();
    });

    service.trigger();
  });
});
