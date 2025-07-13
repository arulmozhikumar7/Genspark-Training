import { TestBed } from '@angular/core/testing';
import { NotificationService } from './notification.service';
import { ToastrService } from 'ngx-toastr';

class MockToastrService {
  success = jasmine.createSpy('success');
  error = jasmine.createSpy('error');
  info = jasmine.createSpy('info');
  warning = jasmine.createSpy('warning');
}

describe('NotificationService', () => {
  let service: NotificationService;
  let toastr: MockToastrService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        NotificationService,
        { provide: ToastrService, useClass: MockToastrService }
      ]
    });

    service = TestBed.inject(NotificationService);
    toastr = TestBed.inject(ToastrService) as unknown as MockToastrService;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call toastr.success with default title', () => {
    service.success('Operation completed');
    expect(toastr.success).toHaveBeenCalledWith('Operation completed', 'Success', undefined);
  });

  it('should call toastr.error with custom title', () => {
    service.error('Something went wrong', 'Oops!');
    expect(toastr.error).toHaveBeenCalledWith('Something went wrong', 'Oops!', undefined);
  });

  it('should call toastr.info with custom options', () => {
    const options = { timeOut: 5000 };
    service.info('Informational message', 'Heads Up', options);
    expect(toastr.info).toHaveBeenCalledWith('Informational message', 'Heads Up', options);
  });

  it('should call toastr.warning with default title', () => {
    service.warn('Low disk space');
    expect(toastr.warning).toHaveBeenCalledWith('Low disk space', 'Warning', undefined);
  });
});
