import { Image } from "./image";
import { PostDTO } from "./post/postDTO";

export interface UserDTO{
    id : number;
    username : string;
    email : string;
    dateOfBirth : Date;
    password : string;
    imageID: number | null;
    avatar: Image | null;
    posts: PostDTO[];
    followersIds: number[];
    followingIds: number[];
    bio: string;
    location: string;
    bookmarkedPostsIds: number[];
    repostsIds: number;
    
}

