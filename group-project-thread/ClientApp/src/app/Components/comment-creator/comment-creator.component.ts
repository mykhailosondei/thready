import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import PostFormatter from "../../helpers/postFormatter";
import {CommentService} from "../../Services/comment.service";
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {User} from "oidc-client";

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
  constructor(private commentService : CommentService, private userService : UserService) { }

  @ViewChild('inputComponent') commentInput: {inputValue: string} = {inputValue: ""};

  ngOnInit(): void {
    this.setCurrentUser();
  }

  isButtonDisabled() {
    return PostFormatter.isInputLengthInvalid(this.commentInput.inputValue);
  }

  setCurrentUser() {
    this.userService.getCurrentUser().subscribe(Response => {
      if(Response.ok){
        this.currentUser = Response.body!;
      }
    });
  }

  getCircleColor(user: UserDTO) {
    return PostFormatter.getCircleColor(user.username);
  }

  isAvatarNull(user: UserDTO) {
    return user.avatar === null;
  }

  getFirstInitial(user: UserDTO) {
    return user.username[0].toUpperCase();
  }


  onReplyClick() {
    if(!this.replyArgs.isCommentReply) {
      this.commentService.postComment({postId:this.post.id, textContent: this.commentInput.inputValue, images:[]}).subscribe(Response => {
        this.commentInput.inputValue = "";
        console.log(Response);

        if(Response.ok){
          this.onReplyClickEvent.emit();
        }
      });
    }
    else {
      this.commentService.postComment({commentId:this.post.id, textContent: this.commentInput.inputValue, images:[]}).subscribe(Response => {
        this.commentInput.inputValue = "";
        console.log(Response);
        if(Response.ok){
          this.onReplyClickEvent.emit();
        }
      });
    }
  }
}
