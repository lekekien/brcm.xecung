import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { AuthenticatedUser } from '../models/authenticateduser';
import { environment } from 'src/environments/environment.prod';
import { Observable } from 'rxjs';
import { ServerResponse } from 'src/app/models/response';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { SysrequestService } from './sysrequest.service';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  constructor(
    private http: HttpClient,
    private systemReques: SysrequestService
  ) {
  }

  // Time max: 15 min
  getTokenUpdateView(id: number, callback: (response: ServerResponse) => void) {
    if (id > 0) {
      const url = environment.apiEndPoint + `api/ConfirmRequestToken/gettokenupdateview/` + id;
      this.systemReques.sendRequest(url, {}, (res: ServerResponse) => {
        callback(res);
      });
    }
  }

  // Time max: 1 min
  getTokenPopup(id: number, callback: (response: ServerResponse) => void) {
    if (id > 0) {
      const url = environment.apiEndPoint + `api/ConfirmRequestToken/gettoken/` + id;
      this.systemReques.sendRequest(url, {}, (res: ServerResponse) => {
        callback(res);
      });
    }
  }
}
