import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LayoutComponent } from './layout.component';
import { RouterTestingModule } from '@angular/router/testing';
import { provideMockStore } from '@ngrx/store/testing';

describe('LayoutComponent', () => {
  let component: LayoutComponent;
  let fixture: ComponentFixture<LayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LayoutComponent, RouterTestingModule],
      providers: [
        provideMockStore({})  
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(LayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the layout component', () => {
    expect(component).toBeTruthy();
  });

  it('should render sidebar component', () => {
    const sidebarEl = fixture.nativeElement.querySelector('app-sidebar');
    expect(sidebarEl).toBeTruthy();
  });

  it('should render bottom nav component', () => {
    const bottomNavEl = fixture.nativeElement.querySelector('app-bottom-nav');
    expect(bottomNavEl).toBeTruthy();
  });
});
