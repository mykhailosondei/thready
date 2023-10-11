import {Component, Input, OnInit} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {months} from "../../../assets/months";
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
    console.log(date.getHours());
    if(Date.now()-date.getTime() < 3600000){
      return this.minutesToReadable(date);
    }
    if(Date.now()-date.getTime() < 86400000){
      return this.hoursToReadable(date);
    }
    if(Date.now()-date.getTime() < 31536000000){
      return months[date.getMonth()].name.substring(0, 3) + " " + date.getDate();
    }
    return months[date.getMonth()].name.substring(0, 3) + " " + date.getDate() + ", " + date.getFullYear();
  }

  private hoursToReadable(date: Date) {
    return Math.floor((Date.now() - date.getTime()) / 3600000) + "h";
  }

  private minutesToReadable(date: Date) {
    return Math.floor((Date.now() - date.getTime()) / 60000) + "m";
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
      data: { post: this.post}
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
    const colorArray = ["red", "green", "yellow", "purple", "pink", "orange", "blue"];
    return colorArray[Math.floor(seedrandom(this.user.username).double() * colorArray.length)];
  }


}
