import {Component, Input} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {ReturnStatement} from "@angular/compiler";
import {months} from "../../../assets/months";
import {faComment, faHeart} from "@fortawesome/free-regular-svg-icons";
import {faRetweet, faSquarePollVertical} from "@fortawesome/free-solid-svg-icons";
import {RNG} from "random";
import {Random} from "random";
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

  getFirstInitial() : string {
    return this.user.username[0].toUpperCase();
  }

  public getCreatedDate() : string {
    const date = new Date(this.post.dateCreated);
    return months[date.getMonth()].name.substring(0,3) + " " + date.getDay();
  }

  isAvatarNull() : boolean {
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
    return colorArray[Math.floor(seedrandom(this.user.username).double()*colorArray.length)];
  }

}
