import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertController, LoadingController } from '@ionic/angular';
import { catchError } from 'rxjs';
import { AuthenticationService, CLIENT_URI_EMAIL_CONFIRMATION } from '../../../services/authentication.service';
import { RegisterRequest } from '../../../interfaces/register-request';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
})
export class RegistrationComponent implements OnInit {

  registerForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthenticationService,
    private router: Router,
    private loadingController: LoadingController,
    private alertController: AlertController,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.registerForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  async register() {
    if (this.password?.value !== this.confirmPassword?.value) {
      let alert = await this.alertController.create({
        header: "Registrierungsfehler",
        message: "Passwort stimmt nicht mit dem wiederholten Passwort überein. Wiederholen sie die Passworteingabe.",
        buttons: ['OK']
      });
      return await alert.present();
    }
    const loading = await this.loadingController.create();
    await loading.present();

    const registrationKey = this.route.snapshot.queryParams['registrationKey'];
    const email = this.route.snapshot.queryParams['email'];

    let registerRequest: RegisterRequest = {
      registerationKey: registrationKey,
      email: email,
      password: this.password?.value,
      clientUri: CLIENT_URI_EMAIL_CONFIRMATION
    };

    this.authService.register(registerRequest)
      .pipe(
        catchError(async () => await loading.dismiss())
      )
      .subscribe(async (res) => {
        if (!res || res === true) return;
        await loading.dismiss();
        this.router.navigate(['auth']);
        let alert = await this.alertController.create({
          header: "Registrierung erfolgreich",
          message: "Dein Account wurde erfolgreich angelegt. Du kannst ihn jetzt verwenden, um dich einzuloggen.",
          buttons: ['OK']
        });
        return await alert.present();
      });
  }

  get password() {
    return this.registerForm.get('password');
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }
}
