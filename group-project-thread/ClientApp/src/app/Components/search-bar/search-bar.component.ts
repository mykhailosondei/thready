import {Component, Input} from '@angular/core';
import {faArrowLeftLong} from "@fortawesome/free-solid-svg-icons";
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons/faMagnifyingGlass";
import {PostDTO} from "../../models/post/postDTO";
import {BehaviorSubject} from "rxjs";
import {SearchServiceService} from "../../Services/search-service.service";
import {HttpResponse} from "@angular/common/http";
@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss', '../../../assets/ContentFrame.scss', '../../../assets/navigation-bar.scss']
})
export class SearchBarComponent {

  protected readonly faArrowLeftLong = faArrowLeftLong;
  public query : string = "";
  public matchingPosts$ = new BehaviorSubject<PostDTO[]>([]);
  public matchingUsers$ = new BehaviorSubject<PostDTO[]>([]);
  private postsToLoadLowerCount = 0;
  private postsToLoadUpperCount = 10;
  private readonly postsPerPaged : number = 10;
  @Input() public isTrending : boolean;
  @Input() public isForYou : boolean = true;

  constructor(private searchService : SearchServiceService) {
  }
  backToMainPaige() {

  }

  protected readonly faMagnifyingGlass = faMagnifyingGlass;

  searchByQuery() {
    if (this.query == ""){
      return;
    }
    this.loadInitialPosts();
  }
  loadInitialPosts(){
    this.searchService.getPosts(this.query, this.postsToLoadLowerCount, this.postsToLoadUpperCount).subscribe(
      (posts : HttpResponse<PostDTO[]>) => {
        this.matchingPosts$.next(posts.body || []);
      },
      (error) => console.log(error.error)
    );
  }

  loadMorePosts(){
    this.searchService.getPosts(this.query, this.postsToLoadLowerCount, this.postsToLoadUpperCount).subscribe(
      (posts : HttpResponse<PostDTO[]>) => {
        const currentPosts = this.matchingPosts$.getValue();
        const newPosts = posts.body || [];
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
}
