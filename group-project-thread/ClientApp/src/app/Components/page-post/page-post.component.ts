import {Component, Input} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {months} from "../../../assets/months";
import {faComment, faHeart} from "@fortawesome/free-regular-svg-icons";
import {faRetweet, faSquarePollVertical} from "@fortawesome/free-solid-svg-icons";
import seedrandom from "seedrandom";

@Component({
  selector: 'app-page-post',
  templateUrl: './page-post.component.html',
  styleUrls: ['./page-post.component.scss']
})
export class PagePostComponent {
  faComment = faComment;
  faHeart = faHeart;
  faRetweet = faRetweet;
  faSquarePollVertical = faSquarePollVertical;
  @Input() public post!: PagePostDTO;
  @Input() public user!: UserWithPostDTO;

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
    console.log("Comment clicked");
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
