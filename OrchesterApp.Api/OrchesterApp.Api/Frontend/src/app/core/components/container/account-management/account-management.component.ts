import { Component, ViewChild } from '@angular/core';
import { GetAdminInfoResponse } from '../../../interfaces/get-admin-info-response';
import { AccountManagementService } from 'src/app/core/services/account-management.service';
import { interval } from 'rxjs';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';

@Component({
  selector: 'app-account-management',
  templateUrl: './account-management.component.html',
  styleUrls: ['./account-management.component.scss'],
  providers: [Unsubscribe]
})
export class AccountManagementComponent {

  data!: GetAdminInfoResponse[];
  displayedData!: GetAdminInfoResponse[];

  @ViewChild('searchBar') searchBar!: any;

  constructor(
    private accountManagementService: AccountManagementService,
    private us: Unsubscribe
  ) {
  }

  ionViewWillEnter() {
    if(this.searchBar) this.searchBar.value = ""; 
    this.us.autoUnsubscribe(this.accountManagementService.getManagementInfos()).subscribe(data => {
      this.data = data;
      this.displayedData = data;
    });
  }

  ionViewDidLeave(){
    this.us.unsubscribe();
  }

  search(event: any) {
    let searchString = (event.detail.value as string).toLowerCase();
    this.displayedData = this.data.filter(e => e.orchesterMitgliedsName.toLowerCase().includes(searchString));
  }

  handleRefresh(event: any){
    this.us.autoUnsubscribe(this.accountManagementService.getManagementInfos()).subscribe(data => {
      this.data = data;
      this.displayedData = data;
      event.target.complete();
    });
  }
}
