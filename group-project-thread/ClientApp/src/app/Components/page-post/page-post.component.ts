import {Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import PostFormatter from 'src/app/helpers/postFormatter';
import {faBookmark as faBookmarkUnactivated, faComment, faHeart as faHeartUnactivated} from "@fortawesome/free-regular-svg-icons";
import {faBookmark as faBookmarkActivated, faEllipsisH, faRetweet, faSquarePollVertical} from "@fortawesome/free-solid-svg-icons";
import {faHeart as faHeartActivated} from "@fortawesome/free-solid-svg-icons";
import seedrandom from "seedrandom";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {CommentCreationDialogComponent} from "../comment-creation-dialog/comment-creation-dialog.component";
import {CommentService} from "../../Services/comment.service";
import {CommentCreateDTO} from "../../models/coment/commentCreateDTO";
import {PostService} from "../../Services/post.service";
import {UserService} from "../../Services/user.service";
import {PostDTO} from "../../models/post/postDTO";
import {UserDTO} from "../../models/user/userDTO";
import {Image} from "../../models/image";
import {faPen} from "@fortawesome/free-solid-svg-icons/faPen";
import {C} from "@angular/cdk/keycodes";
import {PostEditorDialogComponent} from "../post-editor-dialog/post-editor-dialog.component";
import {CommentCreateDialogData} from "../../models/coment/CommentCreateDialogData";
import {UserHoverCardTriggerService} from "../../Services/user-hover-card-trigger.service";
import {waitForAsync} from "@angular/core/testing";
import {delay} from "rxjs";


@Component({
  selector: 'app-page-post',
  templateUrl: './page-post.component.html',
  styleUrls: ['./page-post.component.scss']
})
export class PagePostComponent implements OnInit {
  faPen = faPen;
  faComment = faComment;
  faRetweet = faRetweet;
  faSquarePollVertical = faSquarePollVertical;
  faHeartActivated = faHeartActivated;
  faHeartUnactivated = faHeartUnactivated;
  faBookmarkActivated = faBookmarkActivated;
  faBookmarkUnactivated = faBookmarkUnactivated;

  @Input() public postInput!: PostDTO;
  post: PagePostDTO = {} as PagePostDTO;
  @Input() public userInput!: UserDTO;
  @Input() public excludeFooter: boolean = false;
  @Input() public excludeImages: boolean = false;

  @ViewChild('userInfo') userInfo: ElementRef<HTMLDivElement>;
  @ViewChild('wholePost') wholePost: ElementRef<HTMLDivElement>;

  private observer: IntersectionObserver;


  viewed: boolean = false;
  editable: boolean = false;
  liked: boolean = false;
  reposted: boolean = false;
  bookmarked: boolean;

  constructor(public dialog: MatDialog,
              private readonly commentService: CommentService,
              private readonly postService: PostService,
              private readonly userService: UserService,
              private readonly hoverCardTriggerService: UserHoverCardTriggerService) {
  }

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe(response => {
      if (response.ok) {
        this.editable = response.body!.id === this.postInput.author.id;
        this.liked = this.postInput.likesIds.includes(response.body!.id);
        this.reposted = this.postInput.repostersIds.includes(response.body!.id);
        this.bookmarked = response.body!.bookmarkedPostsIds.includes(this.postInput.id);
      }
    });
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
      viewsAmount: this.postInput.viewedBy.length,
      bookmarksAmount: this.postInput.bookmarks
    }
    // Set up IntersectionObserver to watch for 50% visibility
    this.observer = new IntersectionObserver(this.handleIntersection.bind(this), {
      root: null, // Use the viewport as the root
      rootMargin: '0px', // No margin
      threshold: 0.5, // 50% visibility
    });

  }

  startObserve() {
    this.observer.observe(this.wholePost.nativeElement);
  }

  handleIntersection(entries: any) {
    entries.forEach((entry: any) => {
      if (entry.isIntersecting && !this.viewed) {
        // The component is at least 50% visible in the viewport
        this.postService.viewPost(this.post.id).subscribe(Response => {
          console.log(Response);
          if (Response.ok) this.viewed = true;
        });
      }
    });
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

  openEditDialog() {
    const dialogRef: MatDialogRef<PostEditorDialogComponent, {
      post: PagePostDTO,
      textContentOutput: string
    }> = this.dialog.open(PostEditorDialogComponent, {
      width: '500px',
      data: {post: this.post},
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === undefined) return;
      this.postService.putPost(this.post.id, {
        textContent: result.textContentOutput,
        images: []
      }).subscribe(response => {
          console.log(response);
          if (!response.ok) return;
          this.post.textContent = result.textContentOutput;
        }
      );
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
    return PostFormatter.getCircleColor(this.post.user.username);
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
        });
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
}
