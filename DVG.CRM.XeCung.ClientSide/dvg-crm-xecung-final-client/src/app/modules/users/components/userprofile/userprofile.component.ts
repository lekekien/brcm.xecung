import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserModel, ChangePassWord } from 'src/app/models/usermodel';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { DialogService } from 'src/app/services/dialog.service';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
import { ListDropDown, DropDownDic, ListLocation } from 'src/app/models/listdropdown';
import { GetdropdownlistService } from 'src/app/services/getdropdownlist.service';
import { Role } from 'src/app/enums/roles';
import { RoleenumdisplayService } from 'src/app/shared/roleenumdisplay.service';
import { NgForm } from '@angular/forms';
import { KeyValueObject } from 'src/app/models/key-value';
import { ScriptLoaderService } from 'src/app/services/script-loader.service';
declare var $: any;
declare var jQuery: any;

@Component({
  selector: 'app-userprofile',
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css']
})
export class UserprofileComponent extends BaseComponent implements OnInit {
  userCreateModel: UserModel;
  changePass: ChangePassWord;
  id = 0;
  dropdown: DropDownDic;
  location: ListLocation;
  RoleName = '';
  @ViewChild('formchangpass') formChangePass: NgForm;
  constructor(
    private tokenService: TokenService,
    private userService: UserService,
    private router: Router,
    private routerActive: ActivatedRoute,
    private dialog: DialogService,
    private dropdownlist: GetdropdownlistService,
    private script: ScriptLoaderService,
    private getNameEnum: RoleenumdisplayService,
  ) {
    super();
    this.userCreateModel = new UserModel();
    this.changePass = new ChangePassWord();
    this.dropdown = new DropDownDic();
    this.location = new ListLocation();
  }

  ngOnInit() {

    this.routerActive.params.subscribe(params => {
      const userId = +params.id;
      if (userId > 0) {
        this.id = userId;
        this.userService.getUserById(userId, (response: ServerResponse) => {
          switch (response.Code) {
            case ResponseCode.Success:
              this.tokenService.getTokenUpdateView(userId, (tokenResponse: ServerResponse) => {
                if (tokenResponse.Code === ResponseCode.Success) {
                  if (response.Data != null) {
                    this.userCreateModel = response.Data as UserModel;
                    this.checkDate(this.userCreateModel.Birthday);                   
                  }
                  this.userCreateModel.Token = tokenResponse.Data;
                  this.RoleName = this.getNameEnum.getRoleEnumDisplay(this.userCreateModel.RoleId);
                  this.userCreateModel.BirthdayConvert = this.userCreateModel.Birthday;
                }
              });
              break;
            case ResponseCode.Error:
              this.dialog.showError('ERROR: ' + response.Message);
              break;
            case ResponseCode.Warning:
              this.dialog.showWarning('WARNING: ' + response.Message);
              break;
            default: break;
          }
        });
      }
    });
  }
  // tslint:disable-next-line: use-life-cycle-interface
  ngAfterViewInit() {
    this.script.loadScripts('app-userprofile', ['assets/demo/default/custom/components/forms/widgets/select2.js']);
    this.script.loadScripts('app-userprofile', ['./assets/js-custom/select2init.js']);
    $('input[name=birthday]').off('change').on('change', () => {
      this.userCreateModel.Birthday = $('input[name=birthday]').val();
    });
  }
  // get token
  public getToken(userId) {
    this.tokenService.getTokenUpdateView(userId, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        this.userCreateModel.Token = tokenResponse.Data;
      }
    });
  }
  // submit form
  public update() {
    this.userService.updateProfile(this.userCreateModel, (res: ServerResponse) => {
      switch (res.Code) {
        case ResponseCode.Success:
          this.dialog.showSuccess(res.Message);
          break;
        case ResponseCode.Error:
          this.dialog.showError('ERROR: ' + res.Message);
          break;
        case ResponseCode.Warning:
          this.dialog.showWarning('WARNING: ' + res.Message);
          break;
        default: break;
      }
    });
  }
  // change pass
  public changePassWord() {
    this.changePass.Token = this.userCreateModel.Token;
    this.changePass.Id = this.userCreateModel.Id;
      this.userService.changePassword(this.changePass, (response: ServerResponse) => {
        if (response.Code === ResponseCode.Success) {
          this.dialog.showSuccess(response.Message);
          this.refresh();
        } else {
          this.dialog.showError(response.Message);
        }
      });
  }

  public refresh() {
    this.formChangePass.onReset();
    this.changePass = new ChangePassWord();
  }
  public checkDate(stringDate: string) {
    const date = new Date(stringDate);
    const minDate = new Date('0001-01-01T00:00:00');
    return this.userCreateModel.Birthday = date > minDate ? stringDate : null;
  }
}
