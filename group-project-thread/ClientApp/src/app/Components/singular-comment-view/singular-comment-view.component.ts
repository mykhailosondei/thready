import {Component, ElementRef, OnInit, TemplateRef, ViewChild} from '@angular/core';
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
import {PostEditorDialogComponent} from "../post-editor-dialog/post-editor-dialog.component";
import {DeleteDialogComponent} from "../delete-dialog/delete-dialog.component";
import {Location} from "@angular/common";
import {BsModalRef, BsModalService} from "ngx-bootstrap/modal";

@Component({
  selector: 'app-singular-comment-view',
  templateUrl: './singular-comment-view.component.html',
  styleUrls: ['./singular-comment-view.component.scss', "../singular-post-view/singular-post-view.component.scss", '../page-post/page-post.component.scss']
})
export class SingularCommentViewComponent {

  faBookmarkActivated = faBookmarkActivated;
  faBookmarkUnactivated = faBookmarkUnactivated;
  faHeartActivated = faHeartActivated;
  faHeartUnactivated = faHeartUnactivated;

  incomingCommentId : number = 0;
  timestamp : number = 0;
  public authorInput : UserDTO;
  public commentInput : CommentDTO;
  public comments : CommentDTO[] = [];
  public commentView : PagePostDTO;
  public parentComments : (CommentDTO)[] = [];
  public parentPost : PostDTO;
  public currentUser : UserDTO;
  @ViewChild('userInfo') userInfo: ElementRef<HTMLDivElement>;
  @ViewChild('imageRef') imageRef: TemplateRef<any>;
  modalRef: BsModalRef;
  clickedImage: string = "";

  liked: boolean = false;
  reposted: boolean = false;
  bookmarked: boolean = false;
  editable: boolean = false;
  constructor(private route : ActivatedRoute, private userService : UserService,
              private commentService : CommentService,
              private hoverCardTriggerService: UserHoverCardTriggerService,
              private dialog: MatDialog,
              private location: Location,
              private modalService: BsModalService){
    this.route.paramMap.subscribe(params => {
      const postId = params.get('commentId');
      console.log(postId);
      if (postId) {
        this.incomingCommentId = Number.parseInt(postId);
      }
    });
  }
  ngOnInit(): void {
    this.fetchEssentialData();
  }

  openFullImage(url: string) {
    this.clickedImage = url;
    this.modalRef = this.modalService.show(this.imageRef, {class: 'modal-lg'});
  }

  fetchEssentialData() {
    this.commentService.getCommentTreeById(this.incomingCommentId).subscribe(response => {
      if(response.ok) {
        this.commentInput = response.body!;
        this.commentInput.id = this.incomingCommentId;
        this.authorInput = this.commentInput.author;
        this.commentView = PostFormatter.mapCommentToPagePost(this.commentInput, this.authorInput);
        console.log("=============commentWithTree=============");
        console.log(response);
      }
    }).add(()=>{
      this.fetchComments();
      this.retrieveParents(this.commentInput);
      this.fetchCurrentUser();
    });
  }

  fetchCurrentUser() {
    this.userService.getCurrentUser().subscribe(response => {
      if(response.ok){
        this.currentUser = response.body!;
        this.editable = response.body!.id == this.authorInput.id;
      }
    });
  }

  fetchComments() {
    console.log(this.commentInput.commentsIds);
    for (let commentId of this.commentInput.commentsIds) {
      this.commentService.getCommentById(commentId).subscribe(response => {
        if(response.ok) {
          let comment: CommentDTO = response.body!;
          comment.id = commentId;
          this.comments.push(comment);
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
    const dialogRef: MatDialogRef<CommentCreationDialogComponent, { imagesOutput: string[], textOutput:string }> = this.dialog.open(CommentCreationDialogComponent, {
      width: '500px',
      data: {post: this.commentView, currentUser: this.currentUser, textContent: "", images: []}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === undefined) return;
      console.log('The dialog was closed');
      console.log(result);
      this.commentService.postComment({
        commentId: this.commentView.id,
        textContent: result.textOutput,
        images: result.imagesOutput.map(i => {return {url: i}})
      }).subscribe(response => {
        console.log(response)
        if (!response.ok) return;
        this.commentView.commentsAmount++;
      });
    });
  }

  retrieveParents(comment: CommentDTO) {
    if (comment.parentComment) {
      this.userService.getUserById(comment.parentComment.userId).subscribe(response => {
        if (response.ok) {
          comment.parentComment!.author = response.body!;
        }
      }).add(() => {
        this.parentComments.unshift(comment.parentComment!);
        this.retrieveParents(comment.parentComment!);
      });
    } else if (comment.post) {
      this.parentPost = comment.post;
    }
  }

  handleLikeClick() {
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

  protected readonly faPen = faPen;

  handleBookmarkClick() {
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
        }).add(() => console.log("Completed"))
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
    }
  }

  getCreatedTime() {
    return PostFormatter.getTimeFormatted(new Date(this.commentInput.createdAt));
  }

  getCreatedDate() {
    return PostFormatter.getDateFormatted(new Date(this.commentInput.createdAt));
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
    this.hoverCardTriggerService.user = this.commentView.user;
    this.hoverCardTriggerService.enableHoverCardVisibility();
    this.hoverCardTriggerService.isHoveredOnTriggeringElement = true;
    this.hoverCardTriggerService.coordinates = {
      x: this.userInfo.nativeElement.getBoundingClientRect().x - 60,
      y: this.userInfo.nativeElement.getBoundingClientRect().y + document.documentElement.scrollTop + 20
    };
  }

  handleCommentCreation() {
    this.commentView.commentsAmount++;
    this.commentService.getCommentById(this.incomingCommentId).subscribe(response => {
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

    protected readonly Endpoint = Endpoint;
  protected readonly faTrash = faTrash;

  openDeleteDialog() {
    const dialogRef : MatDialogRef<DeleteDialogComponent, boolean> = this.dialog.open(DeleteDialogComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.commentService.deleteComment(this.commentView.id).subscribe(response => {
          console.log(response);
          if (!response.ok) return;
          this.location.back();
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
      data: {post: this.commentView},
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === undefined) return;
      this.commentService.putComment(this.commentView.id, {
        textContent: result.textContentOutput,
        images: result.imagesOutput.map(i => {return {url: i}})
      }).subscribe(response => {
          console.log(response);
          if (!response.ok) return;
          this.commentView.textContent = result.textContentOutput;
          this.commentView.imagesUrls = result.imagesOutput;
        }
      );
    });
  }
}
