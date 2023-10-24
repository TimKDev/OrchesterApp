import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent  implements OnInit {
  
  @Input() header!: string;
  @Input() showBack = false;
  @Output() clickBack = new EventEmitter<void>();

  constructor() { }

  ngOnInit() {}

  cancel(){
    this.clickBack.emit();
  }

}
