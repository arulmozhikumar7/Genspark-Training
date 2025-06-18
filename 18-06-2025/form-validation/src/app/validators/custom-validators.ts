import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class CustomValidators {
  
  // Password strength validator
  static strongPassword(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
      
      const hasUpperCase = /[A-Z]/.test(value);
      const hasLowerCase = /[a-z]/.test(value);
      const hasNumeric = /[0-9]/.test(value);
      const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(value);
      const isValidLength = value.length >= 8;
      
      const passwordValid = hasUpperCase && hasLowerCase && hasNumeric && hasSpecialChar && isValidLength;
      
      if (!passwordValid) {
        return {
          strongPassword: {
            hasUpperCase,
            hasLowerCase,
            hasNumeric,
            hasSpecialChar,
            hasValidLength: isValidLength
          }
        };
      }
      
      return null;
    };
  }
  
  // Username availability validator (simulates async check)
  static usernameValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
      
      // Simulate checking against reserved usernames
      const reservedUsernames = ['admin', 'root', 'user', 'test', 'demo'];
      
      if (reservedUsernames.includes(value.toLowerCase())) {
        return { usernameReserved: { value } };
      }
      
      // Check minimum length and valid characters
      if (value.length < 3) {
        return { usernameTooShort: { minLength: 3, actualLength: value.length } };
      }
      
      if (!/^[a-zA-Z0-9_-]+$/.test(value)) {
        return { usernameInvalidChars: true };
      }
      
      return null;
    };
  }
  
  // Age range validator
  static ageRange(min: number, max: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
      
      const age = parseInt(value, 10);
      if (isNaN(age)) return { invalidAge: true };
      
      if (age < min || age > max) {
        return { ageRange: { min, max, actual: age } };
      }
      
      return null;
    };
  }
  
  // Cross-field validator for password confirmation
  static passwordMatch(passwordField: string, confirmPasswordField: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = control.get(passwordField);
      const confirmPassword = control.get(confirmPasswordField);
      
      if (!password || !confirmPassword) return null;
      
      if (password.value !== confirmPassword.value) {
        confirmPassword.setErrors({ passwordMismatch: true });
        return { passwordMismatch: true };
      }
      
      // Clear the error if passwords match
      if (confirmPassword.errors && confirmPassword.errors['passwordMismatch']) {
        delete confirmPassword.errors['passwordMismatch'];
        if (Object.keys(confirmPassword.errors).length === 0) {
          confirmPassword.setErrors(null);
        }
      }
      
      return null;
    };
  }
}
