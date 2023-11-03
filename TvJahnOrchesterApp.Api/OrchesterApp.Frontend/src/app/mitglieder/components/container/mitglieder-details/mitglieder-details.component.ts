import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { GetSpecificMitgliederResponse } from 'src/app/mitglieder/interfaces/get-specific-mitglieder-response';
import { MitgliederService } from 'src/app/mitglieder/services/mitglieder.service';

@Component({
  selector: 'app-mitglieder-details',
  templateUrl: './mitglieder-details.component.html',
  styleUrls: ['./mitglieder-details.component.scss'],
})
export class MitgliederDetailsComponent  implements OnInit {

  data$!: Observable<GetSpecificMitgliederResponse>;
  mitgliedsId!: string;

  constructor(
    private mitgliederService: MitgliederService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.mitgliedsId = this.route.snapshot.params["mitgliedsId"];
    this.data$ = this.mitgliederService.getSpecificMitglied(this.mitgliedsId);
  }

}
