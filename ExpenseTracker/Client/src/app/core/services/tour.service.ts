// tour.service.ts
import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { BehaviorSubject ,Observable,tap,catchError,of} from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TourService {
  private readonly storageKey = 'tour_progress';
  private progressReadySubject = new BehaviorSubject<boolean>(false);
  progressReady$ = this.progressReadySubject.asObservable();

  constructor(private http: HttpService) {}

loadProgressFromApi(): Observable<any> {
  return this.http.get<any>('/TourProgress').pipe(
    tap((progress) => {
      localStorage.setItem(this.storageKey, JSON.stringify(progress.data));
      this.progressReadySubject.next(true);
    }),
    catchError((err) => {
      this.progressReadySubject.next(true);
      return of(null); // avoid crashing
    })
  );
}
  isCompleted(tourKey: string): boolean {
    const progress = JSON.parse(localStorage.getItem(this.storageKey) || '{}');
    return progress[tourKey] === true;
  }

  markCompleted(tourKey: string) {
    const progress = JSON.parse(localStorage.getItem(this.storageKey) || '{}');
    progress[tourKey] = true;
    localStorage.setItem(this.storageKey, JSON.stringify(progress));

    this.http.post(`/TourProgress/complete/${tourKey}`, {}).subscribe();
  }

  clearProgress() {
    localStorage.removeItem(this.storageKey);
    this.progressReadySubject.next(false);
  }
}
