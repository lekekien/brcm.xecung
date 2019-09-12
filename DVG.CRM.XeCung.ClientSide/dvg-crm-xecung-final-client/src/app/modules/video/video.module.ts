import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideoCreateComponent } from './components/video-create/video-create.component';
import { VideosComponent } from './components/videos/videos.component';
import { VideoRoutingModule } from './video-routing.module';
import { FormsModule } from '@angular/forms';
import { DatepickerModule, BsDatepickerModule } from 'ngx-bootstrap';
import { SharedcomponentModule } from '../sharedcomponent/sharedcomponent.module';
import { VideoEditComponent } from './components/video-edit/video-edit.component';
import { VideoDetailComponent } from './components/video-detail/video-detail.component';
@NgModule({
  declarations: [VideoCreateComponent, VideosComponent, VideoEditComponent, VideoDetailComponent],
  imports: [
    FormsModule,
    CommonModule,
    VideoRoutingModule,
    SharedcomponentModule,
    DatepickerModule.forRoot(),
    BsDatepickerModule.forRoot()
  ]
})
export class VideoModule { }
