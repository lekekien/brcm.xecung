import { Injectable } from '@angular/core';
import { ScriptLoaderService } from './script-loader.service';
import { ServerResponse } from '../models/response';
import { ResponseCode } from '../enums/responsecode';
declare var showMessage: any;
declare var CloseModal: any;
declare var SwitchTab: any;
declare var CustomerSideBar: any;
declare var SizeMonitor: any;
@Injectable({
  providedIn: 'root'
})
export class DialogService {
  constructor() { }
  showSuccess(msg: string, title = '', func: any = null) {
    if (typeof (func) === 'function') {
      func();
    }
    showMessage.showSuccess(msg, title, func);
  }
  showWarning(msg: string, title = '') {
    showMessage.showWarning(msg, title);
  }
  showError(msg: string, title = '') {
    showMessage.showError(msg, title);
  }
  showConfirm(msg: string, method, title = '') {
    showMessage.showConfirm(msg, method, title);
  }
  closeModal() {
    CloseModal.close();
  }
  switchTab() {
    SwitchTab.init();
  }
  CustomerSideBar() {
    CustomerSideBar.init();
  }
  SizeMonitor() {
    const size = SizeMonitor.init();
    return size;
  }
}
