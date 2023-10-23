import {Component, Input, OnInit, SimpleChanges} from '@angular/core';
import {faArrowLeftLong, faSpinner} from "@fortawesome/free-solid-svg-icons";
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons/faMagnifyingGlass";
import {PostDTO} from "../../models/post/postDTO";
import {BehaviorSubject, finalize, takeUntil} from "rxjs";
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
import {User} from "oidc-client";

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
  public loadingPosts : boolean = true;
  public loadingUsers : boolean;
  public noPeopleFound : boolean;
  public noPostsFound : boolean;
  private queries : string[] = [];
  public notFoundSmallText = 'Try searching for something else, or check your country search policy it could be protecting you\n' +
    '      from potentially sensitive content.';
  public notFoundHeaderText : string;

  constructor(private userService : UserService, private route : ActivatedRoute,
              private router : Router, private searchService : SearchService) {

  }

  ngOnInit() {
    this.route.queryParams.subscribe((params) =>{
    this.query = params['q'];
    if (this.query == undefined){
      this.router.navigate(['explore']);
    }
    if (params['f'] === "user"){
      this.selectedTab = 1;
    }
    else if (params['f'] === undefined || params['f'] === ''){
      this.selectedTab = 0;
    }
  });
    this.getCurrentUser();
    this.searchByQuery();
  }

  searchByQuery(initialTab? : Tab, changedTab? : Tab) {

    if(this.query === ""){
      return;
    }
    else if (this.query == this.queries[this.queries.length-1] && initialTab == changedTab){
      return;
    }
    this.notFoundHeaderText = `No results for "${this.query}"`
    if (this.query != this.queries[this.queries.length-1]){
      this.queries.push(this.query);
    }
    this.matchingPosts$ = new BehaviorSubject<PostDTO[]>([]);
    this.matchingUsers$ = new BehaviorSubject<PageUserDTO[]>([]);
    if (this.selectedTab == 0){
      this.allPostsLoaded = false;
      this.postsToLoadLowerCount = 0;
      this.postsToLoadUpperCount = 10;
      this.router.navigate(['search'], { queryParams: { q: this.query } });
      this.loadInitialPosts();
      this.loadInitialPeople();
      this.loadingPosts = false;
    }
    else {
      this.allPeopleLoaded = false;
      this.peopleToLoadLowerCount = 0;
      this.peopleToLoadUpperCount = 10;
      this.router.navigate(['search'], { queryParams: { q: this.query, f: 'user' } });
      this.loadMorePeople();
    }
  }

  public loadInitialPeople(){
    this.loadingUsers = true;
    this.noPeopleFound = false;
    this.searchService.getUsers(this.query, 0, 3)
      .pipe(finalize(() => this.loadingUsers = false))
      .subscribe(
      (users : HttpResponse<PageUserDTO[]>) => {
        if (users.body?.length == 0){
          this.noPeopleFound = true;
        }
        this.matchingUsers$.next(users.body || []);
      }
    );
  }

  public loadInitialPosts(){
    this.loadingPosts = true;
    this.noPostsFound = false;
    this.searchService.getPosts(this.query, this.postsToLoadLowerCount, this.postsToLoadUpperCount)
      .pipe(finalize(() => this.loadingPosts = false))
      .subscribe(
      (posts : HttpResponse<PostDTO[]>) => {
        if (posts.body?.length == 0){
          this.noPostsFound = true;
        }
        this.matchingPosts$.next(posts.body || []);
      }
    );
  }

  loadMorePeople(){
    if (this.allPeopleLoaded) return;
    this.loadingUsers = true;
    this.noPeopleFound = false;
    this.searchService.getUsers(this.query, this.peopleToLoadLowerCount, this.peopleToLoadUpperCount)
      .pipe(finalize(() =>
        this.loadingUsers = false))
      .subscribe(
        (users : HttpResponse<PageUserDTO[]>) => {
          const newUsers = users.body || [];
          if (newUsers.length == 0){
            this.noPeopleFound = true;
            this.allPeopleLoaded = true;
            return;
          }
          const currentUsers = this.matchingUsers$.getValue();
          this.matchingUsers$.next([...currentUsers, ...newUsers])
        }
      );
  }

  loadMorePosts(){
    if (this.allPostsLoaded) return;
    this.loadingPosts = true;
    this.noPostsFound = false;
    this.searchService.getPosts(this.query, this.postsToLoadLowerCount, this.postsToLoadUpperCount)
      .pipe(
        finalize(() => {
          this.loadingPosts = false;
        })
      )
      .subscribe(
      (posts : HttpResponse<PostDTO[]>) => {
        const newPosts = posts.body || [];
        if (newPosts.length == 0){
          this.allPostsLoaded = true;
          this.noPostsFound = true;
          return;
        }
        const currentPosts = this.matchingPosts$.getValue();
        this.matchingPosts$.next([...currentPosts, ...newPosts]);
      }
    );
  }


  navigateToTopSearch() {
    this.selectedTab = 0;
    this.searchByQuery(1, 0);
    this.router.navigate(['search'], {queryParams : {q : this.query}})
  }

  navigateToUserSearch() {
    this.selectedTab  = 1;
    this.searchByQuery(0, 1);
    this.router.navigate(['search'], {queryParams : {q : this.query, f : 'user'}})
  }

  onScrollPostsPage(){
    this.postsToLoadLowerCount += this.postsPerPage;
    this.postsToLoadUpperCount += this.postsPerPage;
    this.loadMorePosts();
  }

  onScrollUsersPage(){
    this.peopleToLoadLowerCount += this.peoplePerPage;
    this.peopleToLoadUpperCount += this.peoplePerPage;
    this.loadMorePeople();
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
