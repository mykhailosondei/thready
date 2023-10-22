import {Component, OnDestroy, OnInit} from '@angular/core';
import {BehaviorSubject, finalize, Observable, Subject, switchMap, takeUntil} from "rxjs";
import {UserDTO} from "../../models/user/userDTO";
import {Image} from "../../models/image";
import {SnackbarService} from "../../Services/snackbar.service";
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
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

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class ProfilePageComponent implements OnInit, OnDestroy{

  public username : string;
  public user = {} as UserDTO;
  public loading = false;
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
  protected contentLoaded = false;
  protected postsText: string = "";

  private unsubscribe$ = new Subject<void>;

  constructor(private snackBarService: SnackbarService, private userService : UserService,
              private postService : PostService, private router: Router,
              public dialog: MatDialog, private route: ActivatedRoute,
              private navigator: NavigatorService) {

  }
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername"; });
      this.checkIsCurrentUser();
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
        },
        (error) => this.snackBarService.showErrorMessage(error.error.title)
      );
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  public backToMainPaige(){
    this.navigator.goBack();
  }
  public navigateToFollowing(){
    this.navigator.openFollowingPage(this.user.username);
  }

  public navigateToFollowers(){
    this.navigator.openFollowersPage(this.user.username);
  }

  public openEditDialog(){
    const dialog : MatDialogRef<UserUpdateDialogComponent, UpdateUserDialogData> = this.dialog.open(UserUpdateDialogComponent, {
      maxWidth: "550px", minHeight: "360px",
      data: {currentUser: this.user, bio: this.user.bio, location: this.user.location, avatar: this.user.avatar }
    })
    dialog.afterClosed().subscribe(result => {
      if (result === undefined) return;
      console.log(result);
      this.userService.putUser(this.user.id, {
        id : this.user.id,
        bio : result.bio,
        location : result.location,
        avatar : null
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
    this.userService.getCurrentUser().pipe(takeUntil(this.unsubscribe$))
      .subscribe( (response) =>{
        if (response.body != null){
           this.isCurrentUser = this.username == response.body.username;
           if (this.username == response.body.username){
             this.postsText = "Your posts"
           }else{
             this.postsText = "User posts"
           }
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
      this.userService.getCurrentUser().pipe(takeUntil(this.unsubscribe$), finalize(() => this.contentLoaded = true)).subscribe((response) => {
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

    protected readonly Endpoint = Endpoint;
}
