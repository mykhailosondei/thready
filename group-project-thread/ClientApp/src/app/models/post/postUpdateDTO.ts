import {Image} from "../image";

export interface PostUpdateDTO {
  id?: number;
  textContent: string;
  images: Image[]
}
