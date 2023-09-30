import {Image} from "../image";

export interface PagePostDTO{
  id : number;
  textContent: string;
  images: Image[];
  commentsAmount: number;
  repostsAmount: number;
  likesAmount: number;
  viewsAmount: number;
  dateCreated: string;
}
