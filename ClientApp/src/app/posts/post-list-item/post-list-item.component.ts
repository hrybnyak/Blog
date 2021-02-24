import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Pipe } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';

import { Post } from '../../models/post';
import { DataService } from '../../services/data.service';
import { AuthService } from '../../services/auth.service';
import { ApiPaths, ApplicationPaths } from '../../app.constants';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-post-list-item',
  templateUrl: './post-list-item.component.html',
  styleUrls: ['./post-list-item.component.css']
})
export class PostListItemComponent implements OnInit {
  @Input() post: Post;
  @Output() postDeleted: EventEmitter<number> = new EventEmitter<number>();


  format: string = 'dd.MM.yyyy';

  constructor(private dataService: DataService,
    private authService: AuthService,
    private router: Router) { }
    error:string = ''

  ngOnInit() {
  }

  redirectToEdit() {
    this.router.navigateByUrl(`${ApplicationPaths.Blogs}/${this.post.blogId}/${ApplicationPaths.Posts}/${this.post.id}`);
  }

  deletePost(event, postId: number) {
    event.stopPropagation();
    this.dataService.deleteItem(ApiPaths.Posts, postId).subscribe(res => {this.postDeleted.emit(postId);
    this.error = ''},
    err => this.error = "Couldn't delete post");
  }
}
