import { TestBed } from '@angular/core/testing';
import { ExpenseRefreshService } from './expense-refresh.service';

describe('ExpenseRefreshService', () => {
  let service: ExpenseRefreshService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExpenseRefreshService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should emit on trigger()', (done) => {
    service.refresh$.subscribe(() => {
      expect(true).toBeTrue();
      done();
    });

    service.trigger();
  });
});
