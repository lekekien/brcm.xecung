import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../base/base.component';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Router, NavigationEnd } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { DialogService } from 'src/app/services/dialog.service';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { DataSharingService } from 'src/app/services/data-sharing.service';
import { AuthenticatedUser } from 'src/app/models/authenticateduser';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent extends BaseComponent implements OnInit {
  userHeader: AuthenticatedUser;
  constructor(
    private authenService: AuthenticationService,
    private router: Router,
    private cookie: CookieService,
    private dialog: DialogService,
    private dataSharingService: DataSharingService
  ) {
    super();
    this.userHeader = new AuthenticatedUser();
    this.dataSharingService.isUserLoggedIn.subscribe(value => {
      if (value) {
        this.userHeader = JSON.parse(localStorage.getItem('currentUser')) as AuthenticatedUser;
      } else {
        this.userHeader = this.currUser;
      }
    });
  }

  ngOnInit() {
  }
  confirmLogout() {
    this.dialog.showConfirm('Are you sure that you want to Logout?', () => { this.logout(); });
  }
  logout() {
    const res = this.authenService.logout();
    res.subscribe((response: ServerResponse) => {
      if (response.Code === ResponseCode.Success) {
        localStorage.removeItem('currentUser');
        localStorage.removeItem('Location');
        this.cookie.delete('UserToken');
        // this.router.navigate(['/login']);
        window.location.href = '/login';
      }
    });
  }
  CustomerSideBar() {
    this.dialog.CustomerSideBar();
  }
}
