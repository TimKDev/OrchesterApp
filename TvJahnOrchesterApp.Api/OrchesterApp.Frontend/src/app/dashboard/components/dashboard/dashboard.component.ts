import { Component, OnInit } from '@angular/core';
import { switchMap, timer } from 'rxjs';
import { AuthHttpClientService } from 'src/app/core/services/auth-http-client.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent  implements OnInit {

  constructor(private http: AuthHttpClientService) { }

  data$ = this.http.get('api/dashboard/nextTermins');

  ngOnInit() {}

}
