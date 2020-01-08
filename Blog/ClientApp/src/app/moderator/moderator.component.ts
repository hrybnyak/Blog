import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { User } from '../models/user';
import { ApiPaths } from '../app.constants';

@Component({
  selector: 'app-moderator',
  templateUrl: './moderator.component.html',
  styleUrls: ['./moderator.component.css']
})
export class ModeratorComponent implements OnInit {
  users: User[];
  constructor(private dataService: DataService) { }
  error: string = '';

  ngOnInit() {
    this.dataService.getItems<User>(ApiPaths.ModeratorUsers).subscribe((data: User[]) => this.users = data,
    err => this.error = "Couldn't get list of users");
  }

}
