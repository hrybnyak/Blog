import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {
  @Input() pageIndex: number;
  @Input() itemsCount: number;
  @Input() itemsPerPage: number;

  constructor(private router: Router) { }

  ngOnInit() {
    let params = new HttpParams()
      .set('pageIndex', this.pageIndex.toString())
      .set('itemsPerPage', this.itemsPerPage.toString());
  }

  pageChange() {
    this.router.navigate(['/posts'], {queryParams: {pageIndex: this.pageIndex, itemsPerPage: this.itemsPerPage}});
  }
}
