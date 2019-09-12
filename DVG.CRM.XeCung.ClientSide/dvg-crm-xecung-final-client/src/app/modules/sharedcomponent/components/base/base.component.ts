import { Component, OnInit } from '@angular/core';
import { AuthenticatedUser } from 'src/app/models/authenticateduser';
import { Role } from 'src/app/enums/roles';

@Component({
  selector: 'app-base',
  template: ''
})
export class BaseComponent implements OnInit {
  isAdmin: boolean;
  isManager: boolean;
  currUser: AuthenticatedUser;

  constructor() {
    this.currUser = JSON.parse(localStorage.getItem('currentUser')) as AuthenticatedUser;
    this.isAdmin = this.hasRole(Role.Admin);
    this.isManager = this.hasRole(Role.Manager);
  }

  ngOnInit() {
    
  }
  hasRole(checkedRole: Role): boolean {
    //const user = JSON.parse(localStorage.getItem('currentUser')) as AuthenticatedUser;
    if (this.currUser !== undefined && this.currUser != null) {
      if (this.currUser.Roles.includes(checkedRole)) {
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }
  }
  checkIsManager() {
    const bool = this.hasRole(Role.Admin) || this.hasRole(Role.Manager);
    return bool;
  }
  checkCustomerIsManager() {
    const bool = this.hasRole(Role.CustomerManager);
    return bool;
  }
  // checkIsSale() {
  //   const user = JSON.parse(localStorage.getItem('currentUser')) as AuthenticatedUser;
  //   let bool = false;
  //   if (this.hasRole(Role.Saler) && user.Roles.length === 1) {
  //     bool = true;
  //   }
  //   return bool;
  // }
  // checkIsCustomerCare() {
  //   const user = JSON.parse(localStorage.getItem('currentUser')) as AuthenticatedUser;
  //   let bool = false;
  //   if (this.hasRole(Role.CustomerCare) && user.Roles.length === 1) {
  //     bool = true;
  //   }
  //   return bool;
  // }

  s2ab(s) {
    const buf = new ArrayBuffer(s.length);
    const view = new Uint8Array(buf);
    for (let i = 0; i !== s.length; ++i) {
      // tslint:disable-next-line:no-bitwise
      view[i] = s.charCodeAt(i) & 0xFF;
    }
    return buf;
  }

  downloadFile(data: any, fileName = 'export file.xlsx') {
    const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;' });
    const dataURL = window.URL.createObjectURL(blob);

    //window.open(dataURL, '_blank');

    // IE doesn't allow using a blob object directly as link href
    // instead it is necessary to use msSaveOrOpenBlob
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob);
      return;
    }

    const link = document.createElement('a');
    link.href = dataURL;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();

    setTimeout(() => {
      document.body.removeChild(link);
      // For Firefox it is necessary to delay revoking the ObjectURL
      window.URL.revokeObjectURL(dataURL);
    }, 100);
  }

  subStringNote(note: string) {
    if (note == null || note === '') {
      return '';
    }

    const maxlength = 20;
    if (note.length > maxlength) {
      return '<label title="' + note + '">' + note.substring(0, maxlength) + '...' + '</label>';
    } else {
      return '<label title="' + note + '">' + note + '</label>';
    }
  }
  encodeToJson(value: any) {
    return encodeURIComponent(JSON.stringify(value));
  }

}
