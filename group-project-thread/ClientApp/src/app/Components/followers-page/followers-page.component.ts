import {Component, NgZone} from '@angular/core';
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {SnackbarService} from "../../Services/snackbar.service";
import {UserDTO} from "../../models/user/userDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {finalize, Subject, takeUntil} from "rxjs";
import {F} from "@angular/cdk/keycodes";
@Component({
  selector: 'app-followers-page',
  templateUrl: './followers-page.component.html',
  styleUrls: ['./followers-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class FollowersPageComponent {
  protected username : string;
  protected user! : UserDTO;
  protected followers : UserWithPostDTO[];
  protected isMyFollowing : boolean;
  public followingText : string = "Following";
  private submittedUnfollow: boolean = false;
  private submittedFollow : boolean = false;
  private unsubscribe$ = new Subject<void>();
  protected followed = false;
  constructor(private userService: UserService, private route: ActivatedRoute,
              private  snackBarService : SnackbarService, private ngZone: NgZone ) {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername";
    })
    this.followers = [];
  }

  handleUser(user: UserDTO) {
    this.user = user;
  }
  ngOnInit(): void {
    this.userService.getUserByUsername(this.username)
      .subscribe(response => {
        if (response.body != null) {
          this.user = response.body;
          for (let i = 0; i < this.user.followersIds.length; i++) {
            const userId = this.user.followersIds[i];
            this.userService.getUserById(userId)
              .subscribe(response => {
                if (response.body != null) {
                  const userResponse: UserDTO = response.body;
                  const follower: UserWithPostDTO = {
                    avatar: userResponse.avatar,
                    bio: userResponse.bio,
                    followers: userResponse.followersIds.length,
                    following: userResponse.followingIds.length,
                    id: userResponse.id,
                    username: userResponse.username,
                  };
                  this.followers.push(follower);
                }
              });
          }
        }
      });
  }

  navigateToUserProfile() {

  }

  amIFollowing(id : number): boolean{
    if (this.user.followingIds.includes(id)){
      this.followed = true;
      return true;
    }
    console.log()
    this.followed = false;
    return false;
  }

  changeSpan() {

    if (this.followingText == "Following"){
      this.followingText = "Unfollow"
    }
    else {
      this.followingText = "Following";
    }
  }
  unfollow(id : number) {
    if (!this.submittedUnfollow){
      this.submittedUnfollow = true;
      this.userService.unfollowUser(id).
      pipe(takeUntil(this.unsubscribe$), finalize(() => {
        this.submittedUnfollow = false;
      })).subscribe((responce) => {
        if (responce.ok){
          this.followed = false;
          const index = this.user.followingIds.indexOf(id);
          this.user.followingIds.splice(index, 1);

        }
      }, (error)=> {
        this.snackBarService.showErrorMessage(error);});
    }
  }


  follow(id : number) {
    if (!this.submittedFollow){
      this.submittedFollow = true;
      this.userService.followUser(id).
      pipe(takeUntil(this.unsubscribe$), finalize(() => this.submittedFollow = false ))
        .subscribe((responce) => {
          if (responce.ok){
            this.followed = true;
            this.user.followingIds.push(id);
          }
        }, (error)=> {
          this.snackBarService.showErrorMessage(error.error.title);});
    }
  }
}
