import { Comment } from "./comment";
import { Category } from "./category";

export class Post {
    id: number;
    name: string;
    content: string;
    lastUpdate: Date;
    blogId: number;
    comments: Comment[];
    tegs: Category[];
    authorId: string;
    authorUsername: string;
}