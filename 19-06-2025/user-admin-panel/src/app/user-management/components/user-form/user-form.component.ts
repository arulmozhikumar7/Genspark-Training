import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { CustomValidators } from "../../validators/custom-validatiors";
import { UserService } from "../../services/user.service";
import { User } from "../../models/user.model";

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css']
})
export class UserFormComponent implements OnInit {
  userForm!: FormGroup;
  roles = ['Admin', 'User', 'Guest'];
  submitted = false;
  showForm = false;


  constructor(private fb: FormBuilder, private userService: UserService) {}

  ngOnInit(): void {
    this.userForm = this.fb.group({
      username: [
        '',
        [Validators.required, CustomValidators.bannedUsernames(['admin', 'root', 'user', 'guest'])]
      ],
      email: [
        '',
        [Validators.required, Validators.email]
      ],
      password: [
        '',
        [Validators.required, CustomValidators.passwordStrength()]
      ],
      confirmPassword: [
        '',
        [Validators.required]
      ],
      role: [
        '',
        [Validators.required]
      ]
    }, {
      validators: CustomValidators.matchPasswords('password', 'confirmPassword')
    });
  }

  get f() {
    return this.userForm.controls;
  }

  onSubmit(): void { 
    this.submitted = true;

    if (this.userForm.invalid) return;

    const { username, email, password, role } = this.userForm.value;
    const newUser: User = { username, email, password, role };

    this.userService.addUser(newUser);
    alert('User Added Successfully!');
    this.userForm.reset();
    this.submitted = false;
    this.showForm = false;
  }
}
