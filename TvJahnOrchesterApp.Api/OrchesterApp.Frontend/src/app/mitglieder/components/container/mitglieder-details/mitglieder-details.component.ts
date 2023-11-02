import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MitgliederService } from 'src/app/mitglieder/services/mitglieder.service';

@Component({
  selector: 'app-mitglieder-details',
  templateUrl: './mitglieder-details.component.html',
  styleUrls: ['./mitglieder-details.component.scss'],
})
export class MitgliederDetailsComponent  implements OnInit {

  constructor(
    private mitgliederService: MitgliederService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {}

}
