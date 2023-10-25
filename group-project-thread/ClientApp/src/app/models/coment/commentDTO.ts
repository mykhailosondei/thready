import {Image} from "../image";
import {PostDTO} from "../post/postDTO";
import {UserDTO} from "../user/userDTO";


export interface CommentDTO {
    id: number;
    userId: number;
    author: UserDTO
    postId: number | null;
    post: PostDTO | null;
    commentId: number | null;
    parentComment: CommentDTO | null;
    createdAt: string;
    images: Image[];
    textContent: string;
    likesIds: number[];
    commentsIds: number[];
    viewedBy: number[];
}
