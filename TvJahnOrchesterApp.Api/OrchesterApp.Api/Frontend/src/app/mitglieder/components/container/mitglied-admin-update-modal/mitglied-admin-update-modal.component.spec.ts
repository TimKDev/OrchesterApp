import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { MitgliedAdminUpdateModalComponent } from './mitglied-admin-update-modal.component';

describe('MitgliedAdminUpdateModalComponent', () => {
  let component: MitgliedAdminUpdateModalComponent;
  let fixture: ComponentFixture<MitgliedAdminUpdateModalComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ MitgliedAdminUpdateModalComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(MitgliedAdminUpdateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
