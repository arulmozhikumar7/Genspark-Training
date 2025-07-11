import {
  Component,
  ElementRef,
  ViewChild,
  ChangeDetectorRef,
} from '@angular/core';
import Chart from 'chart.js/auto';
import { ComparisonApiService } from '@features/expense/services/comparison-api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { firstValueFrom } from 'rxjs';

interface CategoryComparison {
  categoryName: string;
  month1Amount: number;
  month2Amount: number;
}

interface MonthlyExpenseComparisonSummaryDto {
  categoryComparisons: CategoryComparison[];
  totalMonth1: number;
  totalMonth2: number;
  totalDifference: number;
  totalPercentageChange: number;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent {
  @ViewChild('comparisonChart') chartRef!: ElementRef<HTMLCanvasElement>;
  chart!: Chart;

  year1 = new Date().getFullYear();
  month1 = new Date().getMonth() + 1;
  year2 = new Date().getFullYear();
  month2 = new Date().getMonth();

  totalMonth1: number = 0;
  totalMonth2: number = 0;
  totalDifference: number = 0;
  totalPercentageChange: number = 0;

  isLoading = false;
  error = '';
  hasData = false;

  monthNames = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December',
  ];

  years = Array.from({ length: 26 }, (_, i) => 2025 + i);
  months = this.monthNames.map((name, i) => ({
    name,
    value: i + 1,
  }));

  constructor(
    private comparisonApi: ComparisonApiService,
    private cdr: ChangeDetectorRef
  ) {}

  async compare() {
    if (this.year1 === this.year2 && this.month1 === this.month2) {
      this.error = 'Please select two different months.';
      return;
    }

    this.isLoading = true;
    this.error = '';
    this.hasData = false;

    try {
      const res = await firstValueFrom(
        this.comparisonApi.compareTwoMonths(this.year1, this.month1, this.year2, this.month2)
      );

      const data: MonthlyExpenseComparisonSummaryDto = res.data;
      const comparisons = data.categoryComparisons ?? [];

      const labels = comparisons.map(c => c.categoryName);
      const data1 = comparisons.map(c => c.month1Amount || 0);
      const data2 = comparisons.map(c => c.month2Amount || 0);

      this.totalMonth1 = data.totalMonth1;
      this.totalMonth2 = data.totalMonth2;
      this.totalDifference = data.totalDifference;
      this.totalPercentageChange = data.totalPercentageChange;

      this.hasData = true;
      this.cdr.detectChanges(); // Ensure canvas is rendered

      setTimeout(() => {
        this.initChart(labels, data1, data2);
      });
    } catch (err) {
      console.log(err);
      this.error = 'Failed to load comparison data.';
    } finally {
      this.isLoading = false;
    }
  }

  initChart(labels: string[], data1: number[], data2: number[]) {
    if (!this.chartRef || !this.chartRef.nativeElement) {
      console.warn('ChartRef is not initialized yet');
      return;
    }

    if (this.chart) this.chart.destroy();

    const month1Name = this.months.find(m => m.value === this.month1)?.name || '';
    const month2Name = this.months.find(m => m.value === this.month2)?.name || '';

    this.chart = new Chart(this.chartRef.nativeElement, {
      type: 'bar',
      data: {
        labels,
        datasets: [
          {
            label: `${month1Name} ${this.year1}`,
            data: data1,
            backgroundColor: 'rgba(239, 68, 68, 0.8)',
            borderColor: 'var(--expense)',
            borderWidth: 1,
          },
          {
            label: `${month2Name} ${this.year2}`,
            data: data2,
            backgroundColor: 'rgba(34, 197, 94, 0.8)',
            borderColor: 'var(--warning)',
            borderWidth: 1,
          },
        ],
      },
      options: {
        responsive: true,
        plugins: {
          title: {
            display: true,
            text: 'Expense Comparison by Category',
          },
          legend: {
            position: 'top',
            labels: {
              color: '#a8a29e',
            },
          },
        },
        scales: {
          x: {
            ticks: {
              color: '#a8a29e',
            },
            grid: {
              color: 'var(--border)',
            },
          },
          y: {
            beginAtZero: true,
            ticks: {
              color: '#a8a29e',
            },
            grid: {
              color: 'var(--border)',
            },
          },
        },
      },
    });
  }
}
