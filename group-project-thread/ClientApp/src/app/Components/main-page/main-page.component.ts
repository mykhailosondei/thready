import {Component} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {HttpInternalService} from "../../Services/http-internal.service";
import {PostDTO} from "../../models/post/postDTO";
import {Router} from "@angular/router";

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent {

  posts: PostDTO[] = [];
  id: string | null = null;

  constructor(private httpService: HttpInternalService, private router : Router) {
  }

  ngOnInit(): void {
  }

  public getPostView(id: number) {
    this.httpService.getRequest<PostDTO>(`/api/post/${id}`).subscribe(
      Response => {
        console.log(Response);
        Response.images = [{id:0, url: "https://picsum.photos/1080"}]
        this.posts.push(Response);
        console.log(this.posts[this.posts.length - 1]);
        this.id = Response.textContent;
      }
    );
  }

  public logPost() {
    console.log(this.posts);
  }

  public navigateToProfile(){
    this.router.navigate(["dmytrosemeniuk", 'profile']);
  }

  protected readonly Number = Number;
}
