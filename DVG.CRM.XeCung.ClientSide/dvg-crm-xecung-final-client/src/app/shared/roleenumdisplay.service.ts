import { Injectable } from '@angular/core';
import { Role } from '../enums/roles';

@Injectable({
  providedIn: 'root'
})
export class RoleenumdisplayService {
  RoleTypesDisplay: { [index: number]: string } = {};
  constructor() {
    this.RoleTypesDisplay[Role.Admin] = 'Admin';
    this.RoleTypesDisplay[Role.Manager] = 'Manager';
    this.RoleTypesDisplay[Role.RevenueManager] = 'Revenue Manager';
    this.RoleTypesDisplay[Role.ContractManager] = 'Contract Manager';
    this.RoleTypesDisplay[Role.CustomerManager] = 'Customer Manager';
  }
  getRoleEnumDisplay(role: number) {
    const enumName = this.RoleTypesDisplay[role];
    if (enumName !== '' && enumName != null && enumName !== undefined) {
      return enumName;
    }
    return '';
  }
}
