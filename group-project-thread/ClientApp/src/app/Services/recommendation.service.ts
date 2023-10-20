import { Injectable } from '@angular/core';
import {HttpInternalService} from "./http-internal.service";
import {Observable} from "rxjs";
import {HttpResponse} from "@angular/common/http";
import {IndexedWordDTO} from "../models/indexedWordDTO";
import {PostDTO} from "../models/post/postDTO";
import {PageUserDTO} from "../models/user/pageUserDTO";

@Injectable({
  providedIn: 'root'
})
export class RecommendationService {

  private route : string = "/api/recommendation"
  constructor(private httpInternalService : HttpInternalService) {

  }

  getSmallTrends() : Observable<HttpResponse<IndexedWordDTO[]>>{
    return this.httpInternalService.getFullRequest<IndexedWordDTO[]>(`${this.route}/small_trends`);
  }

  getTrends() : Observable<HttpResponse<IndexedWordDTO[]>>{
    return this.httpInternalService.getFullRequest<IndexedWordDTO[]>(`${this.route}/trends`);
  }
  getPostsForYou(userId : number) : Observable<HttpResponse<PostDTO[]>>{
    return this.httpInternalService.getFullRequest<PostDTO[]>(`${this.route}/for_you/${userId}`)
  }

  getWhoToFollow() : Observable<HttpResponse<PageUserDTO[]>>{
    return this.httpInternalService.getFullRequest<PageUserDTO[]>(`${this.route}/who_to_follow`)
  }
  getConnectPeople() : Observable<HttpResponse<PageUserDTO[]>>{
    return this.httpInternalService.getFullRequest<PageUserDTO[]>(`${this.route}/connect_people`)
  }

  getCreatorsForYou() : Observable<HttpResponse<PageUserDTO[]>>{
    return this.httpInternalService.getFullRequest<PageUserDTO[]>(`${this.route}/creators_for_you`)
  }
}
