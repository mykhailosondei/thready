import {Component, OnInit} from '@angular/core';
import {faArrowLeftLong} from "@fortawesome/free-solid-svg-icons";
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons/faMagnifyingGlass";
import {PostDTO} from "../../models/post/postDTO";
import {BehaviorSubject} from "rxjs";
import {SearchService} from "../../Services/search.service";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {DataLoadingService} from "../../Services/data-loading.service";
import {Tab} from "../../models/enums/Tab";
import {Q} from "@angular/cdk/keycodes";

@Component({
  selector: 'app-search-results-page',
  templateUrl: './search-results-page.component.html',
  styleUrls: ['./search-results-page.component.scss', '../../../assets/ContentFrame.scss', '../../../assets/navigation-bar.scss']
})
export class SearchResultsPageComponent implements OnInit{

  protected readonly faArrowLeftLong = faArrowLeftLong;
  protected readonly faMagnifyingGlass = faMagnifyingGlass;
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

  constructor(private searchService : SearchService, private userService : UserService, private route : ActivatedRoute,
              private router : Router, private dataLoadingService : DataLoadingService) {

  }

  ngOnInit() {
    this.route.queryParams.subscribe((params) =>{
      this.query = params['q'];
      if (params['f'] === "user"){
        this.selectedTab = Tab.SecondTab;
      }
      else if (params['f'] === undefined || params['f'] === ''){
        this.selectedTab = Tab.FirstTab;
      }
    });
    this.getCurrentUser();
    if (this.selectedTab == Tab.FirstTab){
      this.dataLoadingService.loadInitialPosts(this.query, this.postsToLoadLowerCount,
        this.postsToLoadUpperCount, this.matchingPosts$);
      this.dataLoadingService.loadInitialPeople(this.query, 0, 3,
        this.matchingUsers$);
    }
    if (this.selectedTab == Tab.SecondTab){
      this.dataLoadingService.loadMorePeople(this.allPeopleLoaded, this.query, this.peopleToLoadLowerCount,
        this.peopleToLoadUpperCount, this.matchingUsers$);
    }
  }
  

  searchByQuery() {
    if(this.query === ""){
      return;
    }
    if (this.selectedTab == Tab.FirstTab){
      this.matchingPosts$ = new BehaviorSubject<PostDTO[]>([]);
      this.matchingUsers$ = new BehaviorSubject<PageUserDTO[]>([]);
      this.allPostsLoaded = false;
      this.postsToLoadLowerCount = 0;
      this.postsToLoadUpperCount = 10;
      this.router.navigate(['search'], { queryParams: { q: this.query } });
      this.dataLoadingService.loadInitialPosts(this.query, this.postsToLoadLowerCount,
        this.postsToLoadUpperCount, this.matchingPosts$);
      this.dataLoadingService.loadInitialPeople(this.query, 0, 3, this.matchingUsers$);
    }
    else {
      this.matchingUsers$ = new BehaviorSubject<PageUserDTO[]>([]);
      this.allPeopleLoaded = false;
      this.peopleToLoadLowerCount = 0;
      this.peopleToLoadUpperCount = 10;
      this.router.navigate(['search'], { queryParams: { q: this.query, f: 'user' } });
      this.dataLoadingService.loadMorePeople(this.allPeopleLoaded, this.query, this.peopleToLoadLowerCount,
        this.peopleToLoadUpperCount, this.matchingUsers$);
    }

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

  goBack() {
    this.router.navigate(['mainPage']);
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
}
