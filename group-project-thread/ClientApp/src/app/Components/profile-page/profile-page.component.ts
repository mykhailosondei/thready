import {Component, OnDestroy, OnInit} from '@angular/core';
import {HttpInternalService} from "../../Services/http-internal.service";
import {AuthService} from "../../Services/auth.service";
import {BehaviorSubject, Observable, Subject, switchMap, takeUntil} from "rxjs";
import {UserDTO} from "../../models/user/userDTO";
import {Image} from "../../models/image";
import {SnackbarService} from "../../Services/snackbar.service";
import {UserService} from "../../Services/user.service";
import {Router} from "@angular/router";
import {faArrowLeftLong} from "@fortawesome/free-solid-svg-icons";
import {PostService} from "../../Services/post.service";
import {PostDTO} from "../../models/post/postDTO";
import {HttpResponse} from "@angular/common/http";

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss']
})
export class ProfilePageComponent implements OnInit, OnDestroy{

  public user = {} as UserDTO;
  public loading = false;
  public image: Image | null = null;
  faArrowLeftLong = faArrowLeftLong;
  public userPosts$ = new BehaviorSubject<PostDTO[]>([]);
  public postCount! : number;


  private unsubscribe$ = new Subject<void>;
  constructor(httpServise: HttpInternalService,
              private authService : AuthService,
              private snackBarService: SnackbarService,
              private userService : UserService,
              private postService : PostService,
              private router: Router) {

  }
  ngOnInit(): void {
    this.authService.getUser()
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap((user) => {
          if (user) {
            this.user = this.userService.copyUser(user);
            return this.postService.getPostsByUserId(this.user.id);
          }
          return []
        })
      )
      .subscribe(
        (posts) => {
          this.user.posts = posts.body || [];
          this.postCount = 3;

        },
        (error) => this.snackBarService.showErrorMessage(error.error.title)
      );

  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }


  test() : void {
    this.authService.getUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((user) => {
        if (user){
          this.user = this.userService.copyUser(user);
        }
      }, (error) => this.snackBarService.showErrorMessage(error.error.title))
  }

  public backToMainPaige(){
    this.router.navigate(['/mainPage']);
  }

}
