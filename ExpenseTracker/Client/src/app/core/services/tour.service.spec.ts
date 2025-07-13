import { TestBed } from '@angular/core/testing';
import { TourService } from './tour.service';
import { HttpService } from './http.service';
import { of, throwError } from 'rxjs';

describe('TourService', () => {
  let service: TourService;
  let httpSpy: jasmine.SpyObj<HttpService>;
  const STORAGE_KEY = 'tour_progress';

  beforeEach(() => {
    const spy = jasmine.createSpyObj('HttpService', ['get', 'post']);

    TestBed.configureTestingModule({
      providers: [
        TourService,
        { provide: HttpService, useValue: spy }
      ]
    });

    service = TestBed.inject(TourService);
    httpSpy = TestBed.inject(HttpService) as jasmine.SpyObj<HttpService>;

    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('loadProgressFromApi', () => {
    it('should store progress in localStorage and emit true on success', (done) => {
      const mockProgress = { data: { intro: true } };
      httpSpy.get.and.returnValue(of(mockProgress));

      service.progressReady$.subscribe((ready) => {
        if (ready) {
          const stored = JSON.parse(localStorage.getItem(STORAGE_KEY)!);
          expect(stored).toEqual(mockProgress.data);
          expect(httpSpy.get).toHaveBeenCalledWith('/TourProgress');
          done();
        }
      });

      service.loadProgressFromApi().subscribe();
    });

    it('should emit true and return null on error', (done) => {
      httpSpy.get.and.returnValue(throwError(() => new Error('Network error')));

      service.progressReady$.subscribe((ready) => {
        if (ready) {
          expect(localStorage.getItem(STORAGE_KEY)).toBeNull();
          done();
        }
      });

      service.loadProgressFromApi().subscribe((result) => {
        expect(result).toBeNull();
      });
    });
  });

  describe('isCompleted', () => {
    it('should return true if tourKey is completed', () => {
      localStorage.setItem(STORAGE_KEY, JSON.stringify({ intro: true }));
      expect(service.isCompleted('intro')).toBeTrue();
    });

    it('should return false if tourKey is not completed', () => {
      localStorage.setItem(STORAGE_KEY, JSON.stringify({ intro: false }));
      expect(service.isCompleted('intro')).toBeFalse();
    });

    it('should return false if no progress stored', () => {
      expect(service.isCompleted('any')).toBeFalse();
    });
  });

  describe('markCompleted', () => {
    it('should mark tourKey as completed and call HTTP post', () => {
      httpSpy.post.and.returnValue(of({}));
      service.markCompleted('welcome');

      const stored = JSON.parse(localStorage.getItem(STORAGE_KEY)!);
      expect(stored['welcome']).toBeTrue();
      expect(httpSpy.post).toHaveBeenCalledWith('/TourProgress/complete/welcome', {});
    });

    it('should preserve existing progress and add new tourKey', () => {
      localStorage.setItem(STORAGE_KEY, JSON.stringify({ intro: true }));
      httpSpy.post.and.returnValue(of({}));
      service.markCompleted('step2');

      const stored = JSON.parse(localStorage.getItem(STORAGE_KEY)!);
      expect(stored['intro']).toBeTrue();
      expect(stored['step2']).toBeTrue();
    });
  });

  describe('clearProgress', () => {
    it('should clear localStorage and emit false', (done) => {
      localStorage.setItem(STORAGE_KEY, JSON.stringify({ intro: true }));

      const emitted: boolean[] = [];
      service.progressReady$.subscribe((val) => emitted.push(val));

      service.clearProgress();

      expect(localStorage.getItem(STORAGE_KEY)).toBeNull();
      // First: false after clearing
      expect(emitted.includes(false)).toBeTrue();
      done();
    });
  });
});
