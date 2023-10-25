import { Injectable } from '@angular/core';
import {Router} from "@angular/router";
import {Tab} from "../models/enums/Tab";
import {Location} from "@angular/common";
import {NavigationHistoryService} from "./navigation-history.service";

@Injectable({
  providedIn: 'root'
})
export class NavigatorService {
  constructor(private router : Router, private location: Location, private historyOfPages : NavigationHistoryService) { }

  public goBack(){
    this.location.back();
  }
  public openProfilePage($event : string){
    this.historyOfPages.resetCounter();
    this.router.navigate([$event, 'profile'])
  }
  public openFollowingPage($event : string){
    this.router.navigate([$event, 'following'])
  }
  public openFollowersPage($event : string){
    this.router.navigate([$event, 'followers'])
  }

  public openWhoToFollowPage($event : string){
    this.router.navigate(['connect-people'])
  }
  public openCreatorsForYouPage($event : string) {
    this.router.navigate(['creators-for-you'])
  }

  navigateToMayBeInterestingPage(currentTab : Tab) {
    currentTab = 0;
    this.router.navigate(['explore']);
  }

  navigateToTrendingPage(currentTab : Tab) {
    currentTab = 1;
    this.router.navigate(['trending']);
  }

  searchByWord(word: string) {
    this.router.navigate(['search'], {queryParams : {q : word}})
  }
  backToMainPage(){
    this.router.navigate(['mainPage']);
  }

  goBackToMainPage(){
    const pagesCount = this.historyOfPages.getPageInHistoryCounter();
    if (pagesCount == 0 || this.historyOfPages.getNavigateToMainPage()){
      this.historyOfPages.resetCounter();
      this.historyOfPages.resetNavigateToMainPage();
      this.backToMainPage();
      return;
    }
    this.historyOfPages.resetCounter();
    this.location.historyGo(-pagesCount);
  }
}

