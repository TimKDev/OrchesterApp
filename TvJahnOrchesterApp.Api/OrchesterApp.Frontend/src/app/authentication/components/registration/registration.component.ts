import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingController } from '@ionic/angular';
import { catchError } from 'rxjs';
import { AuthenticationService } from '../../services/authentication.service';

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
    private loadingController: LoadingController
  ) { }

  ngOnInit() {
    this.registerForm = this.fb.group({
      registerKey: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: [''],
    });

    this.registerForm.get('confirmPassword')?.setValidators([
      Validators.required,
      Validators.minLength(6),
      this.validateConfirmPassword(this.registerForm.get('password'))
    ]);

    this.registerForm.get('password')?.setValidators([
      Validators.required,
      Validators.minLength(6),
      this.validatePassword(this.registerForm.get('confirmPassword'))
    ]);
  }


  async register() {
    const loading = await this.loadingController.create();
    await loading.present();

    this.authService.login(this.registerForm.value)
      .pipe(
        catchError(async () => await loading.dismiss())
      )
      .subscribe(async (res) => {
        if (!res) return;
        await loading.dismiss();
        this.router.navigateByUrl('/tabs', { replaceUrl: true });
      });
  }

  get registerKey() {
    return this.registerForm.get('registerKey');
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

  private validatePassword(confirmationControl: AbstractControl | null): ValidatorFn {
    return (passwordControl: AbstractControl): { [key: string]: boolean } | null => {
      const passwordValue = passwordControl.value;
      const confirmValue = confirmationControl?.value;
      if(!confirmValue) return null;
      if (confirmValue !== passwordValue) {
        return { mustMatch: true }
      }
      return null;
    };
  }

  private validateConfirmPassword(passwordControl: AbstractControl | null): ValidatorFn {
    return (confirmationControl: AbstractControl): { [key: string]: boolean } | null => {
      const confirmValue = confirmationControl.value;
      const passwordValue = passwordControl?.value;
      if (confirmValue !== passwordValue) {
        return { mustMatch: true }
      }
      return null;
    };
  }
}
