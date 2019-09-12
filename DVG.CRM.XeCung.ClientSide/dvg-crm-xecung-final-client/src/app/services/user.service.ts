import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { environment } from 'src/environments/environment';
import { UserModel, ChangePassWord } from '../models/usermodel';
import { SysrequestService } from 'src/app/services/sysrequest.service';
import { Paging } from '../models/pagingmodel';
import { UrlOtp } from '../models/otpprivatekey';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(
    private http: HttpClient,
    private systemReques: SysrequestService
  ) {

  }
  getAllUser(pager: Paging, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + `api/user/search`;
    this.systemReques.sendRequest(url, pager, (response: ServerResponse) => {
      callback(response);
    });
  }
  update(user: UserModel, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + `api/user/update`;
    this.systemReques.sendRequest(url, user, (respones: ServerResponse) => {
      callback(respones);
    });
  }
  updateProfile(user: UserModel, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + `api/user/update`;
    this.systemReques.sendRequest(url, user, (respones: ServerResponse) => {
      callback(respones);
    });
  }
  getUserById(userId: number, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + `api/user/getbyid/` + userId;
    this.systemReques.sendRequest(url, userId, (response: ServerResponse) => {
      callback(response);
    });
  }
  getUserByIdPermitted(userId: number, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + `api/user/getbyidpermitted/` + userId;
    this.systemReques.sendRequest(url, userId, (response: ServerResponse) => {
      callback(response);
    });
  }
  changePassword(user: ChangePassWord, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + `api/user/changepassword`;
    this.systemReques.sendRequest(url, user, (response: ServerResponse) => {
      callback(response);
    });
  }
  resetPassWord(user: ChangePassWord, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + `api/user/resetpassword`;
    this.systemReques.sendRequest(url, user, (response: ServerResponse) => {
      callback(response);
    });
  }
  blockUser(user: UserModel, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/user/block';
    this.systemReques.sendRequest(url, user, (response: ServerResponse) => {
      callback(response);
    });
  }
  unBlockUser(user: UserModel, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/user/unblock';
    this.systemReques.sendRequest(url, user, (response: ServerResponse) => {
      callback(response);
    });
  }
  getUrlOTP(userId: number, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/user/geturl/' + userId;
    this.systemReques.sendRequest(url, {}, (response: ServerResponse) => {
      callback(response);
    });
  }
  getOTP(urlOtp: UrlOtp, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/user/getotpprivatekey';
    this.systemReques.sendRequest(url, urlOtp, (response: ServerResponse) => {
      callback(response);
    });
  }
}
