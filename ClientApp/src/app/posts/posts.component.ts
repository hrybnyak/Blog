import { Component, OnInit, Input, Renderer, ViewChild, ElementRef } from '@angular/core';
import { DataService } from '../services/data.service';
import { Post } from '../models/post';
import { HttpParams } from '@angular/common/http';
import { ActivatedRoute, Params } from '@angular/router';
import { ApiPaths } from '../app.constants'
import { PostService } from '../services/post.service';
import { error } from 'protractor';
import { catchError, filter, debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { Observable, fromEvent } from 'rxjs';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css']
})
export class PostsComponent implements OnInit {
  @ViewChild('input', { static: true }) input: ElementRef;

  posts: Post[] = [];
  search: string = "";
  mode: string = "text";
  pageIndex: number = 1;
  itemsPerPage: number = 10;
  postsCount: number = 0;
  loading: boolean = true;
  error_message: string = '';

  constructor(private dataService: DataService, private route: ActivatedRoute, private postService: PostService, private rendered: Renderer) { }

  ngOnInit() {
    this.dataService.getItems<Post>(ApiPaths.Posts).subscribe((res) => {
      this.posts = res.slice((this.pageIndex - 1) * this.itemsPerPage, this.itemsPerPage * this.pageIndex - 1);
      this.postsCount = this.posts.length;
      this.loading = false;
      this.error_message = '';
    },
      (error) => {
        this.error_message = "Sorry, couldn't find any posts!";
      });
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
    fromEvent(this.input.nativeElement, 'keydown')
      .pipe(
        filter(Boolean),
        debounceTime(300),
        distinctUntilChanged(),
        tap((text) => {
          console.log(this.input.nativeElement.value)
        })
      )
      .subscribe(() => {
        this.search == this.input.nativeElement.value;
        if (this.search === "") {
          this.dataService.getItems<Post>(ApiPaths.Posts).subscribe((res) => {
            this.posts = res.slice((this.pageIndex - 1) * this.itemsPerPage, this.itemsPerPage * this.pageIndex - 1);
            this.postsCount = this.posts.length;
            this.loading = false;
            this.error_message = '';
          },
            (error) => {
              this.error_message = "Sorry, couldn't find any posts!";
            });
        }
        else if (this.mode === "text") {
          console.log(this.search)
          this.loading = true;
          this.postService.getPostsWithTextFilter(this.search).subscribe((res) => {
            if (res.length != 0) {
              this.posts = res.slice((this.pageIndex - 1) * this.itemsPerPage, this.itemsPerPage * this.pageIndex - 1);
              this.postsCount = this.posts.length;
              this.error_message = '';
            }
            else { this.posts = null }
            this.loading = false;
          },
            (error) => {
              this.error_message = "Sorry, couldn't find any posts with this tegs!";
            })
        }
        else if (this.mode === "teg") {
          this.postService.getPostsWithTegFilter(this.search).subscribe((res) => {
            if (res.length != 0) {
              this.posts = res.slice((this.pageIndex - 1) * this.itemsPerPage, this.itemsPerPage * this.pageIndex - 1);
              this.postsCount = this.posts.length;
              this.error_message = '';
            }
            else { this.posts = null }
          },
            (error) => {
              this.error_message = "Sorry, couldn't find any posts with this tegs!";
            })
        }
      });
  }

  onPostDeleted(postId: number): void {
    let index = this.posts.findIndex(p => p.id === postId);
    this.posts.splice(index, 1);
  }
}
