import {Image} from "../image";

export interface PageUserDTO {
  id: number;
  username: string;
  avatar: Image | null;
  bio: string;
  followers: number;
  following: number;
}
