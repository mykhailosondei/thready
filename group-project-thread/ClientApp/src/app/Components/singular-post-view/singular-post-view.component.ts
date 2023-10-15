import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {UserService} from "../../Services/user.service";
import {UserDTO} from "../../models/user/userDTO";
import {PostDTO} from "../../models/post/postDTO";
import {PostService} from "../../Services/post.service";
import {CommentDTO} from "../../models/coment/commentDTO";
import {CommentService} from "../../Services/comment.service";

@Component({
  selector: 'app-singular-post-view',
  templateUrl: './singular-post-view.component.html',
  styleUrls: ['./singular-post-view.component.scss']
})
export class SingularPostViewComponent implements OnInit{

    incomingUsername : string = "";
    incomingPostId : number = 0;
    public author : UserDTO = {} as UserDTO;
    public post : PostDTO= {} as PostDTO;
    public comments : CommentDTO[] = [];
    constructor(private route : ActivatedRoute, private userService : UserService, private postservice : PostService, private commentService : CommentService) {
      this.route.paramMap.subscribe(params => {
        this.incomingUsername = params.get('username') || 'DefaultUsername';
        const postId = params.get('id');
        if (postId) {
          this.incomingPostId = Number.parseInt(postId);
        }
      });
    }
    ngOnInit(): void {
      this.fetchEssentialData()
      this.fetchComments();
    }

    fetchEssentialData() {
      this.userService.getUserByUsername(this.incomingUsername).subscribe(response => {
        if(response.ok) {
          this.author = response.body!;
        }
      });
      this.postservice.getPostById(this.incomingPostId).subscribe(response => {
        if(response.ok) {
          this.post = response.body!;
        }
      });
    }

    fetchComments() {
      for (let commentId of this.post.commentsIds) {
        this.commentService.getCommentById(commentId).subscribe(response => {
          if(response.ok) {
            this.comments.push(response.body!);
          }
        });
      }
    };


}
