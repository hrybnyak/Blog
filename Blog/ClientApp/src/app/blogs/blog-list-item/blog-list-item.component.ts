import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Blog } from 'src/app/models/blog';
import { DataService } from 'src/app/services/data.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { ApiPaths, ApplicationPaths } from 'src/app/app.constants';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-blog-list-item',
  templateUrl: './blog-list-item.component.html',
  styleUrls: ['./blog-list-item.component.css']
})
export class BlogListItemComponent implements OnInit {

  @Input() blog: Blog;
  @Output() blogDeleted: EventEmitter<number> = new EventEmitter<number>();
  error: string = ''
  constructor(private dataService: DataService,
    private router: Router) { }

  ngOnInit() {
  }

  deleteBlog(event, blogId: number) {
    event.stopPropagation();
    this.dataService.deleteItem(ApiPaths.Blogs, blogId).subscribe(res => {this.blogDeleted.emit(blogId);
    this.error = ''},
    err => this.error = "Couldn't delete this post");
  }
}
