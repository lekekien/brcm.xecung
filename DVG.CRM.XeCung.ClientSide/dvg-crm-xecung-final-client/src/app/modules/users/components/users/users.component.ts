import { Component, OnInit, ViewChild } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
import { UserSearchResult } from 'src/app/models/usersearchresult';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { TokenService } from 'src/app/services/token.service';
import { UserModel, ChangePassWord } from 'src/app/models/usermodel';
import { DialogService } from 'src/app/services/dialog.service';
import { Paging } from 'src/app/models/pagingmodel';
import { OtpPrivateKey } from 'src/app/models/otpprivatekey';
import { NgForm } from '@angular/forms';
import { UserIndexModel } from 'src/app/models/userindexmodel';
declare var jquery: any;
declare var $: any;
@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent extends BaseComponent implements OnInit {
  lstUser: UserSearchResult[];
  showChildComponent: string;
  userCreateModel: UserModel;
  changePassWord: ChangePassWord;
  currentUrl = window.location.pathname;
  index = 0;
  @ViewChild('formResetPass') form: NgForm;
  userIndex: UserIndexModel;
  constructor(
    private userService: UserService,
    private tokenService: TokenService,
    private router: Router,
    private dialog: DialogService,
  ) {
    super();
    this.showChildComponent = 'userslist';
    this.userCreateModel = new UserModel();
    this.changePassWord = new ChangePassWord();
    this.userIndex = new UserIndexModel();
  }

  ngOnInit() {
    this.getAllUser(this.userIndex.PageIndex);
  }
  public getAllUser(pageIndex: number) {
    this.userIndex.PageIndex = pageIndex;
    this.userService.getAllUser(this.userIndex, (res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        this.lstUser = res.Data.ListUser as UserSearchResult[];
        this.userIndex.TotalRecord = res.Data.TotalRecord;
      } else {
        this.dialog.showWarning(res.Message);
      }
    });
  }
  public confirmBlock(id: number) {
    this.tokenService.getTokenPopup(id, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        this.userCreateModel.Token = tokenResponse.Data;
        this.userCreateModel.Id = id;
        this.dialog.showConfirm('Bạn có chắc muốn khóa user này?', () => { this.blockUser(this.userCreateModel); });
      }
    });

  }
  public confirmUnBlock(id: number) {
    this.tokenService.getTokenPopup(id, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        this.userCreateModel.Token = tokenResponse.Data;
        this.userCreateModel.Id = id;
        this.dialog.showConfirm('Bạn có chắc muốn mở khóa user này?', () => { this.unBlockUser(this.userCreateModel); });
      }
    });
  }
  private blockUser(user: UserModel) {
    this.userService.blockUser(user, (res: ServerResponse) => {
      switch (res.Code) {
        case ResponseCode.Success:
          this.dialog.showSuccess(res.Message);
          this.getAllUser(this.userIndex.PageIndex);
          break;
        case ResponseCode.Warning:
          this.dialog.showError(res.Message);
          break;
        case ResponseCode.Error:
          this.dialog.showError(res.Message);
          break;
        default: break;
      }
    });
  }
  private unBlockUser(user: UserModel) {
    this.userService.unBlockUser(user, (res: ServerResponse) => {
      switch (res.Code) {
        case ResponseCode.Success:
          this.dialog.showSuccess(res.Message);
          this.getAllUser(this.userIndex.PageIndex);
          break;
        case ResponseCode.Warning:
          this.dialog.showError(res.Message);
          break;
        case ResponseCode.Error:
          this.dialog.showError(res.Message);
          break;
        default: break;
      }
    });
  }
  public getResetId(id: number) {
    return this.changePassWord.Id = id;
  }

  //get token
  public getToken(userId) {
    this.tokenService.getTokenUpdateView(userId, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        this.changePassWord.Token = tokenResponse.Data;
      }
    });
  }
  public resetPassword() {
    this.userService.resetPassWord(this.changePassWord, (res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        $('#closeChangepassword').trigger('click');
        this.dialog.showSuccess(res.Message);
      } else {
        this.dialog.showWarning(res.Message);
      }
    });
  }
  public getUrlOTPPrivateKey(userId: number) {
    this.userService.getUrlOTP(userId, (res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        const url = 'users/otpprivatekey/' + res.Data.UrlEndCode;
        window.open(url);
      } else {
        this.dialog.showWarning(res.Message);
      }
    });
  }
  public resetForm() {
    this.form.onReset();
    this.changePassWord = new ChangePassWord();
  }

}
