import {Component, Input, OnInit} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import PostFormatter from 'src/app/helpers/postFormatter';
import {faComment, faHeart} from "@fortawesome/free-regular-svg-icons";
import {faRetweet, faSquarePollVertical} from "@fortawesome/free-solid-svg-icons";
import seedrandom from "seedrandom";
import {MatDialog} from "@angular/material/dialog";
import {CommentCreationDialogComponent} from "../comment-creation-dialog/comment-creation-dialog.component";

@Component({
  selector: 'app-page-post',
  templateUrl: './page-post.component.html',
  styleUrls: ['./page-post.component.scss']
})
export class PagePostComponent{
  faComment = faComment;
  faHeart = faHeart;
  faRetweet = faRetweet;
  faSquarePollVertical = faSquarePollVertical;
  @Input() public post!: PagePostDTO;
  @Input() public user!: UserWithPostDTO;
  @Input() public excludeFooter: boolean = false;
  @Input() public excludeImages: boolean = false;

  constructor(public dialog: MatDialog) { }

  getFirstInitial(): string {
    return this.user.username[0].toUpperCase();
  }

  public getCreatedDate(): string {
    const date = new Date(this.post.dateCreated);
    return PostFormatter.getDateFormattedString(date);
  }

  isAvatarNull(): boolean {
    return this.user.avatar === null;
  }

  handleCommentClick() {
    this.openCommentDialog();
  }

  openCommentDialog() {
    const dialogRef = this.dialog.open(CommentCreationDialogComponent, {
      width: '500px',
      data: { post: this.post, currentUser: this.user, textContent: "", images: []}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      console.log(result);

    });
  }

  handleRepostClick() {
    console.log("Repost clicked");
  }

  handleLikeClick() {
    console.log("Like clicked");
  }

  getCircleColor() {
    return PostFormatter.getCircleColor(this.user.username);
  }


}
