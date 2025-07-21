import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { VideoDashboardComponent } from './components/video-dashboard.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,VideoDashboardComponent],
  templateUrl: './app.html'
})
export class App {
  protected title = 'client';
}
