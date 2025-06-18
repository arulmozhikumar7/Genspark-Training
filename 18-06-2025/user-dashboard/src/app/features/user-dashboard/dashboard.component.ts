import { Component, OnInit, AfterViewInit, ViewChild, ElementRef, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Chart, BarController, BarElement, CategoryScale, LinearScale, Title, Tooltip, Legend, ChartType } from 'chart.js';

import { UserService } from '../../core/services/user.service';
import { User } from '../../core/models/user.model';
import { users, filteredUsers, filters, availableStates } from './dashboard.signals';

export const showAddModal = signal(false);

Chart.register(
  BarController,
  BarElement,
  CategoryScale,
  LinearScale,
  Title,
  Tooltip,
  Legend
);
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, AfterViewInit {
  private userService = inject(UserService);

  filteredUsers = filteredUsers;
  filters = filters;
  availableStates = availableStates;
  showAddModal = showAddModal;

  defaultImageUrl = 'https://via.placeholder.com/100?text=User';

  @ViewChild('stateChart') stateChartRef!: ElementRef<HTMLCanvasElement>;
  chart!: Chart;

  newUser: Partial<User> = {
    firstName: '',
    lastName: '',
    age: 18,
    gender: 'male',
    email: '',
    phone: '',
    username: '',
    password: '',
    birthDate: '',
    image: '',
    bloodGroup: '',
    height: 160,
    weight: 60,
    eyeColor: '',
    hair: {
      color: '',
      type: '',
    },
    ip: '',
    address: {
      address: '',
      city: '',
      state: '',
      stateCode: '',
      postalCode: '',
      coordinates: { lat: 0, lng: 0 },
    },
    role: 'user',
  };

  ngOnInit(): void {
    this.userService.getUsers().subscribe((data: User[]) => {
      users.set(data);
      this.updateChart(data);
    });
  }

  ngAfterViewInit(): void {
    this.initChart();
  }

  initChart(): void {
    this.chart = new Chart(this.stateChartRef.nativeElement, {
      type: 'bar' as ChartType,
      data: {
        labels: [],
        datasets: [{
          label: 'Users by State',
          data: [],
          backgroundColor: '#0077cc'
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: true },
          title: { display: true, text: 'Users by State' }
        }
      }
    });
  }

  updateChart(data: User[]): void {
    const stateCount: Record<string, number> = {};
    data.forEach(user => {
      const state = user.address?.state || 'Unknown';
      stateCount[state] = (stateCount[state] || 0) + 1;
    });

    const labels = Object.keys(stateCount);
    const values = Object.values(stateCount);

    if (this.chart) {
      this.chart.data.labels = labels;
      this.chart.data.datasets[0].data = values;
      this.chart.update();
    }
  }

  openAddModal(): void {
    this.showAddModal.set(true);
  }

  closeAddModal(): void {
    this.showAddModal.set(false);
  }

  addUser(): void {
    this.userService.addUser(this.newUser).subscribe((user) => {
      users.update(current => [user, ...current]);
      this.updateChart(users());
      this.closeAddModal();
    });
  }

  onImageError(event: Event): void {
    const imgElement = event.target as HTMLImageElement;
    imgElement.src = this.defaultImageUrl;
  }

  updateFilter(key: keyof ReturnType<typeof this.filters>, value: string | number | undefined): void {
    this.filters.update(f => ({
      ...f,
      [key]: value || undefined
    }));
  }

  handleSelectChange(event: Event, key: keyof ReturnType<typeof this.filters>): void {
    const value = (event.target as HTMLSelectElement)?.value || undefined;
    this.updateFilter(key, value);
  }

  handleNumberInput(event: Event, key: keyof ReturnType<typeof this.filters>): void {
    const value = (event.target as HTMLInputElement)?.valueAsNumber;
    this.updateFilter(key, isNaN(value) ? undefined : value);
  }
}
