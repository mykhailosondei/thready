import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {faBookmark as faBookmarkUnactivated, faComment, faHeart as faHeartUnactivated} from "@fortawesome/free-regular-svg-icons";
import {faBookmark as faBookmarkActivated, faEllipsisH, faRetweet, faSquarePollVertical} from "@fortawesome/free-solid-svg-icons";
import {faHeart as faHeartActivated} from "@fortawesome/free-solid-svg-icons";
import {UserService} from "../../Services/user.service";
import {UserDTO} from "../../models/user/userDTO";
import {PostDTO} from "../../models/post/postDTO";
import {PostService} from "../../Services/post.service";
import {CommentDTO} from "../../models/coment/commentDTO";
import {CommentService} from "../../Services/comment.service";
import {PagePostDTO} from "../../models/post/pagePostDTO";
import PostFormatter from "../../helpers/postFormatter";
import {faPen} from "@fortawesome/free-solid-svg-icons/faPen";
import {UserHoverCardTriggerService} from "../../Services/user-hover-card-trigger.service";
import {A} from "@angular/cdk/keycodes";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {CommentCreationDialogComponent} from "../comment-creation-dialog/comment-creation-dialog.component";
import {CommentCreateDialogData} from "../../models/coment/CommentCreateDialogData";

@Component({
  selector: 'app-singular-post-view',
  templateUrl: './singular-post-view.component.html',
  styleUrls: ['./singular-post-view.component.scss', '../page-post/page-post.component.scss']
})
export class SingularPostViewComponent implements OnInit{

    faBookmarkActivated = faBookmarkActivated;
    faBookmarkUnactivated = faBookmarkUnactivated;
    faHeartActivated = faHeartActivated;
    faHeartUnactivated = faHeartUnactivated;

    incomingUsername : string = "";
    incomingPostId : number = 0;
    timestamp : number = 0;
    public authorInput : UserDTO;
    public postInput : PostDTO;
    public comments : CommentDTO[] = [];
    public post : PagePostDTO;
    @ViewChild('userInfo') userInfo: ElementRef<HTMLDivElement>;

    liked: boolean = false;
    reposted: boolean = false;
    bookmarked: boolean = false;
    constructor(private route : ActivatedRoute, private userService : UserService,
                private postService : PostService, private commentService : CommentService,
                private hoverCardTriggerService: UserHoverCardTriggerService,
                private dialog: MatDialog) {
      this.route.paramMap.subscribe(params => {
        this.incomingUsername = params.get('username') || 'DefaultUsername';
        const postId = params.get('id');
        console.log(postId);
        if (postId) {
          this.incomingPostId = Number.parseInt(postId);
        }
      });
    }
    ngOnInit(): void {
      this.fetchEssentialData();
      this.fetchComments();
    }

    fetchEssentialData() {
      this.userService.getUserByUsername(this.incomingUsername).subscribe(response => {
        if(response.ok) {
          this.authorInput = response.body!;
          this.bookmarked = this.authorInput.bookmarkedPostsIds.includes(this.incomingPostId);
          console.log(response);
        }
      });
      this.postService.getPostById(this.incomingPostId).subscribe(response => {
        if(response.ok) {
          this.postInput = response.body!;
          this.post = {
            id: this.postInput.id,
            user: {
              id: this.authorInput.id,
              username: this.authorInput.username,
              avatar: this.authorInput.avatar,
              bio: this.authorInput.bio,
              followers: this.authorInput.followersIds.length,
              following: this.authorInput.followingIds.length
            },
            textContent: this.postInput.textContent,
            dateCreated: this.postInput.createdAt,
            imagesUrls: ['https://picsum.photos/1000/1000'],
            likesAmount: this.postInput.likesIds.length,
            commentsAmount: this.postInput.commentsIds.length,
            repostsAmount: this.postInput.repostersIds.length,
            viewsAmount: this.postInput.viewedBy.length,
            bookmarksAmount: this.postInput.bookmarks
          }
        console.log(response);
        }
      });
    }

    fetchComments() {
      for (let commentId of this.postInput.commentsIds) {
        this.commentService.getCommentById(commentId).subscribe(response => {
          if(response.ok) {
            this.comments.push(response.body!);
          }
        });
      }
    };

    faRetweet = faRetweet;
    faComment = faComment;

  handleCommentClick() {
    this.openCommentDialog();
  }

  openCommentDialog() {
    const dialogRef: MatDialogRef<CommentCreationDialogComponent, CommentCreateDialogData> = this.dialog.open(CommentCreationDialogComponent, {
      width: '500px',
      data: {post: this.post, currentUser: this.post.user, textContent: "", images: []}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === undefined) return;
      console.log('The dialog was closed');
      console.log(result);
      this.commentService.postComment({
        postId: this.post.id,
        textContent: result.textContent,
        images: result.images
      }).subscribe(response => {
        console.log(response)
        if (!response.ok) return;
        this.post.commentsAmount++;
      });
    });
  }

  handleRepostClick() {
    console.log("Repost clicked");
    switch (this.reposted) {
      case true:
        this.post.repostsAmount--;
        this.reposted = false;
        this.postService.undoRepost(this.post.id).subscribe();
        break;
      case false:
        this.post.repostsAmount++;
        this.reposted = true;
        this.postService.repost(this.post.id).subscribe();
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
    return PostFormatter.getCircleColor(this.authorInput.username);
  }

  isAvatarNull() {
    return this.authorInput.avatar === null;
  }

  getFirstInitial() {
    return this.authorInput.username[0].toUpperCase();
  }

  protected readonly faPen = faPen;

  handleBookmarkClick() {
    console.log("Bookmark clicked");
    switch (this.bookmarked) {
      case true:
            this.bookmarked = false;
            this.post.bookmarksAmount--;
        this.postService.removeFromBookmarksPost(this.post.id).subscribe(Response => {
          if (!Response.ok) {
            this.bookmarked = true;
            this.post.bookmarksAmount++;
          }
          console.log(Response)
        }).add(() => console.log("Completed"))
        break;
      case false:
            this.bookmarked = true;
            this.post.bookmarksAmount++;
        this.postService.bookmarkPost(this.post.id).subscribe(Response => {
          if (!Response.ok) {
            this.bookmarked = false;
            this.post.bookmarksAmount--;
          }
          console.log(Response)
        });
    }
  }

  getCreatedDate() {
    return PostFormatter.getDateFormattedString(new Date(this.postInput.createdAt));
  }

  async onUserInfoMouseLeave() {
    this.hoverCardTriggerService.triggeringElementOnLeaveTimeStamp = Date.now();
    this.hoverCardTriggerService.isHoveredOnTriggeringElement = false;
    await this.delay(100).then(() => {
      if (!this.hoverCardTriggerService.isInsideHoverCard && !this.hoverCardTriggerService.isHoveredOnTriggeringElement) {
        this.hoverCardTriggerService.disableHoverCardVisibility();
      }
    });
  }

  private delay(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  onUserInfoMouseEnter() {
    console.log("Mouse enter");
    this.hoverCardTriggerService.user = this.post.user;
    this.hoverCardTriggerService.enableHoverCardVisibility();
    this.hoverCardTriggerService.isHoveredOnTriggeringElement = true;
    this.hoverCardTriggerService.coordinates = {
      x: this.userInfo.nativeElement.offsetLeft - 60,
      y: this.userInfo.nativeElement.offsetTop + 20
    };
  }

  protected readonly A = A;
}
