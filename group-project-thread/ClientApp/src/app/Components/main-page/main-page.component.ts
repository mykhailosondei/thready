import {Component} from '@angular/core';
import {PagePostDTO} from "../../models/post/pagePostDTO";
import {HttpInternalService} from "../../Services/http-internal.service";
import {PostDTO} from "../../models/post/postDTO";

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent {

  posts: PagePostDTO[] = [];
  id: string | null = null;

  constructor(private httpService: HttpInternalService) {
  }

  ngOnInit(): void {
  }

  public getPostView(id: number) {
    this.httpService.getRequest<PostDTO>(`/api/post/${id}`).subscribe(
      Response => {
        console.log(Response);
        this.posts.push({
            id: Response.id,
            textContent: Response.textContent,
            imagesUrls: ["https://picsum.photos/1920/1920"],
            user: {
              id: Response.author.id,
              username: Response.author.username,
              avatar: Response.author.avatar,
              bio: Response.author.bio,
              followers: Response.author.followersIds.length,
              following: Response.author.followingIds.length
            },
            commentsAmount: Response.commentsIds.length,
            repostsAmount: Response.repostersIds.length,
            likesAmount: Response.likesIds.length,
            viewsAmount: Response.viewedBy.length,
            dateCreated: Response.createdAt
          }
        );
        this.id = Response.textContent;
      }
    );
  }

  public logPost() {
    console.log(this.posts);
  }

  protected readonly Number = Number;
}
