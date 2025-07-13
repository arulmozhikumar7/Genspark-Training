import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BottomNavComponent } from './bottom-nav.component';
import { RouterTestingModule } from '@angular/router/testing';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { logout } from '@store/auth/auth.actions';

describe('BottomNavComponent', () => {
  let component: BottomNavComponent;
  let fixture: ComponentFixture<BottomNavComponent>;
  let store: MockStore;
  let dispatchSpy: jasmine.Spy;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BottomNavComponent, RouterTestingModule],
      providers: [provideMockStore({})],
    }).compileComponents();

    fixture = TestBed.createComponent(BottomNavComponent);
    component = fixture.componentInstance;
    store = TestBed.inject(MockStore);
    dispatchSpy = spyOn(store, 'dispatch');
    fixture.detectChanges();
  });

  it('should create the bottom nav component', () => {
    expect(component).toBeTruthy();
  });

  it('should dispatch logout action on onLogout', () => {
    component.onLogout();
    expect(dispatchSpy).toHaveBeenCalledWith(logout());
  });
});
