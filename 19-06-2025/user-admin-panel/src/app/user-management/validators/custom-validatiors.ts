import { AbstractControl, ValidationErrors , ValidatorFn , FormGroup } from "@angular/forms";

export class CustomValidators{
    // Banned Usernames
    static bannedUsernames(bannedWords: string[]) : ValidatorFn{
        return (control: AbstractControl) : ValidationErrors | null => {
            if(!control.value) return null;
            const isBanned = bannedWords.some(word => control.value.toLowerCase().includes(word.toLocaleLowerCase()));
            return isBanned ? {bannedUsername: true} : null;
        }
    }

    // Password
    static passwordStrength(): ValidatorFn{
        return (control: AbstractControl) : ValidationErrors | null =>{
            const value = control.value;
            if(!value) return null;
            const hasMinLength = value.length >= 8;
            const hasNumber = /\d/.test(value);
            const hasSymbol = /[]!@#$%^&*(),.?":,<>{}|/.test(value);

            const valid = hasMinLength && hasNumber && hasSymbol;
            return !valid ? {weakPassword: true} : null;
        };
    }

    // Confirm Password
    static matchPasswords(passwordKey: string, confirmPasswordKey: string): ValidatorFn{
        return (group: AbstractControl): ValidationErrors | null =>{
            const password = (group as FormGroup).get(passwordKey)?.value;
            const confirmPassword = (group as FormGroup).get(confirmPasswordKey)?.value;
            if(password != confirmPassword){
                return { passwordsMismatch: true};
            }
            return null;
        }
    }
}