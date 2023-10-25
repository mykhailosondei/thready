import { Injectable } from '@angular/core';
import {HttpInternalService} from "./http-internal.service";
import {Observable} from "rxjs";
import {PostDTO} from "../models/post/postDTO";
import {HttpResponse} from "@angular/common/http";
import {PostCreateDTO} from "../models/post/postCreateDTO";
import {PostUpdateDTO} from "../models/post/postUpdateDTO";

@Injectable({
  providedIn: 'root'
})
export class PostService {

  private route : string = "/api/post"
  constructor(private httpInternalService : HttpInternalService) { }

  public getAllPost(): Observable<HttpResponse<PostDTO[]>>{
    return this.httpInternalService.getFullRequest<PostDTO[]>(this.route);
  }

  public getPostById(id : number) : Observable<HttpResponse<PostDTO>>{
    return this.httpInternalService.getFullRequest<PostDTO>(`${this.route}/${id}`);
  }

  public getPostsByUserId(userId: number) : Observable<HttpResponse<PostDTO[]>>{
    return this.httpInternalService.getFullRequest<PostDTO[]>(`/api/post/${userId}/posts`);
  }

  public createPost(post : PostCreateDTO) : Observable<HttpResponse<PostDTO>>{
    return  this.httpInternalService.postFullRequest<PostDTO>(this.route, post);
  }

  public likePost(id : number) : Observable<HttpResponse<void>>{
    return this.httpInternalService.postFullRequest<void>(`${this.route}/${id}/likePost`, {});
  }

  public unlikePost(id : number) : Observable<HttpResponse<void>>{
    return this.httpInternalService.postFullRequest<void>(`${this.route}/${id}/unlikePost`, {});
  }

  public bookmarkPost(id: number): Observable<HttpResponse<void>>{
    return this.httpInternalService.postFullRequest<void>(`${this.route}/${id}/bookmarkPost`, {});
  }
  public removeFromBookmarksPost(id: number): Observable<HttpResponse<void>>{
    return this.httpInternalService.postFullRequest<void>(`${this.route}/${id}/removeFromBookmarks`, {});
  }

  public repost(id : number) : Observable<HttpResponse<void>>{
    return this.httpInternalService.postFullRequest<void>(`${this.route}/${id}/repost`, {});
  }

  public undoRepost(id : number) : Observable<HttpResponse<void>>{
    return this.httpInternalService.postFullRequest<void>(`${this.route}/${id}/undoRepost`, {});
  }

  public viewPost(id: number): Observable<HttpResponse<void>>{
    return this.httpInternalService.postFullRequest<void>(`${this.route}/${id}/viewPost`, {});
  }

  public putPost(id:number, post: PostUpdateDTO): Observable<HttpResponse<void>>{
    return this.httpInternalService.putFullRequest<void>(`${this.route}/${id}`, post);
  }

  public deletePost(id: number) : Observable<HttpResponse<void>>{
    return this.httpInternalService.deleteFullRequest(`${this.route}/${id}/`)
  }

}
