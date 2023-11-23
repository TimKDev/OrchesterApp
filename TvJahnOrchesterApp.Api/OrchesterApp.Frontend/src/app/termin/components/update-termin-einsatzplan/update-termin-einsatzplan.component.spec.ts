import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { UpdateTerminEinsatzplanComponent } from './update-termin-einsatzplan.component';

describe('UpdateTerminEinsatzplanComponent', () => {
  let component: UpdateTerminEinsatzplanComponent;
  let fixture: ComponentFixture<UpdateTerminEinsatzplanComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateTerminEinsatzplanComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(UpdateTerminEinsatzplanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
