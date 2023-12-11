import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { combineLatest, map } from 'rxjs';
import { RolesService } from 'src/app/authentication/services/roles.service';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { DropdownService } from 'src/app/core/services/dropdown.service';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { GetAllTermineResponse } from 'src/app/termin/interfaces/get-all-termine-response';
import { TerminService } from 'src/app/termin/services/termin.service';

@Component({
  selector: 'app-history-termin-liste',
  templateUrl: './history-termin-liste.component.html',
  styleUrls: ['./history-termin-liste.component.scss'],
})
export class HistoryTerminListeComponent  implements OnInit {

  data!: GetAllTermineResponse[];
  displayedData!: GetAllTermineResponse[];

  canCreateNewTermin = this.rolesService.isCurrentUserAdmin || this.rolesService.isCurrentUserVorstand;
  
  terminArtenDropdownValues!: DropdownItem[]; 
  terminStatusDropdownValues!: DropdownItem[]; 
  responseDropdownValues!: DropdownItem[]; 

  constructor(
    private terminService: TerminService,
    private refreshService: RefreshService,
    private us: Unsubscribe,
    private rolesService: RolesService,
    private dropdownService: DropdownService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  ionViewWillEnter(){
    if(!this.refreshService.needsRefreshing('TerminListeComponent')) return;
    this.loadData();
  }

  loadData(refreshEvent: any = null){
    this.us.autoUnsubscribe(
      combineLatest([
        this.terminService.getAllTerminsHistory(),
        this.dropdownService.getDropdownElements('TerminArten'),
        this.dropdownService.getDropdownElements('TerminStatus'),
        this.dropdownService.getDropdownElements('Rückmeldungsart'),
      ]).pipe(
        map(([data, terminArtenDropdown, terminStatusDropdown, RückmeldungsArtenDropdown]) => ({data, terminArtenDropdown, terminStatusDropdown, RückmeldungsArtenDropdown}))
      )
      ).subscribe(res => {
      this.data = res.data;
      this.displayedData = res.data;
      this.terminArtenDropdownValues = res.terminArtenDropdown;
      this.terminStatusDropdownValues = res.terminStatusDropdown;
      this.responseDropdownValues = res.RückmeldungsArtenDropdown;
      if(refreshEvent) refreshEvent.target.complete();
    });
  }

  public handleRefresh(event: any){
    this.loadData(event);
  }

  public search(event: any) {
    let searchString = (event.detail.value as string).toLowerCase();
    this.displayedData = this.data.filter(e => (e.name).toLowerCase().includes(searchString));
  }

  public openTermin(terminId: string) {
    this.router.navigate(['tabs', 'termin', 'details', terminId]);
  }

}
