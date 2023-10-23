import {Component, OnInit} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {PostDTO} from "../../models/post/postDTO";
import {PostService} from "../../Services/post.service";
import {UserService} from "../../Services/user.service";
import {UserDTO} from "../../models/user/userDTO";
import {faBookmark} from "@fortawesome/free-solid-svg-icons";
import {Endpoint} from "../side-navbar/side-navbar.component";
import {ProgressSpinnerMode} from "@angular/material/progress-spinner";

@Component({
  selector: 'app-bookmarks-page',
  templateUrl: './bookmarks-page.component.html',
  styleUrls: ['./bookmarks-page.component.scss', '../../../assets/spinner.scss']
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
              this.loading = false;
              this.posts.push(Response.body!);
            }
          });
        }
      }
    });
  }

  protected readonly faBookmark = faBookmark;
  protected readonly Endpoint = Endpoint;
}
