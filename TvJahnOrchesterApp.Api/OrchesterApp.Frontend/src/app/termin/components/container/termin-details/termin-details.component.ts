import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-termin-details',
  templateUrl: './termin-details.component.html',
  styleUrls: ['./termin-details.component.scss'],
})
export class TerminDetailsComponent  implements OnInit {

  activeTab!: string;

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.activeTab = this.route.snapshot.params['activeTab']
  }

}
