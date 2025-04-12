import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { AnwesenheitsListeComponent } from './anwesenheits-liste.component';

describe('AnwesenheitsListeComponent', () => {
  let component: AnwesenheitsListeComponent;
  let fixture: ComponentFixture<AnwesenheitsListeComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AnwesenheitsListeComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(AnwesenheitsListeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
