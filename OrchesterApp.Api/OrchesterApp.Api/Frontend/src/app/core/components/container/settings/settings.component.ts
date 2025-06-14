import { ReturnStatement } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoadingController, AlertController } from '@ionic/angular';
import { catchError } from 'rxjs';
import { AuthenticationService, CLIENT_URI_EMAIL_CONFIRMATION, CLIENT_URI_PASSWORD_RESET } from 'src/app/authentication/services/authentication.service';
import { AuthorizedAuthServiceService } from 'src/app/authentication/services/authorized-auth-service.service';
import { confirmDialog } from 'src/app/core/helper/confirm';
import { ThemeService } from 'src/app/shared/services/theme.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
})
export class SettingsComponent implements OnInit {
  isDarkMode: boolean = false;

  constructor(
    private authService: AuthenticationService,
    private authAuthService: AuthorizedAuthServiceService,
    private router: Router,
    private loadingController: LoadingController,
    private alertController: AlertController,
    private themeService: ThemeService,
  ) { }

  ngOnInit() {
    // Subscribe to theme changes
    this.themeService.theme$.subscribe(theme => {
      this.isDarkMode = theme === 'dark';
    });
  }

  toggleTheme() {
    this.themeService.toggleTheme();
  }

  @confirmDialog("Passwort ändern", "Möchten Sie das Passswort ihres Accounts wirklich ändern?")
  async changePassword(){
    let email = this.authService.userEmail;
    if(!email){
      const alert = await this.alertController.create({
        header: 'Geben Sie die E-Mail Adresse an, zu der der Link zum Zurücksetzen des Passworts geschickt werden soll.',
        inputs: [{
          placeholder: 'E-Mail',
          type: 'text',
        }],
        buttons: [{
          text: 'Abbrechen',
          role: 'cancel',
        },
        {
          text: 'Ändern',
          role: 'confirm',
        }],
      });
  
      await alert.present();
      let alertResult = await alert.onDidDismiss();
      if (alertResult.role === 'confirm') {
        email = alertResult.data.values["0"] ?? '';
      }
      else{
        return;
      }
    }

    const loading = await this.loadingController.create();
    await loading.present();

    this.authService.forgotPassword({email: email!, clientUri: CLIENT_URI_PASSWORD_RESET})
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
            message: `Ein Link zum Reseten deines Passworts wurde an die E-Mail Adresse ${email} versendet. Klicke auf den Link in dieser Mail, um dein Passwort zu ändern.`,
            buttons: ['OK']
          });
          return await alert.present();
        }
      );
  }

  async changeMail(){
    const alert = await this.alertController.create({
      header: 'E-Mail Adresse ändern',
      message: 'Geben Sie ihre alte E-Mail Adresse, ihr Passwort und die gewünschte neue E-Mail Adresse an. Ein Verifizierungslink wird danach an die neue E-Mail Adresse geschickt und muss von ihnen bestätigt werden!',
      inputs: [
        {
          placeholder: 'Alte E-Mail',
          type: 'text',
        },
        {
          placeholder: 'Passwort',
          type: 'password',
        },
        {
          placeholder: 'Neue E-Mail',
          type: 'text',
        },
      ],
      buttons: [{
        text: 'Abbrechen',
        role: 'cancel',
      },
      {
        text: 'Ändern',
        role: 'confirm',
      }],
    });

    await alert.present();
    let alertResult = await alert.onDidDismiss();
    if (alertResult.role !== 'confirm') {
      return;
    }

    const loading = await this.loadingController.create();
    await loading.present();

    this.authService.changeEmail({
      oldEmail: alertResult.data.values["0"],
      password: alertResult.data.values["1"],
      newEmail: alertResult.data.values["2"],
      clientUri: CLIENT_URI_EMAIL_CONFIRMATION
    }).pipe(catchError( async () => { await loading.dismiss();})).subscribe(async (res) => {
      if(!res) return;
      await loading.dismiss();
      let alert = await this.alertController.create({
        header: "E-Mail erfolgreich geändert.",
        message: `Deine E-Mail Adresse wurde erfolgreich geändert und ein Verifizierungslink wurde an die neue E-mail Adresse geschickt. Klicke auf den Link in dieser Mail, um deine neue Adresse zu verifizieren.`,
        buttons: ['OK']
      });
      return await alert.present();
    })
  }


  @confirmDialog("Achtung", "Möchten Sie ihren Account endgültig löschen? Diese Aktion kann nicht rückgängig gemacht werden!")
  async deleteOwnUser(){
    let alert = await this.alertController.create({
      header: "Account endgültig löschen?",
      message: "Wirklich sicher?",
      buttons: [{
        text: 'Abbrechen',
        role: 'cancel',
      },
      {
        text: 'Löschen',
        role: 'confirm',
      }],
    });
    
    await alert.present();
    let alertResult = await alert.onDidDismiss();
    if (alertResult.role !== 'confirm') {
      return;
    }

    this.authAuthService.deleteOwnUser().subscribe(() => {
      this.authService.logout();
      this.router.navigate(['auth']);
    })
  }

}
