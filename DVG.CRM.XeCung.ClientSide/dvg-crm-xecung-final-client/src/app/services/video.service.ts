import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SysrequestService } from './sysrequest.service';
import { environment } from 'src/environments/environment';
import { ServerResponse } from '../models/response';
import { VideoCreate } from '../models/video-create';
import { Paging } from '../models/pagingmodel';
import { VideoIdRequest } from '../models/video-id-request';
import { VideoEdit } from '../models/video-edit';

@Injectable({
  providedIn: 'root'
})
export class VideoService {

  constructor(private http: HttpClient, private sysrequest: SysrequestService ) { }
  initCreateView(callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/video/initcreateview`, {}, (response: ServerResponse) => {
      callback(response);
    });
  }
  initEditView(model: VideoIdRequest, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/video/initeditview`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  getAllVideo(pager: Paging, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + `api/video/search`;
    this.sysrequest.sendRequest(url, pager, (response: ServerResponse) => {
      callback(response);
    });
  }
  create(model: VideoCreate, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/video/create`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  delete(model: any, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/video/delete`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  edit(model: VideoEdit, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/video/edit`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
}
