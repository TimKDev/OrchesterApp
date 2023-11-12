import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { GetAllMitgliederResponse } from 'src/app/mitglieder/interfaces/get-all-mitglieder-response';
import { MitgliederService } from 'src/app/mitglieder/services/mitglieder.service';

@Component({
  selector: 'app-mitglieder-liste',
  templateUrl: './mitglieder-liste.component.html',
  styleUrls: ['./mitglieder-liste.component.scss'],
})
export class MitgliederListeComponent implements OnInit{

  data$!: Observable<GetAllMitgliederResponse[]>;

  constructor(
    private mitgliederService: MitgliederService,
    private refreshService: RefreshService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  ionViewWillEnter(){
    if(!this.refreshService.needsRefreshing('MitgliederListeComponent')) return;
    this.loadData();
  }

  loadData(){
    this.data$ = this.mitgliederService.getAllMitglieder();
  }

}
