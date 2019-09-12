import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
import { VideoIndexModel } from 'src/app/models/video-index-model';
import { VideoService } from 'src/app/services/video.service';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { DialogService } from 'src/app/services/dialog.service';
import { VideoSearchResult } from 'src/app/models/video-search-result';
import { VideoType } from 'src/app/enums/videotype';
import { VideoViewInit } from 'src/app/models/video-view-init';
import { KeyValueObject } from 'src/app/models/key-value';

@Component({
  selector: 'app-videos',
  templateUrl: './videos.component.html',
  styleUrls: ['./videos.component.css']
})
export class VideosComponent extends BaseComponent implements OnInit {
  lstVideos: VideoSearchResult[];
  videoIndex: VideoIndexModel;
  videoViewInit: VideoViewInit;
  constructor(
    private videoService: VideoService,
    private dialog: DialogService,
  ) {
    super();
    this.videoViewInit = new VideoViewInit();
    this.videoIndex = new VideoIndexModel();
  }

  ngOnInit() {
    this.InitView();
    this.getAllVideos(1);
  }
  private InitView(): void {
    this.videoService.initCreateView((resposne: ServerResponse) => {
      if (resposne.Code === ResponseCode.Success) {
        this.videoViewInit.AllVideoType = resposne.Data.AllVideoType as KeyValueObject[];
      } else if (resposne.Code === ResponseCode.Error) {
        this.dialog.showError(resposne.Message);
      } else {
        this.dialog.showWarning(resposne.Message);
      }
    });
  }

  public getAllVideos(pageIndex: number) {
    console.log(this.videoIndex);
    this.videoIndex.PageIndex = pageIndex;
    this.videoService.getAllVideo(this.videoIndex, (res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        this.lstVideos = res.Data.Result as VideoSearchResult[];
        this.videoIndex.TotalRecord = res.Data.TotalRecord;
        console.log(this.lstVideos);
      } else {
        this.dialog.showWarning(res.Message);
      }
    });
  }
  public isDeletable(item: VideoSearchResult): boolean {
    console.log(item.InvoiceIssuedDate);
    if(item.InvoiceIssuedDate !== null || item.InvoiceIssuedDate !== '' || item.InvoiceIssuedDate !== undefined) {
      return false;
    }
    //if(item.Id == 5) return false;
    return true;
  }
  public delete(Video: any) {
    // console.log(itemID);
    // return;
    this.videoService.delete(Video, (res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        
        console.log('this.lstVideos');
      } else {
        this.dialog.showWarning(res.Message);
      }
    });
  }
}
