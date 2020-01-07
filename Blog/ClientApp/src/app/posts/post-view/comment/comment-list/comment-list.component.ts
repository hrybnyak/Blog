import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Comment } from '../../../../models/comment';
import { DataService } from '../../../../services/data.service';
import { ApiPaths } from '../../../../app.constants'
import { AuthService } from '../../../../services/auth.service';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.css']
})
export class CommentListComponent implements OnInit {
  @Input() comments: Comment[] = [];


  constructor(private dataService: DataService, private authService: AuthService) { }

  ngOnInit() {
  }

  deleteComment(comment: Comment, index: number) {
    this.dataService.deleteItem(ApiPaths.Comments, comment.id).subscribe(res => {
      this.comments.splice(index, 1);
    });
  }
}
