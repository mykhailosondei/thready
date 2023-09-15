import { Image } from "../image";
import { PostDTO } from "../post/postDTO";

export interface Comment{
    id: number;
    userId: number;
    postId: number | null;
    post: PostDTO | null;
    commentId: number | null;
    parentComment: Comment | null;
    createdAt: Date;
    images: Image[];
    textContent: string;
    likesIds: number[];
    commentsIds: number[];
    viewedBy: number[];
}