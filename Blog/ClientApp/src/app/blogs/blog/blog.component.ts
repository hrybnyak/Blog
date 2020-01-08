import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { DataService } from 'src/app/services/data.service';
import { User } from 'src/app/models/user';
import { Blog } from 'src/app/models/blog';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { ApiPaths, ApplicationPaths } from 'src/app/app.constants';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.css']
})
export class BlogComponent implements OnInit {
  blog: Blog = <Blog>{};
  mode: string = 'create';
  id: any;
  error: string = '';

  constructor(private dataService: DataService,
    private route: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.checkMode(params);
    });
  }

  editBlog(): void {
    this.dataService.editItem<Blog>(ApiPaths.Blogs, this.id, this.blog).subscribe(res => {this.error = ''; this.redirectToBlogsPage()},
    err => this.error = "Couldn't update blog");
  }

  createBlog(): void {
    var data = {
      name: this.blog.name
    };

    this.dataService.createItem<Blog>(ApiPaths.Blogs, data).subscribe(post => {
      this.id = post.id
      this.error = '';
      this.redirectToBlogsPage();
    },
    err => this.error = "Couldn't create blog");
  }

  private redirectToBlogsPage(): void {
    this.router.navigateByUrl(ApplicationPaths.Blogs);
  }

  private checkMode(params: Params) {
    this.id = params['id'];

    if (this.id !== 'create') {
      this.mode = 'edit';
      this.id = +this.id;
      this.dataService.getItemById<Blog>(ApiPaths.Blogs, this.id).subscribe((data: Blog) => this.blog = data);
    } else {
      this.mode = 'create';
    }
  }

}