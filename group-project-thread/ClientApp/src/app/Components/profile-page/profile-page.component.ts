import {Component, OnDestroy, OnInit} from '@angular/core';
import {HttpInternalService} from "../../Services/http-internal.service";
import {AuthService} from "../../Services/auth.service";
import {BehaviorSubject, Observable, Subject, switchMap, takeUntil} from "rxjs";
import {UserDTO} from "../../models/user/userDTO";
import {Image} from "../../models/image";
import {SnackbarService} from "../../Services/snackbar.service";
import {UserService} from "../../Services/user.service";
import {Router} from "@angular/router";
import {faArrowLeftLong, faLocationDot} from "@fortawesome/free-solid-svg-icons";
import {faCalendar} from "@fortawesome/free-regular-svg-icons"
import {PostService} from "../../Services/post.service";
import {PostDTO} from "../../models/post/postDTO";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {UserUpdateDialogComponent} from "../user-update-dialog/user-update-dialog.component";
import {UpdateUserDialogData} from "../../models/user/updateUserDialogData";
import {HttpResponse} from "@angular/common/http";

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class ProfilePageComponent implements OnInit, OnDestroy{

  public user = {} as UserDTO;
  public loading = false;
  public image: Image | null = null;
  faArrowLeftLong = faArrowLeftLong;
  faCalendar = faCalendar;
  faLocationDot = faLocationDot;
  public userPosts$ = new BehaviorSubject<PostDTO[]>([]);
  public followersCount! : number;
  public followingCount! : number;


  private unsubscribe$ = new Subject<void>;

  constructor(httpServise: HttpInternalService,
              private authService : AuthService,
              private snackBarService: SnackbarService,
              private userService : UserService,
              private postService : PostService,
              private router: Router,
              public dialog: MatDialog,) {

  }
  ngOnInit(): void {
    this.authService.getUser()
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap((user : UserDTO | null) => {
          if (user) {
            this.user = this.userService.copyUser(user);
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
    this.router.navigate(['/mainPage']);
  }
  public navigateToFollowing(){
    this.router.navigate([this.user.username, "following"])
  }

  public navigateToFollowers(){
    this.router.navigate([this.user.username, "followers"])
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
}
