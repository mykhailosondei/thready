import {Image} from "../image";

export interface UserWithPostDTO {
  id: number;
  username: string;
  avatar: Image | null;
  bio: string;
  followers: number;
  following: number;
}
