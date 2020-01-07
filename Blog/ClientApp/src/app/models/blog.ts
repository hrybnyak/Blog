import { Post } from "./post";

export class Blog {
    id: number;
    name: string;
    ownerId: string;
    ownerUsername: string;
    articles: Post[];
}