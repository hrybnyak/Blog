import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { User } from 'src/app/models/user';
import { DataService } from 'src/app/services/data.service';
import { ApiPaths } from 'src/app/app.constants';
import { UserService } from 'src/app/services/user.service';
import { Router, NavigationEnd } from '@angular/router';
import { Blog } from 'src/app/models/blog';

@Component({
  selector: 'app-moderators-list',
  templateUrl: './moderators-list.component.html',
  styleUrls: ['./moderators-list.component.css']
})
export class ModeratorsListComponent implements OnInit {

  moderators: User[];
  loading: boolean = true;
  error:string = ''

  constructor(private dataService: DataService, private router: Router) { }

  ngOnInit() {
    this.dataService.getItems(ApiPaths.AdminModerators).subscribe((data: User[]) => {
      this.moderators = data;
      console.log(this.moderators);
      this.loading = false;
    },
    (error) => {
      this.error = "Couldn't get moderators list";
    });
  }

  navigationSubscription = this.router.events.subscribe((e: any) => {
    if (e instanceof NavigationEnd) {
      this.ngOnInit();
    }
  });

  onModeratorDeleted(id: string): void {
    let index = this.moderators.findIndex(u => u.id === id);
    this.moderators.splice(index, 1);
  }
}
