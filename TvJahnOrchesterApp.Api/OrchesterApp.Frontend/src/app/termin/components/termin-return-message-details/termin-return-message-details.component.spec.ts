import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { TerminReturnMessageDetailsComponent } from './termin-return-message-details.component';

describe('TerminReturnMessageDetailsComponent', () => {
  let component: TerminReturnMessageDetailsComponent;
  let fixture: ComponentFixture<TerminReturnMessageDetailsComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ TerminReturnMessageDetailsComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(TerminReturnMessageDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
