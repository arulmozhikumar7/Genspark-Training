import { Injectable } from '@angular/core';
import { map, shareReplay } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { HttpService } from '@core/services/http.service'; 

export interface FeatureFlags {
  enableCsvExport: boolean;
}

@Injectable({ providedIn: 'root' })
export class FeatureFlagService {
  private flags$!: Observable<FeatureFlags>;

  constructor(private http: HttpService) {
    this.flags$ = this.http.get<FeatureFlags>('/FeatureFlags')
      .pipe(
        shareReplay(1) 
      );
  }

  getFlags(): Observable<FeatureFlags> {
    return this.flags$;
  }
}
