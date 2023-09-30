import {Image} from "../image";

export interface ThreadCommentDTO{
  id: number;
  textContent: string;
  images: Image[];
  dateCreated: string;
  commentsAmount: number;
  repostsAmount: number;
  likesAmount: number;
  viewsAmount: number;
}
