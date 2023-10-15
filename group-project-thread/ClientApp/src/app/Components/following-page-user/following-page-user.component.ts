import {Component, Input} from '@angular/core';
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {finalize, Subject} from "rxjs";
import {UserService} from "../../Services/user.service";
import {SnackbarService} from "../../Services/snackbar.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-following-page-user',
  templateUrl: './following-page-user.component.html',
  styleUrls: ['./following-page-user.component.scss', '../../../assets/PageUser.scss']
})
export class FollowingPageUserComponent {
  @Input() public pageUser : UserWithPostDTO;
  @Input() public isCurrentUser : boolean;


  protected followingText : string = "Following";
  protected unfollowed : boolean = false;
  private unfollowSubmitted: boolean = false;
  private followSubmitted : boolean = false;
  private unsubscribe$ = new Subject<void>();
  constructor(private userService : UserService, private snackBarService : SnackbarService,
              private router: Router) {
  }


  navigateToUserProfile() {

  }
  getUserFollowing(username: string) {
    this.router.navigate([username, 'following'])
  }

  changeSpan() {
    if (this.followingText === "Unfollow"){
      this.followingText = "Following";
    }
    else {
      this.followingText = "Unfollow";
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
        }
      }, (error)=> {
        this.snackBarService.showErrorMessage(error.error.title);});
    }
  }
}

