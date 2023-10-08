import {Image} from "../image";
import {UserWithPostDTO} from "../user/UserWithinPostDTO";

export interface PagePostDTO{
  id : number;
  textContent: string;
  images: Image[];
  user: UserWithPostDTO;
  commentsAmount: number;
  repostsAmount: number;
  likesAmount: number;
  viewsAmount: number;
  dateCreated: string;
}
