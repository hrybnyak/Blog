import { Component, OnInit } from '@angular/core';
import { DataService } from '../../services/data.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from '../../models/post';
import { Pipe } from '@angular/core';
import { Comment } from '../../models/comment';
import { ApiPaths, ApplicationPaths } from '../../app.constants'
import { AuthService } from '../../services/auth.service';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-post-view',
  templateUrl: './post-view.component.html',
  styleUrls: ['./post-view.component.css']
})
export class PostViewComponent implements OnInit {
  post: Post;
  categories : string;
  format: string = 'dd.MM.yyyy';
  loading: boolean = true;

  constructor(private dataService: DataService, 
              private route: ActivatedRoute, 
              private router: Router,
              private authService: AuthService) { }

  ngOnInit() {
    var id = <number>this.route.snapshot.params['id'];
    this.dataService.getItemById<Post>(ApiPaths.Posts, id).subscribe(post => {
      this.post = post;
      this.categories = this.getCategoriesToString();
      this.loading = false;
    });
  }

  redirectToEditPost(){this.router.navigateByUrl(ApplicationPaths.Blogs+`/${this.post.blogId}/${ApplicationPaths.Posts}/${this.post.id}`)};

  private getCategoriesToString(): string {
    let result: string = "";
    if (this.post.tegs !== undefined){
    for (let i = 0; i < this.post.tegs.length; i++) {
      result += this.post.tegs[i].name
      if (i !== this.post.tegs.length - 1) {
        result += ','
      }
    }
  }
    return result;
  }
  onCommentAdded(comment: Comment): void {
    this.post.comments.unshift(comment);
  }

  deletePost() {
    this.dataService.deleteItem(ApiPaths.Posts, this.post.id).subscribe(res => this.router.navigateByUrl(ApplicationPaths.Posts));
  }
}
