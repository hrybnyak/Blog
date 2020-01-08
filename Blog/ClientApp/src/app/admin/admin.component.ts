import { Component, OnInit } from '@angular/core';
import { User } from '../models/user';
import { DataService } from '../services/data.service';
import { ApiPaths } from '../app.constants';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  users: User[];
  error: string ='';
  constructor(private dataService: DataService) { }


  ngOnInit() {
    this.dataService.getItems<User>(ApiPaths.AdminUsers).subscribe((data: User[]) => this.users = data,
    (error) => this.error = "Couldn't load users' data");
  }
}
