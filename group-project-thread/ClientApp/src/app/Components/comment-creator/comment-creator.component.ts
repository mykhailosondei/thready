import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import PostFormatter from "../../helpers/postFormatter";
import {CommentService} from "../../Services/comment.service";
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {User} from "oidc-client";
import {faTimes} from "@fortawesome/free-solid-svg-icons";
import {ImageUploadService} from "../../Services/image-upload.service";

@Component({
  selector: 'app-comment-creator',
  templateUrl: './comment-creator.component.html',
  styleUrls: ['./comment-creator.component.scss', '../page-post/page-post.component.scss']
})
export class CommentCreatorComponent implements OnInit {

  @Input() post: PagePostDTO;
  currentUser: UserDTO;
  @Input() replyArgs: {isCommentReply: boolean} = {isCommentReply: false};
  @Output() onReplyClickEvent = new EventEmitter();
  imageUrls: string[] = [];
  constructor(private commentService : CommentService,
              private userService : UserService,
              private imageUploadService : ImageUploadService) { }

  @ViewChild('inputComponent') commentInput: {inputValue: string} = {inputValue: ""};

  ngOnInit(): void {
    this.setCurrentUser();
  }

  isButtonDisabled() {
    if(this.imageUrls.length > 0) return false;
    return PostFormatter.isInputLengthTooBig(this.commentInput.inputValue) || this.commentInput.inputValue == "";
  }

  setCurrentUser() {
    this.userService.getCurrentUser().subscribe(Response => {
      if(Response.ok){
        this.currentUser = Response.body!;
      }
    });
  }
  onReplyClick() {
    if(!this.replyArgs.isCommentReply) {
      this.commentService.postComment({postId:this.post.id, textContent: this.commentInput.inputValue, images:this.imageUrls.map(i => {return{url:i}})}).subscribe(Response => {
        this.commentInput.inputValue = "";
        this.imageUrls = [];
        console.log(Response);

        if(Response.ok){
          this.onReplyClickEvent.emit();
        }
      });
    }
    else {
      this.commentService.postComment({commentId:this.post.id, textContent: this.commentInput.inputValue, images:this.imageUrls.map(i => {return{url:i}})}).subscribe(Response => {
        this.commentInput.inputValue = "";
        this.imageUrls = [];
        console.log(Response);
        if(Response.ok){
          this.onReplyClickEvent.emit();
        }
      });
    }
  }

    protected readonly faTimes = faTimes;

  deleteImage($event: string) {
    this.imageUrls = this.imageUrls.filter(i => i !== $event);
    var deletionName = $event.split('/').pop()!;
    this.imageUploadService.deleteImage(deletionName).subscribe();
  }

  onPhotoLoaded($event: string) {
    this.imageUrls.push($event);
  }
}
