import { Component, OnInit, NgModule } from '@angular/core';
import { DataService } from '../services/data.service';
import { AuthService } from '../services/auth.service';
import { ApiPaths } from '../app.constants'
import { User } from '../models/user';
import { Blog } from '../models/blog';
import { UserService } from '../services/user.service';
import { Router, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-blog',
  templateUrl: './blogs.component.html',
  styleUrls: ['./blogs.component.css']
})

export class BlogsComponent implements OnInit {
  loading: boolean = true;
  userBlogs: Blog[];
  loggedUser: User;

  constructor(private authService: AuthService,
    private userService: UserService, private router: Router) { }

  ngOnInit() {
    this.userService.getUserBlogs(this.authService.getId()).subscribe((data: Blog[]) => {
      this.userBlogs = data;
      this.authService.getLoggedUser().subscribe(data => {this.loggedUser = data})
      this.loading = false;
    });
  }

  navigationSubscription = this.router.events.subscribe((e: any) => {
    if (e instanceof NavigationEnd) {
      this.ngOnInit();
    }
  });

  onBlogDeleted(postId: number): void {
    let index = this.userBlogs.findIndex(b => b.id === postId);
    console.log(index);
    this.userBlogs.splice(index, 1);
  }
}
