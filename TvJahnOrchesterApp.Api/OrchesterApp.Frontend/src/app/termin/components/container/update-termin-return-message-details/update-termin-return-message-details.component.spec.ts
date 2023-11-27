import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { UpdateTerminReturnMessageDetailsComponent } from './update-termin-return-message-details.component';

describe('UpdateTerminReturnMessageDetailsComponent', () => {
  let component: UpdateTerminReturnMessageDetailsComponent;
  let fixture: ComponentFixture<UpdateTerminReturnMessageDetailsComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateTerminReturnMessageDetailsComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(UpdateTerminReturnMessageDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
