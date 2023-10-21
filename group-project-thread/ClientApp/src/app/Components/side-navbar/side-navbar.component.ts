import {Component, ElementRef, Input, ViewChild} from '@angular/core';
import {
  faHome,
  faBookmark as faBookmarkActivated,
  faSearch,
  faUser as faUserActivated,
  faRetweet, faTimes
} from "@fortawesome/free-solid-svg-icons";
import {faBookmark as faBookmarkUnactivated, faUser as faUserUnactivated} from "@fortawesome/free-regular-svg-icons";
import {IconDefinition} from "@fortawesome/free-brands-svg-icons";
import {Icon} from "@fortawesome/fontawesome-svg-core";
import {PostCreationDialogComponent} from "../post-creation-dialog/post-creation-dialog.component";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {MainPageComponent} from "../main-page/main-page.component";
import {PostService} from "../../Services/post.service";
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import PostFormatter from "../../helpers/postFormatter";
import {Router, RouterOutlet} from "@angular/router";
  interface faToggleIcon {
  activated : IconDefinition,
  unactivated : IconDefinition
}

export enum Endpoint{
  Home ,
  Search ,
  Bookmarks,
  Reposts,
  Profile
}
@Component({
  selector: 'app-side-navbar',
  templateUrl: './side-navbar.component.html',
  styleUrls: ['./side-navbar.component.scss']
})
export class SideNavbarComponent{

    constructor(private router : Router,private dialog: MatDialog, private readonly postService: PostService, private readonly userService: UserService) {

    }

    get faHome() { return faHome }
    get faSearch() { return faSearch }
    get faBookmarkIcon() { return this.faBookmark[this.activatedEndpoint === Endpoint.Bookmarks ? "activated" : "unactivated"] }
    get faRetweet() { return faRetweet };
    get faUserIcon() { return this.faUser[this.activatedEndpoint === Endpoint.Profile ? "activated" : "unactivated"] }

    @Input() activatedEndpoint : Endpoint = Endpoint.Home;
    @ViewChild('logoutButton') logoutButton : ElementRef<HTMLButtonElement>;
    numberOfLogoutClicks : number = 0;
    user : UserDTO = {} as UserDTO;

    ngOnInit(): void {
      this.userService.getCurrentUser().subscribe(Response => {
        if(Response.ok) {
          this.user = Response.body!;
        }
      });
    }

    faBookmark : faToggleIcon = {activated: faBookmarkActivated, unactivated: faBookmarkUnactivated};
    faUser : faToggleIcon = {activated: faUserActivated, unactivated: faUserUnactivated};

    openPostingDialog() {
      const dialogRef : MatDialogRef<PostCreationDialogComponent, {output:string}> = this.dialog.open(PostCreationDialogComponent, {
         width: '500px'
       });

        dialogRef.afterClosed().subscribe(result => {
         if(result) {
           this.postService.createPost({textContent: result!.output, images: []}).subscribe(response => console.log(response));
         }
        });
    }

  protected readonly Endpoint = Endpoint;

  isAvatarNull() {
    return this.user.avatar === null;
  }

  getCircleColor() {
    return PostFormatter.getCircleColor(this.user.username);
  }

  getFirstInitial() {
    return this.user.username.charAt(0).toUpperCase();
  }

  logoutBtnClick() {
    this.numberOfLogoutClicks++;
    console.log(this.numberOfLogoutClicks);
    if(this.numberOfLogoutClicks === 2) {
      localStorage.removeItem("token");
      this.router.navigate(["/login"]);
      this.numberOfLogoutClicks = 0;
    }
  }

  protected readonly faTimes = faTimes;

  discardCloseBtnClick() {
    this.numberOfLogoutClicks = 0;
  }
}
