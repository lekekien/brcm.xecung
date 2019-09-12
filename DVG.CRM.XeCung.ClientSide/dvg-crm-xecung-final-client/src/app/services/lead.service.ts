import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SysrequestService } from 'src/app/services/sysrequest.service';
import { ServerResponse } from '../models/response';
import { environment } from 'src/environments/environment';
import { LeadIndex } from '../models/lead-index';
import { LeadReceive } from '../models/lead-receive';
import { LeadAssign } from '../models/lead-assign';
import { LeadConverted } from './../models/lead-converted';
import { LeadReject } from '../models/lead-reject';
import { LeadBlock } from '../models/lead-block';
import { LeadNote } from '../models/lead-note';

@Injectable({
  providedIn: 'root'
})
export class LeadService {
  constructor(private http: HttpClient, private sysrequest: SysrequestService) { }
  init(callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/init`, {}, (response: ServerResponse) => {
      callback(response);
    });
  }
  search(model: LeadIndex, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/search`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  receiveLead(model: LeadReceive, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/receive`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  assignLead(model: LeadAssign, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/assign`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  assignMultiLead(adminIds: string, saleId: number, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/assignmultitosale`
      , { AdminIds: adminIds, UserId: saleId }
      , (response: ServerResponse) => {
        callback(response);
      });
  }

  initConvertedView(model: LeadConverted, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/initconvertedview`, model, (response: ServerResponse) => {
      callback(response);
    });
  }

  converted(model: LeadConverted, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/converted`, model, (response: ServerResponse) => {
      callback(response);
    });
  }

  reject(model: LeadReject, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/reject`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  addBlockRequest(model: LeadBlock, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/addblockrequest`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  block(model: LeadBlock, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/block`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  cancel(model: LeadBlock, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/cancel`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  unblock(model: LeadBlock, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/lead/unblock`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  addNote(model: LeadNote, blockUI: any, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequestAndBlockUI(environment.apiEndPoint + `api/lead/addnote`, model, blockUI, (response: ServerResponse) => {
      callback(response);
    });
  }
}
