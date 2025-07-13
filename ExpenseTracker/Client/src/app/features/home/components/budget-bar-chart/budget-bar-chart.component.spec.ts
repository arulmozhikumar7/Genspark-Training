import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BudgetBarChartComponent } from './budget-bar-chart.component';
import { provideMockStore } from '@ngrx/store/testing';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';
import { Chart } from 'chart.js/auto';
import { selectAllBudgets } from '@store/budget/budget.selectors';

// Mock canvas context
function createMockCanvas(): HTMLCanvasElement {
  const canvas = document.createElement('canvas');
  canvas.getContext = () => ({
    fillRect: () => {},
    clearRect: () => {},
    getImageData: () => ({ data: [] }),
    putImageData: () => {},
    createImageData: () => [],
    setTransform: () => {},
    drawImage: () => {},
    save: () => {},
    fillText: () => {},
    restore: () => {},
    beginPath: () => {},
    moveTo: () => {},
    lineTo: () => {},
    closePath: () => {},
    stroke: () => {},
    translate: () => {},
    scale: () => {},
    rotate: () => {},
    arc: () => {},
    fill: () => {},
    measureText: () => ({ width: 0 }),
    transform: () => {},
    rect: () => {},
    clip: () => {},
  } as any);
  return canvas;
}

// Mock budget data
const mockBudgets = [
  {
    name: 'Groceries',
    startDate: new Date('2025-07-01'),
    endDate: new Date('2025-07-31'),
    balanceAmount: 3000,
    limitAmount: 5000,
  },
  {
    name: 'Transport',
    startDate: new Date('2025-07-01'),
    endDate: new Date('2025-07-31'),
    balanceAmount: 1000,
    limitAmount: 2000,
  },
];

describe('BudgetBarChartComponent', () => {
  let component: BudgetBarChartComponent;
  let fixture: ComponentFixture<BudgetBarChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BudgetBarChartComponent],
      providers: [
        provideMockStore({
          selectors: [
            {
              selector: selectAllBudgets,
              value: mockBudgets,
            },
          ],
        }),
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BudgetBarChartComponent);
    component = fixture.componentInstance;

    // Mock the canvas element in the template
    const mockCanvas = createMockCanvas();
    (component as any).canvasRef = {
      nativeElement: mockCanvas,
    };

    fixture.detectChanges();
  });

  afterEach(() => {
    if (component['chart']) {
      component['chart'].destroy();
    }
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should render chart with budget data', () => {
    component.renderChart();
    expect(component['chart']).toBeTruthy();
    const chartData = component['chart']?.data;
    expect(chartData?.labels?.length).toBeGreaterThan(0);
    expect(chartData?.datasets?.length).toBe(2);
    expect(chartData?.datasets?.[0].data).toContain(2000); // spent for Groceries
    expect(chartData?.datasets?.[1].data).toContain(5000); // actual for Groceries
  });

  it('should destroy chart on component destroy', () => {
    const destroySpy = spyOn(component['chart']!, 'destroy').and.callThrough();
    component.ngOnDestroy();
    expect(destroySpy).toHaveBeenCalled();
  });

  it('should toggle fullscreen state and resize chart', (done) => {
    const resizeSpy = spyOn(component['chart']!, 'resize');
    component.toggleFullscreen();
    expect(component.isFullscreen).toBeTrue();
    setTimeout(() => {
      expect(resizeSpy).toHaveBeenCalled();
      done();
    }, 310); // waits slightly more than 300ms to allow timeout to trigger
  });
});
