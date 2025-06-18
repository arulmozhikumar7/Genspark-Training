import { Component } from '@angular/core';
import { TemplateFormComponent } from './components/template-form/template-form.component';
import { ReactiveFormComponent } from './components/reactive-form/reactive-form.component';
import { CustomValidatorComponent } from './components/custom-validator/custom-validator.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [TemplateFormComponent, ReactiveFormComponent,CustomValidatorComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {}