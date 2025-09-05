import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication.service';
import { LoadingController } from '@ionic/angular';
import { catchError, NEVER } from 'rxjs';

@Component({
  selector: 'app-email-confirmed',
  templateUrl: './email-confirmed.component.html',
  styleUrls: ['./email-confirmed.component.scss'],
})
export class EmailConfirmedComponent  implements OnInit {
  showSuccessMessage = false;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthenticationService,
    private loadingController: LoadingController,
  ) { }

  async ngOnInit() {
    const token = this.route.snapshot.queryParams['token'];
    const email = this.route.snapshot.queryParams['email'];
    const loading = await this.loadingController.create();
    await loading.present();
    this.authService.confirmEmail({email, token})
    .pipe(
      catchError(async () => {
        await loading.dismiss();
        return NEVER;
      })
    )
    .subscribe(async () => {
      this.showSuccessMessage = true;
      await loading.dismiss();
    });
  }

}
