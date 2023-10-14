import {Image} from "../image";

export interface UserUpdateDTO {
  id: number;
  bio: string;
  location: string;
  avatar : Image | null;
}
