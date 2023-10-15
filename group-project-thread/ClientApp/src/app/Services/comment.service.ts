import { Injectable } from '@angular/core';
import {HttpInternalService} from "./http-internal.service";
import {HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {CommentDTO} from "../models/coment/commentDTO";
import {CommentCreateDTO} from "../models/coment/commentCreateDTO";
import {CommentUpdateDTO} from "../models/coment/commentUpdateDTO";

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private route : string = "/api/comment"
  constructor(private httpInternalService: HttpInternalService) { }

  public getCommentById(id: number): Observable<HttpResponse<CommentDTO>> {
    return this.httpInternalService.getFullRequest<CommentDTO>(`${this.route}/${id}`);
  }

  public getCommentsOfPostId(postId: number): Observable<HttpResponse<CommentDTO[]>>{
    return this.httpInternalService.getFullRequest<CommentDTO[]>(`${this.route}/${postId}/comments`);
  }

  public postComment(comment: CommentCreateDTO): Observable<HttpResponse<void>>{
    return this.httpInternalService.postFullRequest<void>(this.route, comment);
  }

  public likeComment(id: number): Observable<HttpResponse<void>>{
    return this.httpInternalService.postFullRequest<void>(`${this.route}/${id}/likeComment`, {});
  }

  public unlikeComment(id: number): Observable<HttpResponse<void>>{
    return this.httpInternalService.postFullRequest<void>(`${this.route}/${id}/unlikeComment`, {});
  }
  public putComment(id: number, comment: CommentUpdateDTO): Observable<HttpResponse<void>>{
    return this.httpInternalService.putFullRequest<void>(`${this.route}/${id}`, comment);
  }

  public deleteComment(id: number): Observable<HttpResponse<void>>{
    return this.httpInternalService.deleteFullRequest<void>(`${this.route}/${id}`);
  }

  public viewComment(id: number) {
    console.log("viewing comment with id: " + id);
  }

  public bookmarkComment(id: number){
    console.log("bookmarking comment with id: " + id);
  }

  public undoBookmarkComment(id: number){
    console.log("unbookmarking comment with id: " + id);
  }
}
