import {Image} from "../image";

export interface CommentCreateDTO{
    authorId?: number;
    postId?: number | null;
    parentCommentId?: number | null;
    textContent: string;
    images: Image[];

}
