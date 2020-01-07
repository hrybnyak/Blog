import { Component, OnInit, NgModule } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { Blog } from 'src/app/models/blog';
import { ApiPaths, ApplicationPaths } from '../../app.constants'
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-blog-view',
  templateUrl: './blog-view.component.html',
  styleUrls: ['./blog-view.component.css']
})

@NgModule({
  imports: [CommonModule]
})

export class BlogViewComponent implements OnInit {
  blog: Blog;
  loading: boolean = true;
  constructor(private dataService: DataService,
    private route: ActivatedRoute,
    private router: Router) { }


  ngOnInit() {
    var id = <number>this.route.snapshot.params['id'];
    this.dataService.getItemById<Blog>(ApiPaths.Blogs, id).subscribe((data: Blog) => {
      this.blog = data;
      if (this.blog.articles !== null) {
        this.blog.articles.forEach(post => {
          post.authorId = data.ownerId
        })
      }
      this.loading = false;
    });
  }

  onPostDeleted(postId: number): void {
    let index = this.blog.articles.findIndex(p => p.id === postId);
    this.blog.articles.splice(index, 1);
  }

  redirectToEdit() {
    this.router.navigateByUrl(`${ApplicationPaths.Blogs}/${this.blog.id}`);
  }

  redirectToPostCreate() {
    this.router.navigateByUrl(`${ApplicationPaths.Blogs}/${this.blog.id}/${ApplicationPaths.Posts}/create`);
  }

  deleteBlog(blogId: number) {
    event.stopPropagation();
    this.dataService.deleteItem(ApiPaths.Blogs, blogId).subscribe();
    this.router.navigateByUrl(ApplicationPaths.Blogs);
  }
}
