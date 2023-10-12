import {Component, Input, OnInit} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import PostFormatter from 'src/app/helpers/postFormatter';
import {faComment, faHeart as faHeartUnactivated} from "@fortawesome/free-regular-svg-icons";
import {faRetweet, faSquarePollVertical} from "@fortawesome/free-solid-svg-icons";
import {faHeart as faHeartActivated} from "@fortawesome/free-solid-svg-icons";
import seedrandom from "seedrandom";
import {MatDialog} from "@angular/material/dialog";
import {CommentCreationDialogComponent} from "../comment-creation-dialog/comment-creation-dialog.component";
import {CommentService} from "../../Services/comment.service";
import {CommentCreateDTO} from "../../models/coment/commentCreateDTO";
import {PostService} from "../../Services/post.service";
import {PostDTO} from "../../models/post/postDTO";
import {UserDTO} from "../../models/user/userDTO";
import {Image} from "../../models/image";

@Component({
  selector: 'app-page-post',
  templateUrl: './page-post.component.html',
  styleUrls: ['./page-post.component.scss']
})
export class PagePostComponent implements OnInit {
  faComment = faComment;
  faRetweet = faRetweet;
  faSquarePollVertical = faSquarePollVertical;
  @Input() public postInput!: PostDTO;
  post:PagePostDTO = {} as PagePostDTO;
  @Input() public userInput!: UserDTO;
  @Input() public excludeFooter: boolean = false;
  @Input() public excludeImages: boolean = false;

  liked: boolean = false;
  reposted: boolean = false;
  faHeartActivated = faHeartActivated;
  faHeartUnactivated = faHeartUnactivated;

  constructor(public dialog: MatDialog, private readonly commentService : CommentService, private readonly postService : PostService) {

  }

  ngOnInit(): void {
    this.liked = this.postInput.likesIds.includes(this.userInput.id);
    this.reposted = this.postInput.repostersIds.includes(this.userInput.id);
    this.post = {
      id: this.postInput.id,
      user: {
        id: this.userInput.id,
        username: this.userInput.username,
        avatar: this.userInput.avatar,
        bio: this.userInput.bio,
        followers: this.userInput.followersIds.length,
        following: this.userInput.followingIds.length
      },
      textContent: this.postInput.textContent,
      dateCreated: this.postInput.createdAt,
      imagesUrls: this.postInput.images.map(image => image.url),
      likesAmount: this.postInput.likesIds.length,
      commentsAmount: this.postInput.commentsIds.length,
      repostsAmount: this.postInput.repostersIds.length,
      viewsAmount: this.postInput.viewedBy.length
    }
    this.postService.viewPost(this.post.id).subscribe(response => console.log(response));
  }

  getFirstInitial(): string {
    return this.post.user.username[0].toUpperCase();
  }

  public getCreatedDate(): string {
    const date = new Date(this.post.dateCreated);
    return PostFormatter.getDateFormattedString(date);
  }

  isAvatarNull(): boolean {
    return this.post.user.avatar === null;
  }

  handleCommentClick() {
    this.openCommentDialog();
  }

  openCommentDialog() {
    const dialogRef = this.dialog.open(CommentCreationDialogComponent, {
      width: '500px',
      data: { post: this.post, currentUser: this.post.user, textContent: "", images: []}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      console.log(result);
      this.commentService.postComment({
        postId: this.post.id,
        textContent: result.textContent,
        images: result.images
      }).subscribe(response => console.log(response));
    });
  }

  handleRepostClick() {
    console.log("Repost clicked");
    switch (this.reposted) {
      case true:
        this.post.repostsAmount--;
        this.reposted = false;
        this.postService.undoRepost(this.post.id).subscribe(response => console.log(response));
        break;
      case false:
        this.post.repostsAmount++;
        this.reposted = true;
        this.postService.repost(this.post.id).subscribe(response => console.log(response));
    }
  }

  handleLikeClick() {
    console.log("Like clicked");
    switch (this.liked) {
        case true:
        this.post.likesAmount--;
        this.liked = false;
        this.postService.unlikePost(this.post.id).subscribe(response => console.log(response));
        break;
      case false:
        this.post.likesAmount++;
        this.liked = true;
        this.postService.likePost(this.post.id).subscribe(response => console.log(response));
        break;
    }
  }

  getCircleColor() {
    return PostFormatter.getCircleColor(this.post.user.username);
  }


}
