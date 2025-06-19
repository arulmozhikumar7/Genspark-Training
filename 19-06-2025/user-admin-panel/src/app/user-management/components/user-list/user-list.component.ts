import {
  Component,
  ElementRef,
  OnInit,
  ViewChild,
  AfterViewInit
} from '@angular/core';
import {
  CommonModule
} from '@angular/common';
import {
  BehaviorSubject,
  combineLatest,
  debounceTime,
  distinctUntilChanged,
  fromEvent,
  map,
  Observable,
  startWith
} from 'rxjs';
import { User } from '../../models/user.model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit, AfterViewInit {
  @ViewChild('searchInput', { static: true }) searchInput!: ElementRef;
  @ViewChild('roleSelect', { static: true }) roleSelect!: ElementRef;

  roles = ['All', 'Admin', 'User', 'Guest'];
  filteredUsers$!: Observable<User[]>;

  private searchTerm$ = new BehaviorSubject<string>('');
  private selectedRole$ = new BehaviorSubject<string>('All');

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.filteredUsers$ = combineLatest([
      this.userService.getUsers(),
      this.searchTerm$.pipe(startWith('')),
      this.selectedRole$.pipe(startWith('All'))
    ]).pipe(
      map(([users, term, role]) => {
        const lowerTerm = term.toLowerCase();
        return users.filter(user => {
          const matchesName = user.username.toLowerCase().includes(lowerTerm);
          const matchesRole = role === 'All' || user.role === role;
          return matchesName && matchesRole;
        });
      })
    );
  }

  ngAfterViewInit(): void {
    fromEvent(this.searchInput.nativeElement, 'input')
      .pipe(
        map((event: any) => event.target.value),
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(value => this.searchTerm$.next(value));

    fromEvent(this.roleSelect.nativeElement, 'change')
      .pipe(
        map((event: any) => event.target.value),
        distinctUntilChanged()
      )
      .subscribe(value => this.selectedRole$.next(value));
  }
}
