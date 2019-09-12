import { Injectable } from '@angular/core';
import { SysrequestService } from './sysrequest.service';
import { ServerResponse } from '../models/response';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ResponseCode } from '../enums/responsecode';
import { ListDropDown, ListLocation } from '../models/listdropdown';

@Injectable({
  providedIn: 'root'
})
export class GetdropdownlistService {

  constructor(
    private http: HttpClient,
    private systemRequest: SysrequestService
  ) { }
  getdropdownLocation(callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/location/getdropdownlist';
    this.systemRequest.sendRequest(url, {}, (response: ServerResponse) => {
      callback(response);
    });
  }
  // getdropdownLocation() {
  //   const url = environment.apiEndPoint + 'api/location/getdropdownlist';
  //   const lstLocation = this.http.post<any>(url, {}, { withCredentials: true })
  //     .pipe(map((res: ServerResponse) => {
  //       return res;
  //     }));
  //   lstLocation.subscribe((response: ServerResponse) => {
  //     if (response.Code === ResponseCode.Success) {
  //       const location = response.Data as ListLocation;
  //       localStorage.setItem('Location', JSON.stringify(location));
  //     }
  //   });
  // }
  getdropdownUser(callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/user/getdropdownlist';
    this.systemRequest.sendRequest(url, {}, (response: ServerResponse) => {
      callback(response);
    });
  }
  getdropdownlistPermittedUser(callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/user/getdropdownlistpermitted';
    this.systemRequest.sendRequest(url, {}, (response: ServerResponse) => {
      callback(response);
    });
  }
  getdropdownlistContact(callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/contact/getdropdownlist';
    this.systemRequest.sendRequest(url, {}, (response: ServerResponse) => {
      callback(response);
    });
  }
}