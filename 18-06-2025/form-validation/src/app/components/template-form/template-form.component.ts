import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-template-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './template-form.html',
  styleUrl: './template-form.css'
 
})
export class TemplateFormComponent {
  user = {
    name: '',
    email: '',
    age: null
  };
  
  templateSubmitted = false;

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.templateSubmitted = true;
      console.log('Template Form:', this.user);
    }
  }
}
