import {Component, Input, OnInit} from '@angular/core';
import {faArrowLeftLong} from "@fortawesome/free-solid-svg-icons";
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons/faMagnifyingGlass";
import {PostDTO} from "../../models/post/postDTO";
import {BehaviorSubject, takeUntil} from "rxjs";
import {SearchService} from "../../Services/search.service";
import {HttpResponse} from "@angular/common/http";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
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
  public isAllPage : boolean;
  public isUsersPage : boolean;

  constructor(private searchService : SearchService, private userService : UserService, private route : ActivatedRoute,
              private router : Router) {

  }

  ngOnInit() {
    this.route.queryParams.subscribe((params) =>{
      this.query = params['q'];
      if (params['f'] === "user"){
        this.isUsersPage = true;
      }
      else if (params['f'] === undefined || params['f'] === ''){
        this.isAllPage = true;
      }
    });
    this.getCurrentUser();
    if (this.isAllPage){
      this.loadPostsInitially();
      this.loadPeopleInitially();
    }
    if (this.isUsersPage){
      console.log('bob')
      this.loadMorePeople();
    }
  }

  searchByQuery() {
    if(this.query === ""){
      return;
    }
    if (this.isAllPage){
      this.matchingPosts$ = new BehaviorSubject<PostDTO[]>([]);
      this.matchingUsers$ = new BehaviorSubject<PageUserDTO[]>([]);
      this.allPostsLoaded = false;
      this.postsToLoadLowerCount = 0;
      this.postsToLoadUpperCount = 10;
      this.router.navigate(['search'], { queryParams: { q: this.query } });
      this.loadPostsInitially();
      this.loadPeopleInitially();
    }
    else {
      this.matchingUsers$ = new BehaviorSubject<PageUserDTO[]>([]);
      this.allPeopleLoaded = false;
      this.peopleToLoadLowerCount = 0;
      this.peopleToLoadUpperCount = 10;
      this.router.navigate(['search'], { queryParams: { q: this.query, f: 'user' } });
      this.loadMorePeople();
    }

  }

  loadPeopleInitially(){
    this.searchService.getUsers(this.query, 0, 3).subscribe(
      (users : HttpResponse<PageUserDTO[]>) => {
        this.matchingUsers$.next(users.body || []);
      },
      (error) => {
        this.allPeopleLoaded = true;
      }
    );
  }
  loadPostsInitially(){
    this.searchService.getPosts(this.query, this.postsToLoadLowerCount, this.postsToLoadUpperCount).subscribe(
      (posts : HttpResponse<PostDTO[]>) => {
        this.matchingPosts$.next(posts.body || []);
      },
      () =>
        this.allPostsLoaded = true
    );
  }

  loadMorePeople(){
    if (this.allPeopleLoaded) return;
    this.searchService.getUsers(this.query, this.peopleToLoadLowerCount, this.peopleToLoadUpperCount).subscribe(
      (posts : HttpResponse<PageUserDTO[]>) => {
        const currentUsers = this.matchingUsers$.getValue();
        const newUsers = posts.body || [];
        this.matchingUsers$.next([...currentUsers, ...newUsers]);
      },
      () => {
        this.allPeopleLoaded = true;
      }
    );
  }

  loadMorePosts(){
    if (this.allPostsLoaded) return;
    this.searchService.getPosts(this.query, this.postsToLoadLowerCount, this.postsToLoadUpperCount).subscribe(
      (posts : HttpResponse<PostDTO[]>) => {
        const currentPosts = this.matchingPosts$.getValue();
        const newPosts = posts.body || [];
        this.matchingPosts$.next([...currentPosts, ...newPosts]);
      },
      () => {
        this.allPostsLoaded = true;
      }
    );
  }
  navigateToTopSearch() {
    this.isUsersPage =false;
    this.isAllPage = true;
    this.searchByQuery();
  }

  navigateToUserSearch() {
    this.isUsersPage =true;
    this.isAllPage = false;
    this.searchByQuery();
  }

  onScrollPostsPage(){
    this.postsToLoadLowerCount += this.postsPerPage;
    this.postsToLoadUpperCount += this.postsPerPage;
    this.loadMorePosts()
  }

  onScrollUsersPage(){
    this.peopleToLoadLowerCount += this.peoplePerPage;
    this.peopleToLoadUpperCount += this.peoplePerPage;
    this.loadMorePosts()
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
