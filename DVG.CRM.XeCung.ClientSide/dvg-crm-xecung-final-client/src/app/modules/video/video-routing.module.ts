import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { VideosComponent } from './components/videos/videos.component';
import { VideoCreateComponent } from './components/video-create/video-create.component';
import { VideoEditComponent } from './components/video-edit/video-edit.component';
import { VideoDetailComponent } from './components/video-detail/video-detail.component';

const routes: Routes = [
  {
   path: '',
   component: VideosComponent,
   canActivate: [AuthGuard],
   //data: { roles: [Role.Admin] }
  },
  {
     path: 'create',
     component: VideoCreateComponent,
     canActivate: [AuthGuard],
    //  data: { roles: [Role.Admin] }
   },
   {
    path: 'edit/:id',
    component: VideoEditComponent,
    canActivate: [AuthGuard],
   //  data: { roles: [Role.Admin] }
  },
  {
   path: 'detail/:id',
   component: VideoDetailComponent,
   canActivate: [AuthGuard],
  //  data: { roles: [Role.Admin] }
 },
 ];
 
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VideoRoutingModule { }
