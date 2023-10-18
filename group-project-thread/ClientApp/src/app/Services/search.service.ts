import { Injectable } from '@angular/core';
import {HttpInternalService} from "./http-internal.service";
import {PageUserDTO} from "../models/user/pageUserDTO";
import {PagePostDTO} from "../models/post/pagePostDTO";
import {PostDTO} from "../models/post/postDTO";

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private route : string = "/api/search"
  constructor(private httpInternalService : HttpInternalService) { }

  public getUsers(query: string, lowerCount : number, upperCount : number) {
    const url = `${this.route}/users?query=${query}&lowerCount=${lowerCount}&upperCount=${upperCount}`;
    return this.httpInternalService.getFullRequest<PageUserDTO[]>(url);
  }
  public getPosts(query: string, lowerCount : number, upperCount : number) {
    const url = `${this.route}/posts?query=${query}&lowerCount=${lowerCount}&upperCount=${upperCount}`;
    return this.httpInternalService.getFullRequest<PostDTO[]>(url);
  }
}
