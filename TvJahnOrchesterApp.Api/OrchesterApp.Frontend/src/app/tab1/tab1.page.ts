import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss']
})
export class Tab1Page {

  constructor(
    private http: HttpClient
  ) {}

  loadData(){
    console.log("Button");
    this.http.get("https://localhost:44331/api/OrchesterMitglied/GetAll").subscribe(data => {
      debugger;
    })
  }
}
