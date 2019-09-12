import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { DialogService } from 'src/app/services/dialog.service';

@Component({
  selector: 'app-paging',
  templateUrl: './paging.component.html',
  styleUrls: ['./paging.component.css']
})
export class PagingComponent implements OnInit {
  @Output() searchEvent = new EventEmitter<number>();
  @Input() pageIndex: number;
  @Input() pageSize: number;
  @Input() totalRecord: number;
  constructor(private dialog: DialogService) { }
  get totalPage() {
    if (this.totalRecord % this.pageSize !== 0) {
      return (Math.floor(this.totalRecord / this.pageSize)) + 1;
    } else {
      return this.totalRecord / this.pageSize;
    }
  }
  get previousPage() {
    return this.pageIndex === 1 ? 1 : this.pageIndex - 1;
  }
  get nextPage() {
    return this.pageIndex === this.totalPage ? this.totalPage : this.pageIndex + 1;
  }
  getShowIndexPage() {
    if (this.totalRecord === 0) {
      return [];
    }
    const size = this.dialog.SizeMonitor();
    const arrIndex = [];
    let start = 0;
    let end = 0;
    if (size > 1000) {
      if (this.totalPage <= 10) {
        for (let i = 1; i <= this.totalPage; i++) {
          arrIndex.push(i);
        }
      } else {
        if (this.pageIndex < 6) {
          for (let i = 1; i <= 10; i++) {
            arrIndex.push(i);
          }
        } else {

          if (this.pageIndex + 5 > this.totalPage) {
            start = this.totalPage - 10;
            end = this.totalPage;
          } else {
            start = this.pageIndex - 4;
            end = this.pageIndex + 5;
          }

          for (let i = start; i <= end; i++) {
            arrIndex.push(i);
          }
        }
      }
    } else if (size > 600 && size <= 1000) {
      if (this.totalPage <= 6) {
        for (let i = 1; i <= this.totalPage; i++) {
          arrIndex.push(i);
        }
      } else {
        if (this.pageIndex < 4) {
          for (let i = 1; i <= 7; i++) {
            arrIndex.push(i);
          }
        } else {
          if (this.pageIndex + 3 > this.totalPage) {
            start = this.totalPage - 5;
            end = this.totalPage;
          } else {
            start = this.pageIndex - 2;
            end = this.pageIndex + 3;
          }

          for (let i = start; i <= end; i++) {
            arrIndex.push(i);
          }
        }
      }
    } else {
      if (this.totalPage <= 3) {
        for (let i = 1; i <= this.totalPage; i++) {
          arrIndex.push(i);
        }
      } else {
        if (this.pageIndex < 3) {
          for (let i = 1; i <= 3; i++) {
            arrIndex.push(i);
          }
        } else {
          if (this.pageIndex + 2 > this.totalPage) {
            start = this.totalPage - 2;
            end = this.totalPage;
          } else {
            start = this.pageIndex - 1;
            end = this.pageIndex + 1;
          }

          for (let i = start; i <= end; i++) {
            arrIndex.push(i);
          }
        }
      }
    }
    return arrIndex;
  }
  ngOnInit() {
  }
  callParentSearch(showPage: number) {
    this.pageIndex = showPage;
    this.searchEvent.emit(showPage);
  }
}
