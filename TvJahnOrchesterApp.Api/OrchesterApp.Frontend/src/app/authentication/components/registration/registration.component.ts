import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertController, LoadingController } from '@ionic/angular';
import { catchError } from 'rxjs';
import { AuthenticationService } from '../../services/authentication.service';
import { RegisterRequest } from '../../interfaces/register-request';
import { BASE_PATH, BASE_PATH_FRONTEND } from 'src/app/core/services/unauthorized-http-client.service';

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
  ) { }

  ngOnInit() {
    this.registerForm = this.fb.group({
      registerationKey: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
    });
  }


  async register() {
    if(this.password?.value !== this.confirmPassword?.value){
      let alert = await this.alertController.create({
        header: "Registrierungsfehler",
        message: "Passwort stimmt nicht mit dem wiederholten Passwort überein. Wiederholen sie die Passworteingabe.",
        buttons: ['OK']
      });
      return await alert.present();
    }
    const loading = await this.loadingController.create();
    await loading.present();

    let registerRequest: RegisterRequest = {
      registerationKey: this.registerationKey?.value,
      email: this.email?.value,
      password: this.password?.value,
      clientUri: `${BASE_PATH_FRONTEND}auth/email-confirmation`
    };

    this.authService.register(registerRequest)
      .pipe(
        catchError(async () => await loading.dismiss())
      )
      .subscribe(async (res) => {
        if (!res) return;
        await loading.dismiss();
        this.router.navigateByUrl('/tabs', { replaceUrl: true });
      });
  }

  get registerationKey() {
    return this.registerForm.get('registerationKey');
  }

  get email() {
    return this.registerForm.get('email');
  }

  get password() {
    return this.registerForm.get('password');
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }
}
