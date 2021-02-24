import { User } from "./user";

export class Comment {
    id: number;
    content: string;
    lastUpdated: Date;
    creatorUsername: string;
    creatorId: string;
    articleId: number;
}