import { Component, OnInit, Input, Renderer } from '@angular/core';
import { DataService } from '../services/data.service';
import { Post } from '../models/post';
import { HttpParams } from '@angular/common/http';
import { ActivatedRoute, Params } from '@angular/router';
import { ApiPaths } from '../app.constants'
import { PostService } from '../services/post.service';
import { error } from 'protractor';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css']
})
export class PostsComponent implements OnInit {
  posts: Post[] = [];
  search: string = "";
  mode: string = "text";
  pageIndex: number = 1;
  itemsPerPage: number = 10;
  postsCount: number = 0;
  loading: boolean = true;

  constructor(private dataService: DataService, private route: ActivatedRoute, private postService: PostService, private rendered: Renderer) { }

  ngOnInit() {
    
    this.getPosts();
    this.route.queryParams.subscribe((params: Params) => {
      if (!isNaN(params['pageIndex'])) {
        this.pageIndex = params['pageIndex'];
      }
      if (!isNaN(params['itemsPerPage'])) {
        this.itemsPerPage = params['itemsPerPage'];
      }
      this.getPosts();
    });
  }

  getPosts(): void {
    if (this.search === "")
    {
      this.dataService.getItems<Post>(ApiPaths.Posts).subscribe(res => {
      this.posts = res.slice((this.pageIndex-1)*this.itemsPerPage, this.itemsPerPage*this.pageIndex-1);
      this.postsCount = this.posts.length;
      this.loading = false;
    });}
    else if(this.mode === "text"){
      this.loading = true;
      this.postService.getPostsWithTextFilter(this.search).subscribe(res => {
        if (res.length != 0){
        this.posts = res.slice((this.pageIndex-1)*this.itemsPerPage, this.itemsPerPage*this.pageIndex-1);
        this.postsCount = this.posts.length;
        }
        else {this.posts = null}
        this.loading = false;
      })
    }
    else if(this.mode === "teg"){
      this.postService.getPostsWithTegFilter(this.search).subscribe(res => {
        if (res.length != 0){
        this.posts = res.slice((this.pageIndex-1)*this.itemsPerPage, this.itemsPerPage*this.pageIndex-1);
        this.postsCount = this.posts.length;
        }
        else {this.posts = null}
      })
    }
  }

  onPostDeleted(postId: number): void {
    let index = this.posts.findIndex(p => p.id === postId);
    this.posts.splice(index, 1);
  }
}
