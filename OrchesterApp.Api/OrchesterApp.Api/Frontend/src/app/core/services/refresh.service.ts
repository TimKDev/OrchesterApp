import { Injectable } from '@angular/core';

export type ComponentsToRefresh = 'MitgliederListeComponent' | 'TerminListeComponent' | 'TerminDetails' | 'Anwesenheitsliste' | 'Dashboard';

@Injectable({
  providedIn: 'root'
})
export class RefreshService {

  private refreshNecessary: Record<ComponentsToRefresh, boolean> = {
    MitgliederListeComponent: false,
    TerminListeComponent: false,
    TerminDetails: false,
    Anwesenheitsliste: false,
    Dashboard: false
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
