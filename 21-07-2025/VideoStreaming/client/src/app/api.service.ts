// api.service.ts
import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

export interface TrainingVideo {
  id: number;
  title: string;
  description: string;
  blobUrl: string;
  uploadDate: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  private baseUrl = 'http://localhost:5110/api/videos';

  private _videos = signal<TrainingVideo[]>([]);
  videos = () => this._videos();

  constructor(private http: HttpClient) {
    this.loadVideos();
  }

  loadVideos() {
    this.http.get<TrainingVideo[]>(this.baseUrl).subscribe(videos => this._videos.set(videos));
  }

  upload(formData: FormData) {
    return this.http.post<TrainingVideo>(`${this.baseUrl}/upload`, formData).pipe(
      tap(video => this._videos.update(v => [video, ...v]))
    );
  }

  download(video: TrainingVideo) {
    const a = document.createElement('a');
    a.href = `${this.baseUrl}/${video.id}/stream`;
    a.download = video.title + '.mp4';
    a.target = '_blank';
    a.click();
  }
}
