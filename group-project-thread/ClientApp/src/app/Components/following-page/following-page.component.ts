import {Component, OnInit} from '@angular/core';
import {UserService} from "../../Services/user.service";
import {ActivatedRoute} from "@angular/router";
import {SnackbarService} from "../../Services/snackbar.service";
import {UserDTO} from "../../models/user/userDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {finalize, Subject, takeUntil} from "rxjs";

@Component({
  selector: 'app-following-page',
  templateUrl: './following-page.component.html',
  styleUrls: ['./following-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class FollowingPageComponent implements OnInit{
  protected username : string;
  protected user! : UserDTO;
  protected following : UserWithPostDTO[];
  protected followingText : string = "Following";
  protected unfollowed : boolean = false;
  private submitted: boolean = false;
  private unsubscribe$ = new Subject<void>();

  constructor(private userService: UserService, private route: ActivatedRoute, private snackBarService: SnackbarService) {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername";
    })
    this.following = [];
  }

  ngOnInit(): void {
    this.userService.getUserByUsername(this.username)
      .subscribe(response => {
        if (response.body != null) {
          this.user = response.body;
          for (let i = 0; i < this.user.followingIds.length; i++) {
            const userId = this.user.followingIds[i];
            this.userService.getUserById(userId)
              .subscribe(response => {
                if (response.body != null) {
                  const userResponse: UserDTO = response.body;
                  const followingUser: UserWithPostDTO = {
                    avatar: userResponse.avatar,
                    bio: userResponse.bio,
                    followers: userResponse.followersIds.length,
                    following: userResponse.followingIds.length,
                    id: userResponse.id,
                    username: userResponse.username,
                  };
                  this.following.push(followingUser);
                }
              });
          }
        }
      });
  }

  navigateToUserProfile() {

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
    if (!this.submitted){
      this.submitted = true;
      this.userService.unfollowUser(id).
      pipe(takeUntil(this.unsubscribe$), finalize(() => {
        this.submitted = true;
        this.unfollowed = true;
      })).subscribe(() => {

      }, (error)=> {
        this.snackBarService.showErrorMessage(error.error.title);});
    }

  }

  follow() {
    this.unfollowed = false;
  }
}
