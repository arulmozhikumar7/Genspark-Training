import { Component } from '@angular/core';
import { CitySearchComponent } from './components/city-search/city-search';
import { WeatherDashboardComponent } from './components/weather-dashboard/weather-dashboard';
@Component({
  selector: 'app-root',
  imports: [CitySearchComponent,WeatherDashboardComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'weather-dashboard';
}
