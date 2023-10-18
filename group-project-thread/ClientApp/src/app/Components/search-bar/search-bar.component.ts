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
import {error} from "@angular/compiler-cli/src/transformers/util";
@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss', '../../../assets/ContentFrame.scss', '../../../assets/navigation-bar.scss']
})
export class SearchBarComponent implements OnInit{

  protected readonly faArrowLeftLong = faArrowLeftLong;
  public query : string = "";
  public currentUser! : UserDTO;
  public isFollowing : boolean;
  public matchingPosts$ = new BehaviorSubject<PostDTO[]>([]);
  public matchingUsers$ = new BehaviorSubject<PageUserDTO[]>([]);
  private postsToLoadLowerCount = 0;
  private postsToLoadUpperCount = 10;
  private readonly postsPerPaged : number = 10;
  private allPostsLoaded = false;
  @Input() public isTrending : boolean;
  @Input() public isForYou : boolean = true;

  constructor(private searchService : SearchService, private userService : UserService) {
  }
  backToMainPaige() {

  }

  protected readonly faMagnifyingGlass = faMagnifyingGlass;

  searchByQuery() {
    if (this.query == ""){
      return;
    }
    this.loadInitialPosts();
    this.loadInitialPeople();
  }

  loadInitialPeople(){
    this.searchService.getUsers(this.query, this.postsToLoadLowerCount, this.postsToLoadUpperCount).subscribe(
      (users : HttpResponse<PageUserDTO[]>) => {
        this.matchingUsers$.next(users.body || []);
      },
      (error) => console.log(error.error)
    );
  }
  loadInitialPosts(){
    if (this.allPostsLoaded) return;
    this.searchService.getPosts(this.query, this.postsToLoadLowerCount, this.postsToLoadUpperCount).subscribe(
      (posts : HttpResponse<PostDTO[]>) => {
        this.matchingPosts$.next(posts.body || []);
      },
      (error) => console.log(error.error)
    );
  }

  loadMorePosts(){
    if (this.allPostsLoaded) return;
    this.searchService.getPosts(this.query, this.postsToLoadLowerCount, this.postsToLoadUpperCount).subscribe(
      (posts : HttpResponse<PostDTO[]>) => {
        const currentPosts = this.matchingPosts$.getValue();
        const newPosts = posts.body || [];
        if (newPosts.length == 0){
          this.allPostsLoaded = true;
        }
        this.matchingPosts$.next([...currentPosts, ...newPosts]);
      },
      (error) => console.log(error.error)
    );
  }

  onScroll(){
    this.postsToLoadLowerCount = this.postsToLoadLowerCount + this.postsPerPaged;
    this.postsToLoadUpperCount = this.postsToLoadUpperCount + this.postsPerPaged;
    this.loadMorePosts()
  }
  navigateToForYouPage() {

  }

  navigateToTendingPage() {

  }

  getCurrentUser(): void{
    this.userService.getCurrentUser()
      .subscribe( (response) =>{
        if (response.body != null){
          this.currentUser = response.body;
          console.log(this.currentUser)
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

  ngOnInit(): void {
    this.getCurrentUser();
  }

  navigateToUserSearch() {

  }
}
