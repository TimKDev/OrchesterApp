import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, combineLatest, map } from 'rxjs';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { DropdownService } from 'src/app/core/services/dropdown.service';
import { GetSpecificMitgliederResponse } from 'src/app/mitglieder/interfaces/get-specific-mitglieder-response';
import { MitgliederService } from 'src/app/mitglieder/services/mitglieder.service';

@Component({
  selector: 'app-mitglieder-details',
  templateUrl: './mitglieder-details.component.html',
  styleUrls: ['./mitglieder-details.component.scss'],
})
export class MitgliederDetailsComponent  implements OnInit {

  data$!: Observable<{data: GetSpecificMitgliederResponse, instrumentDropdown: DropdownItem[], notenStimmeDropdown: DropdownItem[]}>;
  mitgliedsId!: string;

  constructor(
    private mitgliederService: MitgliederService,
    private route: ActivatedRoute,
    private dropdownService: DropdownService
  ) { }

  ngOnInit() {
    this.mitgliedsId = this.route.snapshot.params["mitgliedsId"];
    this.data$ = combineLatest([
      this.mitgliederService.getSpecificMitglied(this.mitgliedsId), 
      this.dropdownService.getDropdownElements('Instrument'),
      this.dropdownService.getDropdownElements('Notenstimme') 
    ]).pipe(map(([data, instrumentDropdown, notenStimmeDropdown]) => ({data, instrumentDropdown, notenStimmeDropdown})));
  }

}
