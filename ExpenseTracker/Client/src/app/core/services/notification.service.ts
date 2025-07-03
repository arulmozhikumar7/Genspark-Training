import { Injectable } from '@angular/core';
import { ToastrService, IndividualConfig } from 'ngx-toastr';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  constructor(private toastr: ToastrService) {}

  success(message: string, title: string = 'Success', options?: Partial<IndividualConfig>) {
    this.toastr.success(message, title, options);
  }

  error(message: string, title: string = 'Error', options?: Partial<IndividualConfig>) {
    this.toastr.error(message, title, options);
  }

  info(message: string, title: string = 'Info', options?: Partial<IndividualConfig>) {
    this.toastr.info(message, title, options);
  }

  warn(message: string, title: string = 'Warning', options?: Partial<IndividualConfig>) {
    this.toastr.warning(message, title, options);
  }
}
