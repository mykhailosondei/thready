import {Image} from "../image";

export interface CommentCreateDTO{
    authorId?: number;
    postId?: number | null;
    commentId?: number | null;
    textContent: string;
    images: Image[];

}
