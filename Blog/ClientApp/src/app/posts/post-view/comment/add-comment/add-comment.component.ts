import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { DataService } from '../../../../services/data.service';
import { ActivatedRoute } from '@angular/router';
import { Comment } from '../../../../models/comment';
import { AuthService } from '../../../../services/auth.service';
import { ApiPaths } from '../../../../app.constants'
import { User } from 'src/app/models/user';


@Component({
  selector: 'app-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.css']
})
export class AddCommentComponent implements OnInit {
  @Output() commentAdded: EventEmitter<Comment> = new EventEmitter<Comment>();
  commentContent: string = '';
  authorEmail: string = null;
  error: string = '';

  constructor(private dataService: DataService, 
              private route: ActivatedRoute, 
              private authService: AuthService) { }

  ngOnInit() { }

  addComment(): void {
    let id = +this.route.snapshot.params['id'];

    let commentData = {
      articleId: id,
      content: this.commentContent,
      authorId: this.authService.isLogged() ? this.authService.getId() : null
    }
    this.dataService.createItem<Comment>(ApiPaths.Comments, commentData).subscribe(comment => {
      this.commentAdded.emit(comment);
      this.commentContent = '';
      this.error = ''
    },
    err => this.error = "Couldn't create comment");
  }
}
