import { Image } from "../image";

export interface PostCreateDTO{
    authorId: number;
    textContent: string;
    images: Image[];
}