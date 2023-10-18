import {Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import {PostDTO} from "../../models/post/postDTO";
import {UserDTO} from "../../models/user/userDTO";
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {CommentService} from "../../Services/comment.service";
import {PostService} from "../../Services/post.service";
import {UserService} from "../../Services/user.service";
import {UserHoverCardTriggerService} from "../../Services/user-hover-card-trigger.service";
import PostFormatter from "../../helpers/postFormatter";
import {CommentCreationDialogComponent} from "../comment-creation-dialog/comment-creation-dialog.component";
import {CommentCreateDialogData} from "../../models/coment/CommentCreateDialogData";
import {PostEditorDialogComponent} from "../post-editor-dialog/post-editor-dialog.component";
import {faBookmark as faBookmarkUnactivated, faComment, faHeart as faHeartUnactivated} from "@fortawesome/free-regular-svg-icons";
import {faBookmark as faBookmarkActivated, faRetweet, faHeart as faHeartActivated, faSquarePollVertical, faPen} from "@fortawesome/free-solid-svg-icons";
import {CommentDTO} from "../../models/coment/commentDTO";

@Component({
  selector: 'app-page-comment',
  templateUrl: './page-comment.component.html',
  styleUrls: ['./page-comment.component.scss', '../page-post/page-post.component.scss']
})
export class PageCommentComponent implements OnInit {
  faPen = faPen;
  faComment = faComment;
  faRetweet = faRetweet;
  faSquarePollVertical = faSquarePollVertical;
  faHeartActivated = faHeartActivated;
  faHeartUnactivated = faHeartUnactivated;
  faBookmarkActivated = faBookmarkActivated;
  faBookmarkUnactivated = faBookmarkUnactivated;

  @Input() public commentInput!: CommentDTO;
  @Input() public userInput!: UserDTO;
  @Input() public isParentView: boolean = false;
  commentView: PagePostDTO = {} as PagePostDTO;

  @ViewChild('userInfo') userInfo: ElementRef<HTMLDivElement>;
  @ViewChild('wholePost') wholePost: ElementRef<HTMLDivElement>;

  private observer: IntersectionObserver;

  viewed: boolean = false;
  editable: boolean = false;
  liked: boolean = false;
  bookmarked: boolean;

  constructor(public dialog: MatDialog,
              private readonly commentService: CommentService,
              private readonly userService: UserService,
              private readonly hoverCardTriggerService: UserHoverCardTriggerService) {
  }

  ngOnInit(): void {
    console.log(this.commentInput);
    this.userService.getCurrentUser().subscribe(response => {
      if (response.ok) {
        this.editable = response.body!.id === this.commentInput.author.id;
        this.liked = this.commentInput.likesIds.includes(response.body!.id);
        this.bookmarked = response.body!.bookmarkedPostsIds.includes(this.commentInput.id);
      }
    });
    this.commentView = PostFormatter.mapCommentToPagePost(this.commentInput, this.userInput);
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
        this.commentService.viewComment(this.commentView.id).subscribe(Response => {
          console.log(Response);
          if (Response.ok) {
            this.viewed = true;
            this.commentView.viewsAmount++;
          }
        });
      }
    });
  }

  getFirstInitial(): string {
    return this.commentView.user.username[0].toUpperCase();
  }


  public getCreatedDate(): string {
    const date = new Date(this.commentView.dateCreated);
    return PostFormatter.getDateFormattedString(date);
  }

  isAvatarNull(): boolean {
    return this.commentView.user.avatar === null;
  }

  handleCommentClick() {
    this.openCommentDialog();
  }

  openCommentDialog() {
    const dialogRef: MatDialogRef<CommentCreationDialogComponent, CommentCreateDialogData> = this.dialog.open(CommentCreationDialogComponent, {
      width: '500px',
      data: {post: this.commentView, currentUser: this.commentView.user, textContent: "", images: []}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === undefined) return;
      console.log('The dialog was closed');
      console.log(result);
      console.log(this.commentView);
      this.commentService.postComment({
        commentId: this.commentView.id,
        textContent: result.textContent,
        images: result.images
      }).subscribe(response => {
        console.log(response)
        if (!response.ok) return;
        this.commentView.commentsAmount++;
      });
    });
  }

  openEditDialog() {
    const dialogRef: MatDialogRef<PostEditorDialogComponent, {
      post: PagePostDTO,
      textContentOutput: string
    }> = this.dialog.open(PostEditorDialogComponent, {
      width: '500px',
      data: {post: this.commentView},
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === undefined) return;
      this.commentService.putComment(this.commentView.id, {
        textContent: result.textContentOutput,
        images: []
      }).subscribe(response => {
          console.log(response);
          if (!response.ok) return;
          this.commentView.textContent = result.textContentOutput;
        }
      );
    });
  }

  handleLikeClick() {
    console.log("Like clicked");
    switch (this.liked) {
      case true:
        this.commentView.likesAmount--;
        this.liked = false;
        this.commentService.unlikeComment(this.commentView.id).subscribe(response => console.log(response));
        break;
      case false:
        this.commentView.likesAmount++;
        this.liked = true;
        this.commentService.likeComment(this.commentView.id).subscribe(response => console.log(response));
        break;
    }
  }

  getCircleColor() {
    return PostFormatter.getCircleColor(this.commentView.user.username);
  }

  async onUserInfoMouseLeave() {
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
    this.hoverCardTriggerService.user = this.commentView.user;
    this.hoverCardTriggerService.enableHoverCardVisibility();
    this.hoverCardTriggerService.isHoveredOnTriggeringElement = true;
    console.log(this.userInfo.nativeElement.getBoundingClientRect());
    this.hoverCardTriggerService.coordinates = {
      x: this.userInfo.nativeElement.getBoundingClientRect().x - 60,
      y: this.userInfo.nativeElement.getBoundingClientRect().y + document.documentElement.scrollTop + 20
    };
  }

  handleBookmarkClick() {
    console.log("Bookmark clicked");
    switch (this.bookmarked) {
      case true:
        this.bookmarked = false;
        this.commentView.bookmarksAmount--;
        this.commentService.undoBookmarkComment(this.commentView.id).subscribe(Response => {
          if (!Response.ok) {
            this.bookmarked = true;
            this.commentView.bookmarksAmount++;
          }
          console.log(Response)
        });
        break;
      case false:
        this.bookmarked = true;
        this.commentView.bookmarksAmount++;
        this.commentService.bookmarkComment(this.commentView.id).subscribe(Response => {
          if (!Response.ok) {
            this.bookmarked = false;
            this.commentView.bookmarksAmount--;
          }
          console.log(Response)
        });
        break;
    }
  }
}
