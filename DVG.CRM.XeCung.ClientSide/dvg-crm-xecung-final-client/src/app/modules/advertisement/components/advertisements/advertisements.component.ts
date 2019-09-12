import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
// import { VideoIndexModel } from 'src/app/models/video-index-model';
// import { VideoService } from 'src/app/services/video.service';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { DialogService } from 'src/app/services/dialog.service';
// import { VideoSearchResult } from 'src/app/models/video-search-result';
// import { VideoType } from 'src/app/enums/videotype';
// import { VideoViewInit } from 'src/app/models/video-view-init';
import { KeyValueObject } from 'src/app/models/key-value';

@Component({
  selector: 'app-advertisements',
  templateUrl: './advertisements.component.html',
  styleUrls: ['./advertisements.component.css']
})
export class AdvertisementsComponent extends BaseComponent implements OnInit {
 
}
