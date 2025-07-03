import { Injectable } from '@angular/core';

const expense_tracker_username = 'Guest';


@Injectable({ providedIn: 'root' })
export class UserService {
  // Access Username
  setUsername(name: string): void {
    localStorage.setItem(expense_tracker_username, name);
  }

  getUsername(): string | null {
    return localStorage.getItem(expense_tracker_username);
  }

  removeUsername(): void {
    localStorage.removeItem(expense_tracker_username);
  }



 

}
