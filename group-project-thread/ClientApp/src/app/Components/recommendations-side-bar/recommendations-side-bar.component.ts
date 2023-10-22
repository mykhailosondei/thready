import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons/faMagnifyingGlass";
import {RecommendationService} from "../../Services/recommendation.service";
import {IndexedWordDTO} from "../../models/indexedWordDTO";
import {BehaviorSubject} from "rxjs";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {Router} from "@angular/router";
import {NavigatorService} from "../../Services/navigator.service";
import {Tab} from "../../models/enums/Tab";
import {NavigationHistoryService} from "../../Services/navigation-history.service";
import {Location} from "@angular/common";

@Component({
  selector: 'app-recommendations-side-bar',
  templateUrl: './recommendations-side-bar.component.html',
  styleUrls: ['./recommendations-side-bar.component.scss', '../../../assets/PageUser.scss']
})
export class RecommendationsSideBarComponent implements OnInit{

  protected readonly faMagnifyingGlass = faMagnifyingGlass;
  public smallTrends$ = new BehaviorSubject<IndexedWordDTO[]>([]);
  public whoToFollow$ = new BehaviorSubject<PageUserDTO[]>([]);
  public currentUser : UserDTO;
  imagewidth: number = 40;
  @Input() showWhatsHappening : boolean = true;
  @Input() showWhoToFollow : boolean = true;
  @Input() showSearchbar : boolean = true;

  @Output() trendClicked : EventEmitter<string> = new EventEmitter<string>();
  @Output() userClicked : EventEmitter<string> = new EventEmitter<string>();

  constructor(private recommendationService : RecommendationService, private userService : UserService,
              private router : Router, public navigatorService : NavigatorService,
              private  historyOfPages : NavigationHistoryService) {
  }



  ngOnInit(): void {
    this.getCurrentUser();
    this.getSmallTrends();
    this.getWhoToFollow();
  }

  public getSmallTrends() {
    this.recommendationService.getSmallTrends()
      .subscribe( (response) => {
        if (response.body != null){
          this.smallTrends$.next( response.body || []);
        }
      } )
  }

  public getWhoToFollow() {
    this.recommendationService.getWhoToFollow()
      .subscribe( (response) => {
        if (response.body != null){
          this.whoToFollow$.next( response.body || []);
        }
      } )
  }

  getCurrentUser(): void{
    this.userService.getCurrentUser()
      .subscribe( (response) =>{
        if (response.body != null){
          this.currentUser = response.body;
        }
      });
  }

  navigateToConnectPeople() {
    this.router.navigate(['connect-people']);
  }

  protected readonly Tab = Tab;

  searchByWord(word: string) {
    this.trendClicked.emit(word);
    this.navigatorService.searchByWord(word);
  }

  navigateToUserPage(username : string) {
    this.userClicked.emit(username);
  }

  navigateToMayBeInteresting() {
    this.historyOfPages.IncrementPageInHistoryCounter();
    this.navigatorService.navigateToMayBeInterestingPage(Tab.FirstTab);
  }


}
