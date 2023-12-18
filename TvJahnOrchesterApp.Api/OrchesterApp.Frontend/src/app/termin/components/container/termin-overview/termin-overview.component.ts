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
  selector: 'app-termin-overview',
  templateUrl: './termin-overview.component.html',
  styleUrls: ['./termin-overview.component.scss'],
  providers: [Unsubscribe]
})
export class TerminOverviewComponent  implements OnInit {

  data!: GetAllTermineResponse[];
  displayedData!: GetAllTermineResponse[];

  canCreateNewTermin = this.rolesService.isCurrentUserAdmin || this.rolesService.isCurrentUserVorstand;
  
  terminArtenDropdownValues!: DropdownItem[]; 
  terminStatusDropdownValues!: DropdownItem[]; 
  responseDropdownValues!: DropdownItem[]; 

  highlightedDates = [
    {
      date: '2023-12-05',
      textColor: '#09721b',
      backgroundColor: '#c8e5d0',
    },
    {
      date: '2023-12-10',
      textColor: '#09721b',
      backgroundColor: '#c8e5d0',
    },
    {
      date: '2023-12-20',
      textColor: '#09721b',
      backgroundColor: '#c8e5d0',
    },
    {
      date: '2023-12-23',
      textColor: 'rgb(68, 10, 184)',
      backgroundColor: 'rgb(211, 200, 229)',
    },
  ];

  today = new Date();

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
    this.loadData(null, false);
  }

  loadData(refreshEvent: any = null, useCache = true){
    this.us.autoUnsubscribe(
      combineLatest([
        this.terminService.getAllTerminsHistory(useCache),
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
    this.loadData(event, false);
  }

  public search(event: any) {
    let searchString = (event.detail.value as string).toLowerCase();
    this.displayedData = this.data.filter(e => (e.name).toLowerCase().includes(searchString));
  }

  public openTermin(terminId: string) {
    this.router.navigate(['tabs', 'termin', 'details', terminId]);
  }

}
