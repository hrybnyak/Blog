import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { User } from 'src/app/models/user';
import { DataService } from 'src/app/services/data.service';
import { ApiPaths } from 'src/app/app.constants';

@Component({
  selector: 'app-moderators-list-item',
  templateUrl: './moderators-list-item.component.html',
  styleUrls: ['./moderators-list-item.component.css']
})
export class ModeratorsListItemComponent implements OnInit {

  @Input() user: User;
  @Output() moderatorDeleted: EventEmitter<number> = new EventEmitter<number>();
  constructor(private dataService: DataService) { }

  ngOnInit() {
    console.log(this.user);
  }

  deleteModerator(event, moderatorId: number) {
    event.stopPropagation();
    this.dataService.deleteItem(ApiPaths.AdminModerators, moderatorId).subscribe(res => this.moderatorDeleted.emit(moderatorId));
  }
}
