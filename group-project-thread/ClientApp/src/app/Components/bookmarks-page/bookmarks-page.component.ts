import {Component, OnInit} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {PostDTO} from "../../models/post/postDTO";
import {PostService} from "../../Services/post.service";
import {UserService} from "../../Services/user.service";
import {UserDTO} from "../../models/user/userDTO";

@Component({
  selector: 'app-bookmarks-page',
  templateUrl: './bookmarks-page.component.html',
  styleUrls: ['./bookmarks-page.component.scss']
})
export class BookmarksPageComponent implements OnInit {
  posts: PostDTO[] = [];
  user: UserDTO;
  loading: boolean = false;


  constructor(private userService : UserService, private postService:PostService) {}

  ngOnInit(): void {
    this.loading = true;
    this.userService.getCurrentUser().subscribe(Response => {
      if(Response.ok){
        this.user = Response.body!;
        for(let bookmarkId of this.user.bookmarkedPostsIds) {
          this.postService.getPostById(bookmarkId).subscribe(Response => {
            if(Response.ok) {
              this.posts.push(Response.body!);
            }
          });
        }
      }
    }).add(() => this.loading = false);
  }
}
