import {Component, Input} from '@angular/core';
import {
  faHome,
  faBookmark as faBookmarkActivated,
  faSearch,
  faUser as faUserActivated,
  faRetweet
} from "@fortawesome/free-solid-svg-icons";
import {faBookmark as faBookmarkUnactivated, faUser as faUserUnactivated} from "@fortawesome/free-regular-svg-icons";
import {IconDefinition} from "@fortawesome/free-brands-svg-icons";
import {Icon} from "@fortawesome/fontawesome-svg-core";
import {PostCreationDialogComponent} from "../post-creation-dialog/post-creation-dialog.component";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {MainPageComponent} from "../main-page/main-page.component";
import {PostService} from "../../Services/post.service";
  interface faToggleIcon {
  activated : IconDefinition,
  unactivated : IconDefinition
}
@Component({
  selector: 'app-side-navbar',
  templateUrl: './side-navbar.component.html',
  styleUrls: ['./side-navbar.component.scss']
})
export class SideNavbarComponent{


    constructor(private dialog: MatDialog, private readonly postService: PostService) {

    }
  get faHome() { return faHome }
  get faSearch() { return faSearch }

  @Input() activeIcon : IconDefinition = faHome;

  faBookmark : faToggleIcon = {activated: faBookmarkActivated, unactivated: faBookmarkUnactivated};
  get faBookmarkIcon() { return this.faBookmark[this.activeIcon === faBookmarkActivated ? "activated" : "unactivated"] }
  faUser : faToggleIcon = {activated: faUserActivated, unactivated: faUserUnactivated};
  get faUserIcon() { return this.faUser[this.activeIcon === faUserActivated ? "activated" : "unactivated"] }

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

  get faRetweet() { return faRetweet };
}
