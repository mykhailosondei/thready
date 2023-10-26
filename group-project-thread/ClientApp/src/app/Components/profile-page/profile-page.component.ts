import {Component, OnDestroy, OnInit} from '@angular/core';
import {BehaviorSubject, finalize, Subject, switchMap, takeUntil} from "rxjs";
import {UserDTO} from "../../models/user/userDTO";
import {Image} from "../../models/image";
import {SnackbarService} from "../../Services/snackbar.service";
import {UserService} from "../../Services/user.service";
import {ActivatedRoute} from "@angular/router";
import {faArrowLeftLong, faLocationDot} from "@fortawesome/free-solid-svg-icons";
import {faCalendar} from "@fortawesome/free-regular-svg-icons"
import {PostService} from "../../Services/post.service";
import {PostDTO} from "../../models/post/postDTO";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {UserUpdateDialogComponent} from "../user-update-dialog/user-update-dialog.component";
import {UpdateUserDialogData} from "../../models/user/updateUserDialogData";
import {HttpResponse} from "@angular/common/http";
import {Endpoint} from "../side-navbar/side-navbar.component";
import {NavigatorService} from "../../Services/navigator.service";
import {NavigationHistoryService} from "../../Services/navigation-history.service";
import {Location} from "@angular/common";
import PostFormatter from "../../helpers/postFormatter";

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss', '../../../assets/ContentFrame.scss', '../../../assets/spinner.scss']
})
export class ProfilePageComponent implements OnInit, OnDestroy{

  public username : string;
  public user = {} as UserDTO;
  public image: Image | null = null;
  faArrowLeftLong = faArrowLeftLong;
  faCalendar = faCalendar;
  faLocationDot = faLocationDot;
  public userPosts$ = new BehaviorSubject<PostDTO[]>([]);
  public followersCount! : number;
  public followingCount! : number;
  protected isCurrentUser : boolean = false;
  protected followingText : string = "Following";
  protected unfollowed : boolean;
  private unfollowSubmitted: boolean = false;
  private followSubmitted : boolean = false;
  protected isCurrentUserFollowing : boolean;
  protected postsText: string = "";
  public postsLoading : boolean;
  public noPostsFound : boolean;
  public headerText : string = "";
  protected Endpoint = Endpoint.None;

  private unsubscribe$ = new Subject<void>;

  constructor(private snackBarService: SnackbarService, private userService : UserService,
              private postService : PostService, private location : Location,
              public dialog: MatDialog, private route: ActivatedRoute,
              private navigator: NavigatorService, private historyOfPages : NavigationHistoryService) {

  }
  ngOnInit(): void {
    this.postsLoading = true;
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername"; });
    this.getUserInstance();
  }

  getUserInstance(){
    this.checkIsCurrentUser();
    this.user = {} as UserDTO;
    this.userPosts$ = new BehaviorSubject<PostDTO[]>([]);
    this.headerText = `@${this.username} hasn't posted yet`
    this.userService.getUserByUsername(this.username)
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap((response ) => {
          const user = response.body;
          if (user) {
            this.user = this.userService.copyUser(user);
            this.updateIsCurrentUserFollowing();
            return this.postService.getPostsByUserId(this.user.id);
          }
          return []
        })
      )
      .subscribe(
        (posts : HttpResponse<PostDTO[]>) => {
          this.user.posts = posts.body || [];
          this.followersCount = this.user.followersIds.length;
          this.followingCount = this.user.followingIds.length;
          this.userPosts$.next(posts.body || []);
          this.postsLoading = false;
          if (this.user.posts.length == 0){
            this.noPostsFound = true;
          }
        },
        (error) => {this.snackBarService.showErrorMessage(error.error.title)
        this.postsLoading = false;}
      );
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  public backToMainPaige(){
    this.location.back();
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername";
      console.log(this.username)
      this.getUserInstance();
    });
  }
  public navigateToFollowing(){
    this.historyOfPages.IncrementPageInHistoryCounter();
    this.navigator.openFollowingPage(this.user.username);
  }

  public navigateToFollowers(){
    this.historyOfPages.IncrementPageInHistoryCounter();
    this.navigator.openFollowersPage(this.user.username);
  }

  public openEditDialog(){
    const dialog : MatDialogRef<UserUpdateDialogComponent, {imageUrl:string, data: UpdateUserDialogData}> = this.dialog.open(UserUpdateDialogComponent, {
      minWidth: "550px", minHeight: "360px",
      data: {currentUser: this.user, bio: this.user.bio, location: this.user.location, avatar: this.user.avatar }
    })
    dialog.afterClosed().subscribe(result => {
      if (result === undefined) return;
      console.log(result);
      this.userService.putUser(this.user.id, {
        id : this.user.id,
        bio : result.data.bio,
        location : result.data.location,
        avatar : {url: result.imageUrl}
      }).subscribe(response => {
        if (response.body != null){
          const user : UserDTO = response.body;
          this.user.bio = user.bio;
          this.user.avatar = user.avatar;
          this.user.location = user.location;
        }
      })
    })
  }
  checkIsCurrentUser(): void{
    this.userService.getCurrentUserInstance()
      .subscribe(
      (user) => {
        if (this.username == user.username){
          this.postsText = "Your posts";
          this.isCurrentUser = true;
          this.Endpoint = Endpoint.Profile;
        }else{
          this.postsText = "User posts";
          this.Endpoint = Endpoint.None;
        }
      });
  }
  changeSpan() {
    if (this.followingText === "Unfollow"){
      this.followingText = "Following";
    }
    else {
      this.followingText = "Unfollow";
    }
  }

  updateIsCurrentUserFollowing(): void {
    if (this.user.id) {
      this.userService.getCurrentUser().subscribe((response) => {
        if (response.body != null) {
          this.isCurrentUserFollowing = response.body.followingIds.includes(this.user.id);
          this.unfollowed = !this.isCurrentUserFollowing;
        }
      });
    }
  }

  unfollow(id : number) {
    if (!this.unfollowSubmitted){
      this.unfollowSubmitted = true;
      this.userService.unfollowUser(id).
      pipe(finalize(() => {
        this.unfollowSubmitted = false;
      })).subscribe((response) => {
        if(response.ok){
          this.unfollowed = true;
          this.isCurrentUserFollowing = false;
        }
      }, (error)=> {
        this.snackBarService.showErrorMessage(error.error.title);});
    }
  }

  follow(id: number) {
    if (!this.followSubmitted){
      this.followSubmitted = true;
      this.userService.followUser(id).
      pipe(finalize(() => {
        this.followSubmitted = false;
      })).subscribe((response) => {
        if(response.ok){
          this.unfollowed = false;
          this.isCurrentUserFollowing = true;
        }
      }, (error)=> {
        this.snackBarService.showErrorMessage(error.error.title);});
    }
  }


  getFirstInitial() {
    return this.user.username.charAt(0).toUpperCase();
  }

  getAvatarColor() {
    return PostFormatter.getCircleColor(this.user.username);
  }

  isAvatarNull(): boolean {
    return this.user.avatar === null || this.user.avatar.url === "";
  }

  loadNewUser(username : string) {
    if (this.username == username){
      return;
    }
    this.username = username;
    this.noPostsFound = false;
    this.isCurrentUser = false;
    this.getUserInstance()
  }
}
