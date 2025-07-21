import { Component, signal, computed, inject } from '@angular/core';
import { ApiService, TrainingVideo } from '../api.service';
import { CommonModule } from '@angular/common';
import {FormsModule} from '@angular/forms'
@Component({
  selector: 'app-video-dashboard',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './video-dashboard.component.html'
})
export class VideoDashboardComponent {
  search = signal('');
  showUploadModal = signal(false);
  showPlayerModal = signal(false);
  selectedVideo = signal<TrainingVideo | null>(null);

  title = '';
  description = '';
  file: File | null = null;
  uploading = signal(false);

  public api = inject(ApiService);

  readonly filteredVideos = computed(() => {
    const query = this.search().toLowerCase();
    return this.api.videos().filter((v: TrainingVideo) =>
      v.title.toLowerCase().includes(query) ||
      v.description.toLowerCase().includes(query)
    );
  });

  playVideo(id: number) {
    const video = this.api.videos().find(v => v.id === id);
    if (video) {
      this.selectedVideo.set(video);
      this.showPlayerModal.set(true);
    }
  }

  downloadVideo(id: number) {
    const video = this.api.videos().find(v => v.id === id);
    if (video) this.api.download(video);
  }

  submitForm() {
    console.log("Submit triggered");

    if (!this.title || !this.description || !this.file) {
      alert('Please fill all fields and select a file');
      return;
    }

    this.uploading.set(true);
    const formData = new FormData();
    formData.append('file', this.file);
    formData.append('title', this.title);
    formData.append('description', this.description);

    this.api.upload(formData).subscribe({
      next: () => {
        this.title = '';
        this.description = '';
        this.file = null;
        this.uploading.set(false);
        this.showUploadModal.set(false);
        alert('Uploaded successfully');
        this.api.loadVideos();
      },
      error: () => {
        this.uploading.set(false);
        alert('Upload failed');
      }
    });
  }

  onFileChange(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    this.file = fileInput?.files?.[0] ?? null;
  }
}
