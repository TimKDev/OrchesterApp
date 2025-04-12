import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { TerminAnwesenheitControlModalComponent } from './termin-anwesenheit-control-modal.component';

describe('TerminAnwesenheitControlModalComponent', () => {
  let component: TerminAnwesenheitControlModalComponent;
  let fixture: ComponentFixture<TerminAnwesenheitControlModalComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ TerminAnwesenheitControlModalComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(TerminAnwesenheitControlModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
