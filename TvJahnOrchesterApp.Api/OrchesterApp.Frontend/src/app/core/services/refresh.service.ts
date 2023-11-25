import { Injectable } from '@angular/core';

export type ComponentsToRefresh = 'MitgliederListeComponent' | 'TerminListeComponent'

@Injectable({
  providedIn: 'root'
})
export class RefreshService {

  private refreshNecessary: Record<ComponentsToRefresh, boolean> = {
    MitgliederListeComponent: false,
    TerminListeComponent: false
  }; 

  constructor() { }

  refreshComponent(component: ComponentsToRefresh){
    this.refreshNecessary[component] = true;
  }

  needsRefreshing(component: ComponentsToRefresh){
    let result = this.refreshNecessary[component];
    this.refreshNecessary[component] = false;
    return result;
  }


}
