import {Image} from "../image";

export interface CommentUpdateDTO{
  id: number;
  textContent: string;
  images: Image[];
}
