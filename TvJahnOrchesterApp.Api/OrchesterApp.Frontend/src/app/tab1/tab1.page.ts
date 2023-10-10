import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss']
})
export class Tab1Page {

  public data$!: Observable<any>;
  public loading = false;

  constructor(
    private http: HttpClient
  ) {}

  loadData(){
    this.loading = true;
    this.data$ = this.http.get("https://localhost:44331/api/OrchesterMitglied/GetAll");
  }
}
