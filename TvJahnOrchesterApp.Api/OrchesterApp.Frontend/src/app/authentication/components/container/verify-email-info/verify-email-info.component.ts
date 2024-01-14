import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertController, LoadingController } from '@ionic/angular';
import { catchError } from 'rxjs';
import { AuthenticationService, CLIENT_URI_EMAIL_CONFIRMATION } from '../../../services/authentication.service';

@Component({
  selector: 'app-verify-email-info',
  templateUrl: './verify-email-info.component.html',
  styleUrls: ['./verify-email-info.component.scss'],
})
export class VerifyEmailInfoComponent  implements OnInit {

  showChangeEmailForm = false;
  changeEmailFormGroup!: FormGroup;


  constructor(
    private fb: FormBuilder,
    public authService: AuthenticationService,
    private router: Router,
    private loadingController: LoadingController,
    private alertController: AlertController,
  ) { }

  ngOnInit() {
    this.changeEmailFormGroup = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6)]],
      newEmail: ['', [Validators.required, Validators.email]],
    });
  }

  async changeEmail() {
    const loading = await this.loadingController.create();
    await loading.present();
    this.authService.changeEmail({...this.changeEmailFormGroup.value, oldEmail: this.authService.userEmail!, clientUri: CLIENT_URI_EMAIL_CONFIRMATION})
      .pipe(catchError(async () => {
        await loading.dismiss()
        return {error: true}
      }))
      .subscribe(async (res) => {
        if(res?.error) return;
        this.showChangeEmailForm = false;
        await loading.dismiss();
        let alert = await this.alertController.create({
          header: "Email wurde erfolgreich geändert",
          message: "Deine Email wurde erfolgreich geändert und eine neue Verifizierungsmail wurde an die neue E-Mail Adresse versendet. Bitte klicke auf den Link in der Verifizierungsmail, um deine neue Adresse zu verifizieren.",
          buttons: ['OK']
        });
        this.router.navigate(['auth']);
        return await alert.present();

      });
  }

  async resendVerificationMail(){
    const loading = await this.loadingController.create();
    await loading.present();
    this.authService.resendVerificationMail({email: this.authService.userEmail!, clientUri: CLIENT_URI_EMAIL_CONFIRMATION})
      .pipe(catchError(async () => await loading.dismiss()))
      .subscribe(async () => {
        await loading.dismiss();
        let alert = await this.alertController.create({
          header: "Verifizierungsmail erfolgreich versendet",
          message: "Die Verifizierungsmail wurde erneut versendet. Bitte klicke auf den Link in der Mail, um deine E-Adresse zu verifizieren. Falls du die Mail nicht finden kannst, überprüfe ob du die korrekt Adresse eingegeben hast oder ob die Mail in deinen Spam Ordner gelandet ist.",
          buttons: ['OK']
        });
        return await alert.present();
      });
  }

  get newEmail() {
    return this.changeEmailFormGroup.get('newEmail');
  }

  get password() {
    return this.changeEmailFormGroup.get('password');
  }

}
