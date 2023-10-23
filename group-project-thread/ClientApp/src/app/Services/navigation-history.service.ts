import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavigationHistoryService {

  private pageInHistoryCounter : number = 0;
  private navigateToUserPage = false;
  private navigateToMainPage = false;
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

  public setNavigateToUserPage() : void{
    this.navigateToUserPage = true;
  }

  public getNavigateToUserPage() : boolean{
    return this.navigateToUserPage;
  }

  public resetNavigateToUserPage(){
    this.navigateToUserPage = false;
  }

  public setNavigateToMainPage() : void{
    this.navigateToMainPage = true;
  }

  public getNavigateToMainPage() : boolean{
    return this.navigateToMainPage;
  }

  public resetNavigateToMainPage(){
    this.navigateToMainPage = false;
  }

}
