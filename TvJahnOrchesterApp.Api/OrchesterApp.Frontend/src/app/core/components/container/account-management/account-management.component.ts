import { Component, ElementRef, ViewChild } from '@angular/core';
import { GetAdminInfoResponse } from '../../../interfaces/get-admin-info-response';
import { AccountManagementService } from 'src/app/core/services/account-management.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-account-management',
  templateUrl: './account-management.component.html',
  styleUrls: ['./account-management.component.scss'],
})
export class AccountManagementComponent {

  data!: GetAdminInfoResponse[];
  displayedData!: GetAdminInfoResponse[];
  subscription!: Subscription;

  @ViewChild('searchBar') searchBar!: any;

  constructor(
    private accountManagementService: AccountManagementService,
  ) { }

  ionViewWillEnter() {
    if(this.searchBar) this.searchBar.value = ""; 
    this.subscription = this.accountManagementService.getManagementInfos().subscribe(data => {
      this.data = data;
      this.displayedData = data;
    });
  }

  ionViewDidLeave(){
    this.subscription.unsubscribe();
  }

  search(event: any) {
    let searchString = (event.detail.value as string).toLowerCase();
    this.displayedData = this.data.filter(e => e.orchesterMitgliedsName.toLowerCase().includes(searchString));
  }
}
