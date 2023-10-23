import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertController, LoadingController } from '@ionic/angular';
import { catchError } from 'rxjs';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent  implements OnInit {

  credentials!: FormGroup;
  token!: string;
  email!: string;

  constructor(
    private fb: FormBuilder,
    private authService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute,
    private loadingController: LoadingController,
    private alertController: AlertController,
  ) { }

  ngOnInit() {
    this.token = this.route.snapshot.queryParams['token'];
    this.email = this.route.snapshot.queryParams['email'];
    this.credentials = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  async resetPassword() {
    if(this.password?.value !== this.confirmPassword?.value){
      let alert = await this.alertController.create({
        header: "Fehler",
        message: "Passwort stimmt nicht mit dem wiederholten Passwort überein. Wiederholen Sie die Passworteingabe.",
        buttons: ['OK']
      });
      return await alert.present();
    }

    const loading = await this.loadingController.create();
    await loading.present();

    this.authService.resetPassword({
      email: this.email,
      password: this.password?.value,
      resetPasswordToken: this.token
    }).pipe(
        catchError(async () => {
          await loading.dismiss();
        })
      )
      .subscribe(async (res) => {
          if(!res) return;
          await loading.dismiss();
          this.router.navigate(['auth'], { replaceUrl: true });
          let alert = await this.alertController.create({
            header: "Passwort erfolgreich geändert",
            message: "Sie können sich jetzt mit dem neuen Passwort einloggen.",
            buttons: ['OK']
          });
          return await alert.present();
        }
      );
  }

  get password() {
    return this.credentials.get('password');
  }

  get confirmPassword() {
    return this.credentials.get('confirmPassword');
  }
}
