import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SysrequestService } from './sysrequest.service';
import { ServerResponse } from '../models/response';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  constructor(private http: HttpClient, private sysrequest: SysrequestService) { }
  index(callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/home/index`, {}, (response: ServerResponse) => {
      callback(response);
    });
  }
}
