import { Injectable } from '@angular/core';
import { ServerResponse } from 'src/app/models/response';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ResponseCode } from '../enums/responsecode';
import { Router } from '@angular/router';
import { DialogService } from './dialog.service';
import { Helpers } from '../helpers';
import { BlockUiService } from './block-ui.service';
// import { Helpers } from 'src/app/helpers';

@Injectable({
  providedIn: 'root'
})
export class SysrequestService {

  constructor(
    private router: Router,
    private http: HttpClient,
    private blockUI: BlockUiService,
    private dialogService: DialogService
  ) { }

  sendRequest(url: string, data: any, callback: (res: ServerResponse) => void, urlRedirect = '/'): void {
    Helpers.setLoading(true);
    const body = JSON.stringify(data);
    const options = {
      withCredentials: true,
      headers: new HttpHeaders({ 'Content-Type': 'application/json charset=utf-8' })
    };
    const responseObservable = this.http.post<any>(url, body, options).pipe(map((serverResponse: ServerResponse) => serverResponse));
    responseObservable.subscribe((response: ServerResponse) => {
      Helpers.setLoading(false);
      if (response.Code === ResponseCode.NotLogIn) {
        localStorage.removeItem('currentUser');
        this.router.navigate(['/login']);
      } else if (response.Code === ResponseCode.NotPermitted) {
        this.router.navigate([urlRedirect]);
      } else {
        callback(response);
      }
    },
      errors => {
        Helpers.setLoading(false);
        this.dialogService.showError('Error connect API Server . Contact IT for details !');
      });
  }
  sendRequestResolve(url: string, data: any, returnUrl = '/') {
    const body = JSON.stringify(data);
    const options = {
      withCredentials: true,
      headers: new HttpHeaders({ 'Content-Type': 'application/json charset=utf-8' })
    };
    return this.http.post<any>(url, body, options).pipe(map((serverResponse: ServerResponse) => {
      if (serverResponse.Code === ResponseCode.NotLogIn) {
        localStorage.removeItem('currentUser');
        this.router.navigate(['/login']);
      } else if (serverResponse.Code === ResponseCode.NotPermitted) {
        this.router.navigate([returnUrl]);
      } else {
        return serverResponse;
      }
    }));
  }
  sendRequestAndBlockUI(url: string, data: any, uiElement: any= null, callback: (res: ServerResponse) => void): void {
    const body = JSON.stringify(data);
    const options = {
      withCredentials: true,
      headers: new HttpHeaders({'Content-Type': 'application/json charset=utf-8'})
    };
    this.blockUI.block(uiElement);
    const responseObservable = this.http.post<any>(url, body, options).pipe( map((serverResponse: ServerResponse) => serverResponse));
    responseObservable.subscribe(( response: ServerResponse) => {
      if (response.Code === ResponseCode.NotLogIn) {
        localStorage.removeItem('currentUser');
        this.router.navigate(['/login']);
      } else {
        callback(response);
      }
    },
    (error: any) => {this.blockUI.unblock(uiElement); this.dialogService.showError('Error connect API Server . Contact IT for details !'); },
    () => this.blockUI.unblock(uiElement)
   );
  }
  importExcel(url: string, data: FormData, callback: (res: ServerResponse) => void, returnUrl = '/'): void {
    Helpers.setLoading(true);
    const uploadReq = {
      withCredentials: true,
      reportProgress: true
    };
    const responseObservable = this.http.post<any>(url, data, uploadReq).pipe(map((serverResponse: ServerResponse) => serverResponse));
    responseObservable.subscribe((response: ServerResponse) => {
      Helpers.setLoading(false);
      if (response.Code === ResponseCode.NotLogIn) {
        localStorage.removeItem('currentUser');
        this.router.navigate(['/login']);
      } else if (response.Code === ResponseCode.NotPermitted) {
        this.router.navigate([returnUrl]);
      } else {
        return callback(response);
      }
    },
    errors => {
      Helpers.setLoading(false);
      this.dialogService.showError('Error connect API Server . Contact IT for details !');
    });
  }
}
