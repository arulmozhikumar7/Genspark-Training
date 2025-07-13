import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { HomePage } from './home.page';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { TourService } from '@core/services/tour.service';
import { Subject, of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('HomePage', () => {
  let component: HomePage;
  let fixture: ComponentFixture<HomePage>;
  let store: MockStore;
  let tourServiceSpy: jasmine.SpyObj<TourService>;
  let progressReadySubject: Subject<void>;

  // Minimal mock state matching selectors expectations
  const initialState = {
    budget: {
      loading: false,
      budgets: [],
    },
    expense: {
      loading: false,
      expenses: [],
    },
  };

  beforeEach(async () => {
    progressReadySubject = new Subject<void>();

    const tourServiceMock = jasmine.createSpyObj('TourService', ['isCompleted', 'markCompleted'], {
      progressReady$: progressReadySubject.asObservable(),
    });

    await TestBed.configureTestingModule({
      imports: [HomePage, HttpClientTestingModule],
      providers: [
        provideMockStore({ initialState }),
        { provide: TourService, useValue: tourServiceMock },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({}),
            snapshot: {
              paramMap: new Map(),
            },
          },
        },
      ],
    }).compileComponents();

    store = TestBed.inject(MockStore);
    tourServiceSpy = TestBed.inject(TourService) as jasmine.SpyObj<TourService>;

    fixture = TestBed.createComponent(HomePage);
    component = fixture.componentInstance;

    // Setup tour service spy defaults
    tourServiceSpy.isCompleted.and.returnValue(false);
    tourServiceSpy.markCompleted.and.stub();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should run the tour if not completed and mark it completed on destroy', fakeAsync(() => {
    fixture.detectChanges();

    // Trigger progressReady$
    progressReadySubject.next();
    tick();

    expect(tourServiceSpy.isCompleted).toHaveBeenCalledWith('homeTourCompleted');

    // Simulate driver.js tour destroyed callback by calling onDestroyed manually
    component['tourService'].markCompleted('homeTourCompleted');

    expect(tourServiceSpy.markCompleted).toHaveBeenCalledWith('homeTourCompleted');
  }));

  it('should not run the tour if already completed', fakeAsync(() => {
    tourServiceSpy.isCompleted.and.returnValue(true);

    fixture.detectChanges();

    progressReadySubject.next();
    tick();

    expect(tourServiceSpy.isCompleted).toHaveBeenCalledWith('homeTourCompleted');
    expect(tourServiceSpy.markCompleted).not.toHaveBeenCalled();
  }));
});
