import { computed, signal } from '@angular/core';
import { User, UserFilters } from '../../core/models/user.model';

export const users = signal<User[]>([]);

export const filters = signal<UserFilters>({});

export const filteredUsers = computed(() => {
  const all = users();
  const { gender, role, state, minAge, maxAge } = filters();

  return all.filter(user => {
    const matchGender = !gender || user.gender === gender;
    const matchRole = !role || user.role === role;
    const matchState = !state || user.address?.state === state;
    const matchMinAge = !minAge || user.age >= minAge;
    const matchMaxAge = !maxAge || user.age <= maxAge;

    return matchGender && matchRole && matchState && matchMinAge && matchMaxAge;
  });
});

export const availableStates = computed(() => {
  const all = users();
  const states = new Set<string>();

  all.forEach(user => {
    const state = user.address?.state;
    if (state) {
      states.add(state);
    }
  });

  return Array.from(states).sort();
});
