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

  getResponseColorClass(responseText: string): string {
    if (!this.data || !this.data.terminRückmeldung) return '';
    
    if (responseText !== 'Nicht Zurückgemeldet') {
      return '';
    }

    const now = new Date();
    const oneWeekFromNow = new Date(now.getTime() + 7 * 24 * 60 * 60 * 1000);
    const terminStart = new Date(this.data.termin.startZeit);
    
    // Red if deadline passed OR termin starts in less than one week
    if ((this.data.termin.fristAsDate && new Date(this.data.termin.fristAsDate) < now) || terminStart < oneWeekFromNow) {
      return 'red';
    }
    if (this.data.termin.ersteWarnungVorFristAsDate && new Date(this.data.termin.ersteWarnungVorFristAsDate) < now) {
      return 'yellow';
    }
    return '';
  }

  getResponseText(responseText: string): string {
    if (!this.data || !this.data.terminRückmeldung) return responseText;
    
    if (responseText !== 'Nicht Zurückgemeldet') {
      return responseText;
    }

    const now = new Date();
    if (this.data.termin.ersteWarnungVorFristAsDate && new Date(this.data.termin.ersteWarnungVorFristAsDate) < now && 
        (!this.data.termin.fristAsDate || new Date(this.data.termin.fristAsDate) >= now)) {
      return 'Frist läuft bald ab';
    }
    return responseText;
  }

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
    this.updateTermin(data);
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

  private updateTermin(data: UpdateTerminModal) {
    if (!data.dokumente) {
      data.dokumente = [];
    }

    this.us.autoUnsubscribe(
      this.terminService.updateTerminDetails({...data, dokumente: data.dokumente.map(d => d.name)})).pipe(
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

  getDeadlineDate(termin: any): Date | null {
    if (!termin.frist) return null;
    
    const fristDays = this.parseTimeSpanToDays(termin.frist);
    if (fristDays === null) return null;
    
    const deadline = new Date(termin.startZeit);
    deadline.setDate(deadline.getDate() - fristDays);
    return deadline;
  }

  private parseTimeSpanToDays(timeSpan: string): number | null {
    // Parse TimeSpan format "d.hh:mm:ss" or "hh:mm:ss"
    const parts = timeSpan.split('.');
    if (parts.length === 2) {
      // Format: "d.hh:mm:ss"
      return parseInt(parts[0], 10);
    }
    return null;
  }
}
