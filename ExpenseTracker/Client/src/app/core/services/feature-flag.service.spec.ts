import { TestBed } from '@angular/core/testing';
import { FeatureFlagService, FeatureFlags } from './feature-flag.service';
import { HttpService } from '@core/services/http.service';
import { of } from 'rxjs';

describe('FeatureFlagService', () => {
  let service: FeatureFlagService;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;

  const mockFlags: FeatureFlags = {
    enableCsvExport: true
  };

  beforeEach(() => {
    // ✅ Set up spy with return value BEFORE service creation
    const spy = jasmine.createSpyObj('HttpService', ['get']);
    spy.get.and.returnValue(of(mockFlags));

    TestBed.configureTestingModule({
      providers: [
        { provide: HttpService, useValue: spy },
        FeatureFlagService
      ]
    });

    httpServiceSpy = TestBed.inject(HttpService) as jasmine.SpyObj<HttpService>;
    service = TestBed.inject(FeatureFlagService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch and return feature flags', (done) => {
    service.getFlags().subscribe(flags => {
      expect(flags).toEqual(mockFlags);
      expect(httpServiceSpy.get).toHaveBeenCalledWith('/FeatureFlags');
      done();
    });
  });

  it('should cache the flags and call HTTP only once', (done) => {
    service.getFlags().subscribe(() => {
      service.getFlags().subscribe(() => {
        expect(httpServiceSpy.get).toHaveBeenCalledTimes(1);
        done();
      });
    });
  });
});
