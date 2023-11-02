import { Component, OnInit } from '@angular/core';
import { MitgliederService } from 'src/app/mitglieder/services/mitglieder.service';

@Component({
  selector: 'app-mitglieder-liste',
  templateUrl: './mitglieder-liste.component.html',
  styleUrls: ['./mitglieder-liste.component.scss'],
})
export class MitgliederListeComponent  implements OnInit {

  data$ = this.mitgliederService.getAllMitglieder();

  constructor(
    private mitgliederService: MitgliederService,
  ) { }

  ngOnInit() {}

}
