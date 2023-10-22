import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingController } from '@ionic/angular';
import { catchError } from 'rxjs';
import { AuthenticationService } from '../../services/authentication.service';

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
    private loadingController: LoadingController
  ) { }

  ngOnInit() {
    this.changeEmailFormGroup = this.fb.group({
      email: [this.authService.userEmail, [Validators.required, Validators.email]],
    });
  }

  async changeEmail() {
    const loading = await this.loadingController.create();
    await loading.present();

    this.authService.login(this.changeEmailFormGroup.value)
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

  resendVerificationMail(){

  }

  get email() {
    return this.changeEmailFormGroup.get('email');
  }

}
