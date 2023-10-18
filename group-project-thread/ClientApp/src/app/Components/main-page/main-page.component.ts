import {Component} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {HttpInternalService} from "../../Services/http-internal.service";
import {PostDTO} from "../../models/post/postDTO";
import {Router} from "@angular/router";
import {PostService} from "../../Services/post.service";

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent {

  posts: PostDTO[] = [];

  constructor(private postService: PostService, private router : Router) {
  }

  ngOnInit(): void {
    this.fetchPosts();
  }

  public fetchPosts() {
    this.postService.getAllPost().subscribe(response => {
      this.posts = response.body!;
    });

  }

  public logPost() {
    console.log(this.posts);
  }

  protected readonly Number = Number;
}
