import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavigationHistoryService {

  private pageInHistoryCounter : number = 0
  constructor() { }

  public IncrementPageInHistoryCounter(){
    this.pageInHistoryCounter++;
  }
  public getPageInHistoryCounter() : number{
    return this.pageInHistoryCounter;
  }

  public resetCounter(){
    this.pageInHistoryCounter = 0;
  }
}
