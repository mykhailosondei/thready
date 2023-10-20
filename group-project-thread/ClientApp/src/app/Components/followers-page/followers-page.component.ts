import {Component, NgZone} from '@angular/core';
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {UserDTO} from "../../models/user/userDTO";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {finalize, Subject, takeUntil} from "rxjs";
import {FollowingFollowersNavigatorService} from "../../Services/following-followers-navigator.service";
import {Tab} from "../../models/enums/Tab";
@Component({
  selector: 'app-followers-page',
  templateUrl: './followers-page.component.html',
  styleUrls: ['./followers-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class FollowersPageComponent {
  protected username : string;
  protected user! : UserDTO;
  protected followers : PageUserDTO[];
  private unsubscribe$ = new Subject<void>();
  protected currentUser! : UserDTO;
  constructor(private userService: UserService, private route: ActivatedRoute, public navigatorService : FollowingFollowersNavigatorService) {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername";
    })
    this.followers = [];
  }

  handleUser(user: UserDTO) {
    this.user = user;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername";
      this.getCurrentUser();
      this.fetchFollowersData(this.username);
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  fetchFollowersData(username: string) {
    this.userService.getUserByUsername(username).subscribe(response => {
      if (response.body != null) {
        this.user = response.body;
        this.followers = [];
        for (let i = 0; i < this.user.followersIds.length; i++) {
          const userId = this.user.followersIds[i];
          this.userService.getUserById(userId).subscribe(response => {
            if (response.body != null) {
              const userResponse: UserDTO = response.body;
              const follower: PageUserDTO = {
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

  amIFollowing(id : number): boolean{
    return this.currentUser.followingIds.includes(id);
  }

  getCurrentUser(): void{
    this.userService.getCurrentUser()
      .subscribe( (response) =>{
      if (response.body != null){
         this.currentUser = response.body;
      }
    });
  }

  protected readonly Tab = Tab;
}