import {Component, OnInit} from '@angular/core';
import {PostDTO} from "../../models/post/postDTO";
import {UserDTO} from "../../models/user/userDTO";
import {PostService} from "../../Services/post.service";
import {UserService} from "../../Services/user.service";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {faRetweet} from "@fortawesome/free-solid-svg-icons";
import {Endpoint} from "../side-navbar/side-navbar.component";
import {F} from "@angular/cdk/keycodes";

@Component({
  selector: 'app-reposts-page',
  templateUrl: './reposts-page.component.html',
  styleUrls: ['./reposts-page.component.scss']
})
export class RepostsPageComponent implements OnInit{
  posts: PostDTO[] = [];
  user: UserDTO;
  loading: boolean = false;

  constructor(
    private userService : UserService,
    private postService:PostService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.userService.getCurrentUser().subscribe(Response => {
      if(Response.ok){
        this.user = Response.body!;
        for(let repostId of this.user.repostsIds) {
          this.postService.getPostById(repostId).subscribe(Response => {
            this.loading = false;
            if(Response.ok) {
              this.posts.push(Response.body!);
            }
          });
        }
      }
    })
  }

  get pageUser(): PageUserDTO {
    return {
      id: this.user.id,
      username: this.user.username,
      avatar: this.user.avatar,
      following: this.user.followingIds.length,
      followers: this.user.followersIds.length,
      bio: this.user.bio
    };
  }

  protected readonly faRetweet = faRetweet;
    protected readonly Endpoint = Endpoint;
}
