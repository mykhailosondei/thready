import { Injectable } from '@angular/core';
import {HttpResponse} from "@angular/common/http";
import {PageUserDTO} from "../models/user/pageUserDTO";
import {SearchService} from "./search.service";
import {BehaviorSubject} from "rxjs";
import {PostDTO} from "../models/post/postDTO";

@Injectable({
  providedIn: 'root'
})
export class DataLoadingService {

  constructor(private searchService : SearchService) { }


  public loadInitialPeople(query: string, peopleToLoadLowerCount : number, peopleToLoadUpperCount : number,
                    matchingUsers$ : BehaviorSubject<PageUserDTO[]>){
    this.searchService.getUsers(query, peopleToLoadLowerCount, peopleToLoadUpperCount).subscribe(
      (users : HttpResponse<PageUserDTO[]>) => {
        matchingUsers$.next(users.body || []);
      }
    );
  }

  public loadInitialPosts(query: string, postsToLoadLowerCount : number, postsToLoadUpperCount : number,
                           matchingPosts$ : BehaviorSubject<PostDTO[]>){
    this.searchService.getPosts(query, postsToLoadLowerCount, postsToLoadUpperCount).subscribe(
      (users : HttpResponse<PostDTO[]>) => {
        matchingPosts$.next(users.body || []);
      }
    );
  }

  loadMorePeople(allPeopleLoaded : boolean, query : string, peopleToLoadLowerCount : number,
                 peopleToLoadUpperCount: number, matchingUsers$ : BehaviorSubject<PageUserDTO[]>){
    if (allPeopleLoaded) return;
    this.searchService.getUsers(query, peopleToLoadLowerCount, peopleToLoadUpperCount).subscribe(
      (users : HttpResponse<PageUserDTO[]>) => {
        const newUsers = users.body || [];
        if (newUsers.length == 0){
          allPeopleLoaded = true;
          return;
        }
        const currentUsers = matchingUsers$.getValue();
        matchingUsers$.next([...currentUsers, ...newUsers]);
      }
    );
  }

  loadMorePosts(allPostsLoaded : boolean, query : string, postsToLoadLowerCount : number,
                postsToLoadUpperCount: number, matchingPosts$ : BehaviorSubject<PostDTO[]>){
    if (allPostsLoaded) return;
    this.searchService.getPosts(query, postsToLoadLowerCount, postsToLoadUpperCount).subscribe(
      (posts : HttpResponse<PostDTO[]>) => {
        const newPosts = posts.body || [];
        if (newPosts.length == 0){
          allPostsLoaded = true;
          return;
        }
        const currentPosts = matchingPosts$.getValue();
        matchingPosts$.next([...currentPosts, ...newPosts]);
      },
      () => {

      }
    );
  }
}
