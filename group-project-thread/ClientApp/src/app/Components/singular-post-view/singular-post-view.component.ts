import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {faBookmark as faBookmarkUnactivated, faComment, faHeart as faHeartUnactivated} from "@fortawesome/free-regular-svg-icons";
import {
  faBookmark as faBookmarkActivated,
  faEllipsisH,
  faRetweet,
  faSquarePollVertical,
  faTrash
} from "@fortawesome/free-solid-svg-icons";
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
import {Endpoint} from "../side-navbar/side-navbar.component";
import {DeleteDialogComponent} from "../delete-dialog/delete-dialog.component";
import {Location} from "@angular/common";
import {PostEditorDialogComponent} from "../post-editor-dialog/post-editor-dialog.component";
import {User} from "oidc-client";

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
    public currentUser : UserDTO;
    @ViewChild('userInfo') userInfo: ElementRef<HTMLDivElement>;

    editable: boolean = false;
    liked: boolean = false;
    reposted: boolean = false;
    bookmarked: boolean = false;
    constructor(private route : ActivatedRoute, private userService : UserService,
                private postService : PostService, private commentService : CommentService,
                private hoverCardTriggerService: UserHoverCardTriggerService,
                private dialog: MatDialog,
                private readonly location: Location ) {
      this.route.paramMap.subscribe(params => {
        this.incomingUsername = params.get('username') || 'DefaultUsername';
        const postId = params.get('id');
        if (postId) {
          this.incomingPostId = Number.parseInt(postId);
        }
      });
    }
    ngOnInit(): void {
      this.fetchEssentialData();
    }

    fetchEssentialData() {
      this.userService.getUserByUsername(this.incomingUsername).subscribe(response => {
        if(response.ok) {
          this.authorInput = response.body!;
          console.log(response);
        }
      });
      this.postService.getPostById(this.incomingPostId).subscribe(response => {
        if(response.ok) {
          this.postInput = response.body!;
          this.post = PostFormatter.mapPostToPagePost(this.postInput);
        console.log(response);
        }
      }).add(()=>this.fetchComments());

      this.userService.getCurrentUserInstance()
        .subscribe(response => {
          this.currentUser = response;
          this.bookmarked = this.currentUser.bookmarkedPostsIds.includes(this.incomingPostId);
          this.editable = this.currentUser.username == this.incomingUsername;
        })
    }

    fetchComments() {
      console.log(this.postInput.commentsIds);
      /*for (let commentId of this.postInput.commentsIds) {
        this.commentService.getCommentById(commentId).subscribe(response => {
          if(response.ok) {
            let comment: CommentDTO = response.body!;
            comment.id = commentId;
            this.comments.push(comment);
          }
        });
      }*/
      this.commentService.getCommentsOfPostId(this.incomingPostId).subscribe(response => {
        if(response.ok) {
          this.comments = response.body!;
          console.log(response);
        }
      });
    }

    faRetweet = faRetweet;
    faComment = faComment;

  handleCommentClick() {
    this.openCommentDialog();
  }

  openCommentDialog() {
    const dialogRef: MatDialogRef<CommentCreationDialogComponent, { imagesOutput: string[], textOutput:string }> = this.dialog.open(CommentCreationDialogComponent, {
      width: '500px',
      data: {post: this.post, currentUser: this.currentUser, textContent: "", images: []}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === undefined) return;
      console.log('The dialog was closed');
      console.log(result);
      this.commentService.postComment({
        postId: this.post.id,
        textContent: result.textOutput,
        images: result.imagesOutput.map(i => {return {url: i}})
      }).subscribe(response => {
        console.log(response)
        if (!response.ok) return;
        this.post.commentsAmount++;
      });
    });
  }

  handleRepostClick() {
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


  protected readonly faPen = faPen;

  handleBookmarkClick() {
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

  getFormattedNumber(number: number) {
    return PostFormatter.numberToReadable(number);
  }

  getCreatedTime() {
    return PostFormatter.getTimeFormatted(new Date(this.postInput.createdAt));
  }

  getCreatedDate() {
    return PostFormatter.getDateFormatted(new Date(this.postInput.createdAt));
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
    this.hoverCardTriggerService.user = this.post.user;
    this.hoverCardTriggerService.enableHoverCardVisibility();
    this.hoverCardTriggerService.isHoveredOnTriggeringElement = true;
    this.hoverCardTriggerService.coordinates = {
      x: this.userInfo.nativeElement.getBoundingClientRect().x - 60,
      y: this.userInfo.nativeElement.getBoundingClientRect().y + document.documentElement.scrollTop + 20
    };
  }

  handleCommentCreation() {
    this.post.commentsAmount++;
    this.postService.getPostById(this.post.id).subscribe(response => {
      if(response.ok) {
        let lastId = response.body!.commentsIds[response.body!.commentsIds.length - 1];
        this.commentService.getCommentById(lastId).subscribe(response => {
          if(response.ok) {
            let commentToAdd: CommentDTO = response.body!;
            commentToAdd.id = lastId;
            this.comments.unshift(commentToAdd);
          }
        });
      }
    });
  }

  onCommentDelete($event: CommentDTO) {
    this.comments =  this.comments.filter(comment => comment.id !== $event.id);
  }

  protected readonly Endpoint = Endpoint;
  protected readonly faTrash = faTrash;


  openDeleteDialog() {
    const dialogRef : MatDialogRef<DeleteDialogComponent, boolean> = this.dialog.open(DeleteDialogComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.postService.deletePost(this.post.id).subscribe(response => {
          if(response.ok) {
            console.log("Post deleted");
            this.location.back();
          }
        });
      }
    });
  }

  openEditDialog() {
    const dialogRef: MatDialogRef<PostEditorDialogComponent, {
      imagesOutput: string[],
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
        images: result.imagesOutput.map(i => {return {url: i}})
      }).subscribe(response => {
          console.log(response);
          if (!response.ok) return;
          this.post.textContent = result.textContentOutput;
          this.post.imagesUrls = result.imagesOutput;
        }
      );
    });
  }
}
