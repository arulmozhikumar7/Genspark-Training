import {
  Component,
  ElementRef,
  ViewChild,
  AfterViewInit,
  OnDestroy,
  Input,
  OnChanges,
  SimpleChanges,
  inject,
} from '@angular/core';
import { Chart, ChartConfiguration, registerables } from 'chart.js';
import { ExpenseApiService } from '@features/expense/services/expense-api.service';

Chart.register(...registerables);
// expense-pie-chart.component.ts
@Component({
  selector: 'app-expense-pie-chart',
  standalone: true,
  template: `
    <div class="text-center text-sm font-semibold text-gray-700 dark:text-white mb-2">
      Total Expense: ₹{{ totalAmount }}
    </div>
    <canvas #pieChart class="max-w-xs mx-auto"></canvas>
  `,
})
export class ExpensePieChartComponent implements AfterViewInit, OnDestroy, OnChanges {
  @ViewChild('pieChart') chartRef!: ElementRef<HTMLCanvasElement>;
  @Input() summaryData: { categoryName: string; totalAmount: number }[] = [];

  chart!: Chart;
  totalAmount = 0;

  ngAfterViewInit(): void {
    this.renderChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['summaryData'] && !changes['summaryData'].firstChange) {
      this.renderChart();
    }
  }

  private renderChart() {
    if (!this.chartRef) return;

    if (this.chart) this.chart.destroy();

    const labels = this.summaryData.map((item) => item.categoryName);
    const values = this.summaryData.map((item) => item.totalAmount);
    this.totalAmount = values.reduce((sum, val) => sum + val, 0);

    this.chart = new Chart(this.chartRef.nativeElement, {
      type: 'pie',
      data: {
        labels,
        datasets: [
          {
            label: 'Expenses by Category',
            data: values,
            backgroundColor: [
              '#3B82F6', '#F59E0B', '#10B981', '#EF4444',
              '#A855F7', '#6366F1', '#F472B6', '#FACC15'
            ],
          },
        ],
      },
      options: {
        responsive: true,
        plugins: {
          legend: { position: 'bottom' },
        },
      },
    });
  }

  ngOnDestroy(): void {
    this.chart?.destroy();
  }
}
