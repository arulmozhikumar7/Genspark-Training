import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError, throwError } from 'rxjs';
import { WeatherData } from '../models/weather.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  private readonly API_KEY = 'b34bf1d992bf462f6b91850fd191f33e'; 
  private readonly BASE_URL = 'https://api.openweathermap.org/data/2.5/weather';

  private weatherSubject = new BehaviorSubject<WeatherData | null>(null);
  weather$ = this.weatherSubject.asObservable();

  private errorSubject = new BehaviorSubject<string | null>(null);
  error$ = this.errorSubject.asObservable();

  constructor(private http: HttpClient) {}

  fetchWeather(city: string) {
    const url = `${this.BASE_URL}?q=${city}&appid=${this.API_KEY}&units=metric`;

    this.http.get<any>(url)
      .pipe(
        map(data => this.transformToWeatherData(data)),
        catchError(err => {
          this.errorSubject.next(
            err.status === 404 ? 'City not found' : 'Failed to fetch weather'
          );
          return throwError(() => err);
        })
      )
      .subscribe({
        next: weather => {
          this.errorSubject.next(null);
          this.weatherSubject.next(weather);
        },
        error: () => {
          this.weatherSubject.next(null);
        }
      });
  }

  private transformToWeatherData(data: any): WeatherData {
    return {
      city: data.name,
      temperature: data.main.temp,
      condition: data.weather[0].main,
      icon: `https://openweathermap.org/img/wn/${data.weather[0].icon}@2x.png`,
      humidity: data.main.humidity,
      windSpeed: data.wind.speed
    };
  }
}
