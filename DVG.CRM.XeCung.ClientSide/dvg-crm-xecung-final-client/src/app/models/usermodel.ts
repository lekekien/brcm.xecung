import { DatePipe } from '@angular/common';
import { pipe } from '@angular/core/src/render3';

export class UserModel {
    constructor() {
        this.ListUserRoleId = [];
        this.RoleName = '-- Pick Roles --';
    }
    Id: number;
    UserName: string;
    Password: string;
    FullName: string;
    Email: string;
    CreatedDate: string;
    CreatedBy: string;
    PhoneNumber: string;
    Birthday: string;
    BirthdayConvert: string;
    Address: string;
    Note: string;
    RoleId: number;
    ListUserRoleId: Array<number>;
    RoleName: string;
    LastModifiedBy: string;
    LastModifiedDate: string;
    Token: string;
}
export class ChangePassWord extends UserModel {
    CurrentPassword: string;
    NewPassword: string;
    ConfirmNewPassword: string;
}
