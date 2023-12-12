import { Component, OnInit } from '@angular/core';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';

@Component({
  selector: 'app-termin-overview',
  templateUrl: './termin-overview.component.html',
  styleUrls: ['./termin-overview.component.scss'],
  providers: [Unsubscribe]
})
export class TerminOverviewComponent  implements OnInit {

  constructor() { }

  ngOnInit() {}

}
