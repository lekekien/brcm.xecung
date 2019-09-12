import { Component, OnInit, ViewChild } from '@angular/core';
import { UserModel } from 'src/app/models/usermodel';
import { UserService } from 'src/app/services/user.service';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { Role } from 'src/app/enums/roles';
import { Router, ActivatedRoute } from '@angular/router';
import { TokenService } from 'src/app/services/token.service';
import { DialogService } from 'src/app/services/dialog.service';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
import { GetdropdownlistService } from 'src/app/services/getdropdownlist.service';
import { ListDropDown, DropDownDic, ListLocation } from 'src/app/models/listdropdown';
import { KeyValueObject } from 'src/app/models/key-value';
import { ScriptLoaderService } from 'src/app/services/script-loader.service';
import { DatepickercustomComponent } from 'src/app/modules/sharedcomponent/components/datepickercustom/datepickercustom.component';
declare var $: any;
declare var jQuery: any;


@Component({
  selector: 'app-user-update',
  templateUrl: './userupdate.component.html',
  styleUrls: ['./userupdate.component.css']
})
export class UserupdateComponent extends BaseComponent implements OnInit {
  @ViewChild(DatepickercustomComponent) birthDayField: DatepickercustomComponent;
  userCreateModel: UserModel;
  dropdown: DropDownDic;
  location: ListLocation;
  isEdit = false;
  listOptionRole = [];
  isCheckRole = false;
  constructor(
    private tokenService: TokenService,
    private userService: UserService,
    private router: Router,
    private routerActive: ActivatedRoute,
    private script: ScriptLoaderService,
    private dialog: DialogService,
    private dropdownlist: GetdropdownlistService
  ) {
    super();
    this.userCreateModel = new UserModel();
    this.userCreateModel.Password = '123456';
    this.dropdown = new DropDownDic();
    this.location = new ListLocation();
  }

  ngOnInit() {

    // Get Id nếu là Edit
    this.routerActive.params.subscribe(params => {
      const userId = +params.id;
      if (userId > 0) {
        this.userService.getUserByIdPermitted(userId, (response: ServerResponse) => {
          switch (response.Code) {
            case ResponseCode.Success:
              this.tokenService.getTokenUpdateView(userId, (tokenResponse: ServerResponse) => {
                if (tokenResponse.Code === ResponseCode.Success) {
                  if (response.Data != null) {
                    this.userCreateModel = response.Data as UserModel;
                    this.isEdit = true;
                    this.checkDate(this.userCreateModel.Birthday);
                    // Get các dropdownlistUser
                    this.getDropdownList();
                  }
                  this.userCreateModel.Token = tokenResponse.Data;
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
      } else {
        // Get các dropdownlistUser
        this.getDropdownList();      
      }
    });
  }
  // tslint:disable-next-line: use-life-cycle-interface
  ngAfterViewInit() {
    $('input[name=birthday]').off('change').on('change', () => {
      this.userCreateModel.Birthday = $('input[name=birthday]').val();
    });
    this.script.loadScripts('app-user-update', ['assets/demo/default/custom/components/forms/widgets/select2.js']);
    this.script.loadScripts('app-user-update', ['./assets/js-custom/select2init.js']);
    $('#selRole').on('change', () => {
      this.changeRole(parseInt($('#selRole').val(), 10));
    });
  }
  public getDropdownList() {
    this.dropdownlist.getdropdownlistPermittedUser((res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        this.dropdown.ListRole = res.Data.ListRole as ListDropDown[];
        this.listOptionRole = [];
        for (const item of this.dropdown.ListRole) {
          const Name = item.Value;
          const Key = item.Key;
          let Checked = false;
          if (this.userCreateModel.ListUserRoleId.indexOf(item.Key) > -1) {
            Checked = true;
            this.isCheckRole = true;
          }
          this.listOptionRole.push({ Name, Key, Checked });
        }
        return this.listOptionRole;
      }
    });
  }
  // submit form
  public update() {
    console.log(this.userCreateModel);
    this.userService.update(this.userCreateModel, (res: ServerResponse) => {
      switch (res.Code) {
        case ResponseCode.Success:
          if (this.userCreateModel.Id > 0) {
            this.dialog.showSuccess(res.Message);
          } else {
            this.dialog.showSuccess(res.Message, '',() => {
              this.router.navigate(['/users']);
            });
          }
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
  public refresh() {
    this.birthDayField.changedDate(null);
    this.userCreateModel = new UserModel();
    this.userCreateModel.Password = '123456';
    
  }
  public checkDate(stringDate: string) {
    const date = new Date(stringDate);
    const minDate = new Date('0001-01-01T00:00:00');
    return this.userCreateModel.Birthday = date > minDate ? stringDate : null;
  }
  public closeForm() {
    this.router.navigate(['/users']);
  }
  public changeRole(roleId: number) {
    this.userCreateModel.RoleId = roleId;
  }
  public changeListRole() {
    this.userCreateModel.ListUserRoleId = this.listOptionRole.filter(item => item.Checked === true).map(item => item.Key);
    const listChecked = this.listOptionRole.filter(item => item.Checked === true);
    this.userCreateModel.RoleName = '-- Chọn quyền --';
    if (listChecked.length > 0) {
      this.userCreateModel.RoleName = '';
      this.isCheckRole = true;
      for (const item of listChecked) {
        this.userCreateModel.RoleName = this.userCreateModel.RoleName + item.Name + ', ';
      }
      this.userCreateModel.RoleName = this.userCreateModel.RoleName.substring(0, this.userCreateModel.RoleName.length - 2);
    } else {
      this.isCheckRole = false;
    }
 
  }
}
