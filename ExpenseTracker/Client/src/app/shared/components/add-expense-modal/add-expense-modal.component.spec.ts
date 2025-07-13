import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddExpenseModalComponent } from './add-expense-modal.component';
import { By } from '@angular/platform-browser';
import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-add-expense-form',
  template: '<div></div>',
})
class AddExpenseFormStubComponent {
  @Output() formSubmitted = new EventEmitter<void>();
}

describe('AddExpenseModalComponent', () => {
  let component: AddExpenseModalComponent;
  let fixture: ComponentFixture<AddExpenseModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddExpenseModalComponent, AddExpenseFormStubComponent],  
    }).overrideComponent(AddExpenseModalComponent, {
      set: {
        imports: [AddExpenseFormStubComponent], 
      },
    }).compileComponents();

    fixture = TestBed.createComponent(AddExpenseModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the modal component', () => {
    expect(component).toBeTruthy();
  });

  it('should emit close event when close button is clicked', () => {
    spyOn(component.close, 'emit');
    const closeBtn = fixture.debugElement.query(By.css('button'));
    closeBtn.triggerEventHandler('click', null);
    expect(component.close.emit).toHaveBeenCalled();
  });

  it('should emit close event when formSubmitted event is emitted by app-add-expense-form', () => {
    spyOn(component.close, 'emit');

    const formCompDE = fixture.debugElement.query(By.directive(AddExpenseFormStubComponent));
    formCompDE.componentInstance.formSubmitted.emit();

    expect(component.close.emit).toHaveBeenCalled();
  });
});
