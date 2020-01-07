import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';
import { DataService } from 'src/app/services/data.service';
import { ApiPaths } from 'src/app/app.constants';
import { Blog } from 'src/app/models/blog';
import { Subscriber, Subscription } from 'rxjs';

@Component({
  selector: 'app-profile-picture',
  templateUrl: './profile-picture.component.html',
  styleUrls: ['./profile-picture.component.css']
})
export class ProfilePictureComponent implements OnInit {

  constructor(private authService: AuthService, private userService: UserService, private dataService: DataService) { }

  loggedUser: User;
  commentsCount: number;
  blogsCount: number;
  loading: boolean = true;

  getUser() {
    this.authService.getLoggedUser().subscribe((data: User) => {
      this.loggedUser = data;
    })
  }
  getUserComments() {
    this.userService.getUserComments(this.authService.getId()).subscribe((data: Comment[]) => this.commentsCount = data.length);
    console.log(this.commentsCount);
  }
  getUserBlogs(): Subscription {
    return this.userService.getUserBlogs(this.authService.getId()).subscribe((data: Blog[]) => {
      this.blogsCount = data.length;
      this.loading = false;
    })
  }
  ngOnInit() {
    this.getUser();
    this.getUserComments();
    this.getUserBlogs();
  }


}
