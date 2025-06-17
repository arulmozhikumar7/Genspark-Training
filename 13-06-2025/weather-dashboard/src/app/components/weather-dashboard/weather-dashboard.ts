import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WeatherService } from '../../services/weather';
import { WeatherCardComponent } from '../weather-card/weather-card';

@Component({
  selector: 'app-weather-dashboard',
  standalone: true,
  imports: [CommonModule, WeatherCardComponent],
  templateUrl: './weather-dashboard.html',
  styleUrls: ['./weather-dashboard.css']
})
export class WeatherDashboardComponent {
  constructor(private weatherService: WeatherService) {}

  get weather$() {
    return this.weatherService.weather$;
  }

  get error$() {
    return this.weatherService.error$;
  }
}
