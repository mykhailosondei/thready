import {Component, ElementRef, HostListener, OnInit, ViewChild} from '@angular/core';
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons/faMagnifyingGlass";
import {RecommendationService} from "../../Services/recommendation.service";
import {IndexedWordDTO} from "../../models/indexedWordDTO";
import {BehaviorSubject, takeUntil} from "rxjs";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {Router} from "@angular/router";

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

  @ViewChild('recommendationBarMargin') recommendationBarMargin: ElementRef;
  @ViewChild('stickWrap') stickyWrap : ElementRef;
  public stickyWrapHeight : number;
  sideBarFixed: boolean = false;
  constructor(private recommendationService : RecommendationService, private userService : UserService,
              private router : Router) {
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
  @HostListener('window:scroll', ['$event']) onscroll(){
    const scrollTop = window.scrollY;
    console.log(scrollTop)
    if (scrollTop > 0) {
      this.recommendationBarMargin.nativeElement.style.marginTop = scrollTop + 'px';
      this.stickyWrap.nativeElement.style.bottom = `-${516.8}px`
    } else {
      this.recommendationBarMargin.nativeElement.style.marginTop = '0';
      this.stickyWrap.nativeElement.style.top = `-${300.4}px`
    }

  }

  navigateToConnectPeople() {
    this.router.navigate(['connect-people']);
  }
}
