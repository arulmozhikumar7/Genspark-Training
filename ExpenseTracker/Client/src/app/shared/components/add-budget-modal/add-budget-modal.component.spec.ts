import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddBudgetModalComponent } from './add-budget-modal.component';
import { By } from '@angular/platform-browser';
import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-add-budget-form',
  template: '<div></div>',
})
class AddBudgetFormStubComponent {
  @Output() formSubmitted = new EventEmitter<void>();
}

describe('AddBudgetModalComponent', () => {
  let component: AddBudgetModalComponent;
  let fixture: ComponentFixture<AddBudgetModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddBudgetModalComponent, AddBudgetFormStubComponent],  
    }).overrideComponent(AddBudgetModalComponent, {
      set: {
        imports: [AddBudgetFormStubComponent], 
      },
    }).compileComponents();

    fixture = TestBed.createComponent(AddBudgetModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the add budget modal component', () => {
    expect(component).toBeTruthy();
  });

  it('should emit close event when close button is clicked', () => {
    spyOn(component.close, 'emit');
    const closeBtn = fixture.debugElement.query(By.css('button[aria-label="Close"]'));
    closeBtn.triggerEventHandler('click', null);
    expect(component.close.emit).toHaveBeenCalled();
  });

  it('should emit close event when formSubmitted event is emitted by app-add-budget-form', () => {
    spyOn(component.close, 'emit');

    const formCompDE = fixture.debugElement.query(By.directive(AddBudgetFormStubComponent));
    formCompDE.componentInstance.formSubmitted.emit();

    expect(component.close.emit).toHaveBeenCalled();
  });
});
