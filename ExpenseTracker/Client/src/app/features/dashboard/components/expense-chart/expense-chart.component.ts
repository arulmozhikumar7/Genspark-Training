// expense-chart.component.ts
import {
  Component,
  ElementRef,
  ViewChild,
  AfterViewInit,
  OnDestroy,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { Chart, ChartConfiguration, registerables } from 'chart.js';
import { Expense } from '@features/expense/models/expense.model';

Chart.register(...registerables);

@Component({
  selector: 'app-expense-chart',
  standalone: true,
  template: `
    <div class="text-sm text-center font-semibold mb-2">Total Expense: ₹{{ totalAmount  }}</div>
    <canvas #expenseChart class="w-full h-64"></canvas>
  `,
  imports: [],
})
export class ExpenseChartComponent implements AfterViewInit, OnDestroy, OnChanges {
  @ViewChild('expenseChart') chartRef!: ElementRef<HTMLCanvasElement>;
  @Input() expenses: Expense[] = [];

  private chart!: Chart;
  totalAmount: number = 0;

  ngAfterViewInit(): void {
    this.renderChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['expenses'] && !changes['expenses'].firstChange) {
      this.updateChart();
    }
  }

  private renderChart() {
    const data = this.aggregateDataByCategory();

    const config: ChartConfiguration = {
      type: 'pie',
      data: {
        labels: data.labels,
        datasets: [
          {
            label: 'Expenses',
            data: data.totals,
            backgroundColor: [
              '#60A5FA',
              '#F87171',
              '#34D399',
              '#FBBF24',
              '#A78BFA',
              '#F472B6',
              '#FCD34D',
              '#6EE7B7',
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
    };

    this.chart = new Chart(this.chartRef.nativeElement, config);
  }

  private updateChart() {
    if (!this.chart) return;
    const data = this.aggregateDataByCategory();
    this.chart.data.labels = data.labels;
    this.chart.data.datasets[0].data = data.totals;
    this.chart.update();
  }

  private aggregateDataByCategory(): { labels: string[]; totals: number[] } {
    const grouped = new Map<string, number>();
    let total = 0;

    for (const expense of this.expenses) {
      const category = expense.categoryName ?? 'Uncategorized';
      grouped.set(category, (grouped.get(category) || 0) + expense.amount);
      total += expense.amount;
    }

    this.totalAmount = total;

    return {
      labels: Array.from(grouped.keys()),
      totals: Array.from(grouped.values()),
    };
  }

  ngOnDestroy(): void {
    this.chart?.destroy();
  }
}
