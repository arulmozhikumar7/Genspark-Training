import {
  Component,
  ElementRef,
  ViewChild,
  AfterViewInit,
  OnDestroy,
  inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { selectAllBudgets } from '@store/budget/budget.selectors';
import { Budget } from '@shared/models/budget.model';
import Chart from 'chart.js/auto';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { Subscription } from 'rxjs';

Chart.register(ChartDataLabels);

@Component({
  standalone: true,
  selector: 'app-budget-bar-chart',
  imports: [CommonModule],
  templateUrl: './budget-bar-chart.component.html',
})
export class BudgetBarChartComponent implements AfterViewInit, OnDestroy {
  @ViewChild('canvas', { static: false }) canvasRef?: ElementRef<HTMLCanvasElement>;

  private store = inject(Store);
  private chart?: Chart;
  private subscription = new Subscription();
  isFullscreen = false;
  private budgets: Budget[] = [];

  toggleFullscreen(): void {
    this.isFullscreen = !this.isFullscreen;

   
    setTimeout(() => {
      if (this.chart) {
        this.chart.resize();
      }
    }, 300);
  }

  ngAfterViewInit(): void {
    this.subscription = this.store.select(selectAllBudgets).subscribe((budgets) => {
      this.budgets = budgets;
      this.renderChart();
    });
  }

  renderChart(): void {
    const canvas = this.canvasRef?.nativeElement;
    if (!canvas) return;

    if (this.chart) {
      this.chart.destroy();
    }

    const current = new Date();
    const currentMonth = current.getMonth();
    const currentYear = current.getFullYear();
    console.log(this.budgets);
   const filtered = this.budgets.filter((budget) => {
    const startDate = new Date(budget.startDate);
    const endDate = new Date(budget.endDate);
    return startDate <= current && current <= endDate;
  });
    console.log(filtered);
    const labels = filtered.map((b) => b.name);
    const spent = filtered.map((b) => b.limitAmount - b.balanceAmount);
    const actual = filtered.map((b) => b.limitAmount);

    this.chart = new Chart(canvas, {
      type: 'bar',
      data: {
        labels,
        datasets: [
          {
            label: 'Spent',
            data: spent,
            backgroundColor: 'rgba(239, 68, 68, 0.8)',
            datalabels: {
              anchor: 'end',
              align: 'end',
              formatter: (value: number) => `₹${value}`,
            },
            barThickness: 18,
          },
          {
            label: 'Budget',
            data: actual,
            backgroundColor: 'rgba(34, 197, 94, 0.8)',
            datalabels: {
              anchor: 'end',
              align: 'end',
              formatter: (value: number) => `₹${value}`,
            },
            barThickness: 18,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          datalabels: {
            color: '#a8a29e',
            font: {
              weight: 'bold',
              size: 8,
            },
            rotation: -60,
            clamp: true,
          },
          legend: {
            position: 'top',
          },
          tooltip: {
            callbacks: {
              label: (ctx) => `${ctx.dataset.label}: ₹${ctx.raw}`,
            },
          },
        },
        scales: {
          x: {
            stacked: false,
            title: {
              display: true,
              text: 'Budgets',
            },
            ticks: {
              maxRotation: 45,
              minRotation: 0,
            },
          },
          y: {
            beginAtZero: true,
            title: {
              display: true,
              text: 'Amount (INR)',
            },
          },
        },
      },
      plugins: [ChartDataLabels],
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
    if (this.chart) this.chart.destroy();
  }
}
