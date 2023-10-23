import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertController, LoadingController } from '@ionic/angular';
import { AuthenticationService, CLIENT_URI_EMAIL_CONFIRMATION, CLIENT_URI_PASSWORD_RESET } from '../../services/authentication.service';
import { catchError } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

  credentials!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthenticationService,
    private router: Router,
    private loadingController: LoadingController,
    private alertController: AlertController,
  ) { }

  ngOnInit() {
    this.credentials = this.fb.group({
      email: ['tim@test', [Validators.required, Validators.email]],
      password: ['P@ssw0rd', [Validators.required, Validators.minLength(6)]]
    });
  }

  async login() {
    const loading = await this.loadingController.create();
    await loading.present();

    this.authService.login(this.credentials.value)
      .pipe(
        catchError(async (error) => {
          await loading.dismiss();
        })
      )
      .subscribe(async (res) => {
          if(!res) return;
          await loading.dismiss();
          this.router.navigateByUrl('/tabs', { replaceUrl: true });
        }
      );
  }

  get email() {
    return this.credentials.get('email');
  }

  get password() {
    return this.credentials.get('password');
  }

  async forgotPassword(){
    if(!this.email?.value){
      let alert = await this.alertController.create({
        header: "Email wird benötigt",
        message: "Bitte trage die E-Mail Adresse ins Formular ein.",
        buttons: ['OK']
      });
      return await alert.present();
    }

    const loading = await this.loadingController.create();
    await loading.present();

    this.authService.forgotPassword({email: this.email.value, clientUri: CLIENT_URI_PASSWORD_RESET})
      .pipe(
        catchError(async () => {
          await loading.dismiss();
        })
      )
      .subscribe(async (res) => {
          if(!res) return;
          await loading.dismiss();

          let alert = await this.alertController.create({
            header: "Link erfolgreich versendet.",
            message: `Ein Link zum Reseten deines Passworts wurde an die E-Mail Adresse ${this.email?.value} versendet. Klicke auf den Link in dieser Mail, um dein Passwort zu ändern.`,
            buttons: ['OK']
          });
          return await alert.present();
        }
      );
  }

}
