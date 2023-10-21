import {Component, ElementRef, HostListener, Input, OnInit, ViewChild} from '@angular/core';
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons/faMagnifyingGlass";
import {RecommendationService} from "../../Services/recommendation.service";
import {IndexedWordDTO} from "../../models/indexedWordDTO";
import {BehaviorSubject, takeUntil} from "rxjs";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {Router} from "@angular/router";
import {NavigatorService} from "../../Services/navigator.service";

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

  constructor(private recommendationService : RecommendationService, private userService : UserService,
              private router : Router, public navigatorService : NavigatorService) {
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
          console.log(this.whoToFollow$)
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
}
