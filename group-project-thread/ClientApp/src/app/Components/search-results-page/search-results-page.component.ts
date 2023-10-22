import {Component, Input, OnInit, SimpleChanges} from '@angular/core';
import {faArrowLeftLong, faSpinner} from "@fortawesome/free-solid-svg-icons";
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons/faMagnifyingGlass";
import {PostDTO} from "../../models/post/postDTO";
import {BehaviorSubject, takeUntil} from "rxjs";
import {SearchService} from "../../Services/search.service";
import {HttpResponse} from "@angular/common/http";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {DataLoadingService} from "../../Services/data-loading.service";
import {Endpoint} from "../side-navbar/side-navbar.component";
import {Tab} from "../../models/enums/Tab";
import {Q} from "@angular/cdk/keycodes";
import {Location} from "@angular/common";
import {faCircleNotch} from "@fortawesome/free-solid-svg-icons/faCircleNotch";
import {NavigationHistoryService} from "../../Services/navigation-history.service";

@Component({
  selector: 'app-search-results-page',
  templateUrl: './search-results-page.component.html',
  styleUrls: ['./search-results-page.component.scss', '../../../assets/ContentFrame.scss', '../../../assets/navigation-bar.scss']
})
export class SearchResultsPageComponent implements OnInit{

  protected readonly Endpoint = Endpoint;
  public query : string = "";
  public currentUser! : UserDTO;
  public isFollowing : boolean;
  public matchingPosts$ = new BehaviorSubject<PostDTO[]>([]);
  public matchingUsers$ = new BehaviorSubject<PageUserDTO[]>([]);
  private postsToLoadLowerCount : number = 0;
  private postsToLoadUpperCount : number = 10;
  private readonly postsPerPage : number = 10;
  private peopleToLoadLowerCount : number = 0;
  private peopleToLoadUpperCount : number = 10;
  private readonly peoplePerPage : number = 10;
  private allPostsLoaded : boolean = false;
  private allPeopleLoaded : boolean = false;
  public selectedTab : Tab;
  public loading : boolean;
  private queries : string[] = [];

  constructor(private userService : UserService, private route : ActivatedRoute,
              private router : Router, private dataLoadingService : DataLoadingService,
              private location: Location, private historyOfPages : NavigationHistoryService) {

  }

  ngOnInit() {
    this.route.queryParams.subscribe((params) =>{
      this.query = params['q'];
      if (this.query == undefined){
        this.router.navigate(['explore']);
      }
      if (params['f'] === "user"){
        this.selectedTab = Tab.SecondTab;
      }
      else if (params['f'] === undefined || params['f'] === ''){
        this.selectedTab = Tab.FirstTab;
      }
    });
    this.getCurrentUser();
    this.searchByQuery();
  }

  searchByQuery() {
    if(this.query === ""){
      return;
    }
    else if (this.query == this.queries.pop()){
      this.queries.push(this.query);
      return;
    }
    this.queries.push(this.query);
    this.matchingPosts$ = new BehaviorSubject<PostDTO[]>([]);
    this.matchingUsers$ = new BehaviorSubject<PageUserDTO[]>([]);
    if (this.selectedTab == Tab.FirstTab){
      this.allPostsLoaded = false;
      this.postsToLoadLowerCount = 0;
      this.postsToLoadUpperCount = 10;
      this.router.navigate(['search'], { queryParams: { q: this.query } });
      this.dataLoadingService.loadInitialPosts(this.query, this.postsToLoadLowerCount,
        this.postsToLoadUpperCount, this.matchingPosts$);
      this.dataLoadingService.loadInitialPeople(this.query, 0, 3, this.matchingUsers$);
    }
    else {
      this.allPeopleLoaded = false;
      this.peopleToLoadLowerCount = 0;
      this.peopleToLoadUpperCount = 10;
      this.router.navigate(['search'], { queryParams: { q: this.query, f: 'user' } });
      this.dataLoadingService.loadMorePeople(this.allPeopleLoaded, this.query, this.peopleToLoadLowerCount,
        this.peopleToLoadUpperCount, this.matchingUsers$);
    }
    console.log(this.queries);
  }
  navigateToTopSearch() {
    this.selectedTab = Tab.FirstTab;
    this.searchByQuery();
  }

  navigateToUserSearch() {
    this.selectedTab  = Tab.SecondTab;
    this.searchByQuery();
  }

  onScrollPostsPage(){
    this.postsToLoadLowerCount += this.postsPerPage;
    this.postsToLoadUpperCount += this.postsPerPage;
    this.dataLoadingService.loadMorePosts(this.allPostsLoaded, this.query, this.postsToLoadLowerCount,
      this.postsToLoadUpperCount, this.matchingPosts$);
  }

  onScrollUsersPage(){
    this.peopleToLoadLowerCount += this.peoplePerPage;
    this.peopleToLoadUpperCount += this.peoplePerPage;
    this.dataLoadingService.loadMorePeople(this.allPeopleLoaded, this.query, this.peopleToLoadLowerCount,
      this.peopleToLoadUpperCount, this.matchingUsers$);
  }

  onQueryChanged(query: string) {
    this.query = query;
  }

  getTrend(query : string){
    this.query = query;
    this.searchByQuery();
  }

  getCurrentUser(): void{
    this.userService.getCurrentUser()
      .subscribe( (response) =>{
        if (response.body != null){
          this.currentUser = response.body;
        }
      });
  }
  amIFollowing(id : number): boolean{
    if (id == this.currentUser.id) return true;
    return this.currentUser.followingIds.includes(id);
  }
  isCurrentUser(id : number){
    return id == this.currentUser.id;
  }



  backToMainPage() {
    this.queries.pop();
    const query : string | undefined = this.queries.pop();
    if (query != undefined){
      this.query = query
      this.router.navigate(['search'], {queryParams : {q : this.query}}).then( () => {
        this.route.queryParams.subscribe((params) =>{
          this.query = params['q'];
          this.searchByQuery();
        });
      })
    }
    else {
      this.router.navigate(['explore'])
      return;
    }

  }

  protected readonly faCircleNotch = faCircleNotch;
  protected readonly faSpinner = faSpinner;
}
