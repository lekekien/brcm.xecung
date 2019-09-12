import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from '../enums/responsecode';
import { AuthenticatedUser } from '../models/authenticateduser';
import { environment } from 'src/environments/environment';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<AuthenticatedUser>;
  public currentUser: Observable<AuthenticatedUser>;
  constructor(private http: HttpClient, private cookie: CookieService) {
    this.currentUserSubject = new BehaviorSubject<AuthenticatedUser>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }
  public get currentUserValue(): AuthenticatedUser {
    return this.currentUserSubject.value;
  }
  login(username: string, password: string, otpprivatekey: string) {
    return this.http.post<any>(environment.apiEndPoint + `api/account/login`, { username, password, otpprivatekey }, {
      withCredentials: true
    })
      .pipe(map((serverResponse: ServerResponse) => {
        // login successful if there's a jwt token in the response
        if (serverResponse.Code === ResponseCode.Success && serverResponse.Data) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          const currentUserResponse = serverResponse.Data as AuthenticatedUser;
          //this.cookie.set('UserToken', currentUserResponse.Token, null, '/');
          localStorage.setItem('currentUser', JSON.stringify(currentUserResponse));
          this.currentUserSubject.next(serverResponse.Data as AuthenticatedUser);
        }
        return serverResponse;
      }));
  }
  logout() {
    // remove user from local storage to log user out
    this.currentUserSubject.next(null);
    return this.http.post<any>(environment.apiEndPoint + `api/logOffAccount/logoff`, {}, { withCredentials: true })
      .pipe(map((respones: ServerResponse) => {
        return respones;
      }));
  }
}
