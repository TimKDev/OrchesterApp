import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {AlertController, ModalController} from '@ionic/angular';
import {NEVER, Observable, catchError, tap, switchMap, of, combineLatest} from 'rxjs';
import {RefreshService} from 'src/app/core/services/refresh.service';
import {TerminDetailsResponse} from 'src/app/termin/interfaces/termin-details-response';
import {TerminService} from 'src/app/termin/services/termin.service';
import {UpdateTerminModalComponent} from '../update-termin-modal/update-termin-modal.component';
import {Unsubscribe} from 'src/app/core/helper/unsubscribe';
import {TerminResponseModalComponent} from '../termin-response-modal/termin-response-modal.component';
import {UpdateTerminResponseRequest} from 'src/app/termin/interfaces/update-termin-response-request';
import {UpdateTerminModal} from 'src/app/termin/interfaces/update-termin-request';
import {RolesService} from 'src/app/authentication/services/roles.service';
import {confirmDialog} from 'src/app/core/helper/confirm';
import {FileUploadService} from 'src/app/core/services/file-upload.service';

@Component({
  selector: 'app-termin-details',
  templateUrl: './termin-details.component.html',
  styleUrls: ['./termin-details.component.scss'],
  providers: [Unsubscribe]
})
export class TerminDetailsComponent implements OnInit {

  activeTab!: string;
  canEditTermin = this.rolesService.isCurrentUserAdmin || this.rolesService.isCurrentUserVorstand;
  terminId!: string;
  data$!: Observable<TerminDetailsResponse>;
  isRefreshing = false;
  dateNow = new Date();
  noten = '';
  uniform = '';
  currentlyLoading = false;
  uploadedFiles: File[] = [];
  isUploadingFiles = false;

  private data?: TerminDetailsResponse;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private terminService: TerminService,
    private refreshService: RefreshService,
    private modalCtrl: ModalController,
    private us: Unsubscribe,
    private rolesService: RolesService,
    public alertController: AlertController,
    public fileUploadService: FileUploadService
  ) {
  }

  ngOnInit() {
    this.activeTab = this.route.snapshot.params['activeTab'];
    this.terminId = this.route.snapshot.params['terminId'];
    this.loadData();
  }

  loadData(refreshEvent: any = null) {
    if (this.currentlyLoading) return;
    this.currentlyLoading = true;
    this.data$ = this.terminService.getTerminDetails(this.terminId).pipe(
      tap((data) => {
        data.termin.startZeit = new Date(data.termin.startZeit);
        data.termin.endZeit = new Date(data.termin.endZeit);
        this.data = data;
        if (refreshEvent) {
          refreshEvent.target.complete();
          this.isRefreshing = false;
        }
        this.currentlyLoading = false;
      }),
      catchError(() => {
        this.currentlyLoading = false;
        return NEVER
      })
    );
  }

  ionViewWillEnter() {
    if (!this.refreshService.needsRefreshing('TerminDetails')) return;
    this.loadData();
  }

  public handleRefresh(event: any) {
    this.isRefreshing = true;
    this.loadData(event);
  }

  isActionSheetOpen = false;

  setOpen(isOpen: boolean) {
    this.isActionSheetOpen = isOpen;
  }

  public actionSheetButtons = [
    {
      text: 'Rückmeldungen und Anwesenheit',
      data: {
        action: 'share',
      },
      handler: () => this.navigateToTerminReturnMessages()
    },
    {
      text: 'Lösche Termin',
      role: 'destructive',
      handler: () => this.deleteTermin()

    },
    {
      text: 'Zurück',
      role: 'cancel',
      data: {
        action: 'cancel',
      },
    },
  ];

  public async openResponseModal(dataTermin: TerminDetailsResponse) {
    const modal = await this.modalCtrl.create({
      component: TerminResponseModalComponent,
      componentProps: {
        "dataTermin": dataTermin
      }
    });
    modal.present();
    const {data, role} = await modal.onWillDismiss();
    if (role === 'cancel') return;
    this.updateResponse(data);
  }

  public async openUpdateModal(dataTermin: TerminDetailsResponse) {
    const modal = await this.modalCtrl.create({
      component: UpdateTerminModalComponent,
      componentProps: {
        "dataTermin": dataTermin
      }
    });
    modal.present();
    const {data, role} = await modal.onWillDismiss();
    if (role === 'cancel') return;
    data.terminId = this.terminId;
    
    // Check if termin is today or in the future
    const terminStartDate = new Date(data.startZeit);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    terminStartDate.setHours(0, 0, 0, 0);
    
    if (terminStartDate >= today) {
      // Ask user if emails should be sent
      const shouldEmailBeSend = await this.askForEmailNotification();
      this.updateTermin(data, shouldEmailBeSend);
    } else {
      // Termin is in the past, don't send emails
      this.updateTermin(data, false);
    }
  }

  public downloadFile(objectName: string): void {
    this.terminService.downloadFile(objectName).subscribe((blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = this.fileUploadService.revertGuidTransformation(objectName);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
      });
  }

  private updateTermin(data: UpdateTerminModal, shouldEmailBeSend: boolean) {
    if (!data.dokumente) {
      data.dokumente = [];
    }

    this.us.autoUnsubscribe(
      this.terminService.updateTerminDetails({...data, dokumente: data.dokumente.map(d => d.name), shouldEmailBeSend})).pipe(
      switchMap(() => {
        const filesToAdd = data.dokumente.filter(d => !!d.file);
        return filesToAdd.length == 0 ? of(true) : combineLatest(filesToAdd.map(d => this.fileUploadService.uploadFile(d.name, d.file!)));
      })
    )
      .subscribe(() => {
        this.loadData(null);
        this.refreshService.refreshComponent('TerminListeComponent');
        this.refreshService.refreshComponent('Dashboard');
      });
  }

  private async askForEmailNotification(): Promise<boolean> {
    const alert = await this.alertController.create({
      header: 'E-Mail Benachrichtigung',
      message: 'Möchten Sie die betroffenen Orchestermitglieder per E-Mail über diese Terminänderungen informieren?',
      buttons: [
        {
          text: 'Nein',
          role: 'cancel',
        },
        {
          text: 'Ja',
          role: 'confirm',
        },
      ]
    });
    
    await alert.present();
    const alertResult = await alert.onDidDismiss();
    return alertResult.role === 'confirm';
  }

  private updateResponse(data: UpdateTerminResponseRequest) {
    let dataWithTermin = {...data, terminId: this.terminId};
    this.us.autoUnsubscribe(this.terminService.updateTerminResponse(dataWithTermin)).subscribe(() => {
      this.loadData(null);
      this.refreshService.refreshComponent('TerminListeComponent');
      this.refreshService.refreshComponent('Dashboard');
    });
  }

  @confirmDialog("Achtung", "Möchten sie dieses Termin wirklich löschen? Falls der Termin abgesagt wurde und die Orchestermitglieder darüber informiert werden sollen, setzen sie lieber den Status auf 'Abgesagt'!")
  private deleteTermin() {
    this.refreshService.refreshComponent("TerminListeComponent");
    this.refreshService.refreshComponent('Dashboard');
    this.terminService.deleteTermin(this.terminId).subscribe(() => {
      this.router.navigate(['tabs', 'termin']);
    });
  }

  private navigateToTerminReturnMessages() {
    this.router.navigate(['tabs', 'termin', 'return-messages', this.terminId]);
  }
}
