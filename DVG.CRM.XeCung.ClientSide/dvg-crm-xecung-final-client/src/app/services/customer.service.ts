import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SysrequestService } from './sysrequest.service';
import { ServerResponse } from '../models/response';
import { environment } from 'src/environments/environment';
import { CustomerIndex } from '../models/customerindex';
import { CustomerCreate } from '../models/customer-create';
import { CustomerIdRequest } from '../models/customer-id-request';
import { CustomerUpdateModel } from '../models/customer-edit';
import { Paging } from '../models/pagingmodel';
import { CustomerNote } from '../models/customer-note';
import { CustomerReject } from '../models/customer-reject';
import { CustomerBlock } from '../models/customer-block';
import { CustomerBlockRequest } from '../models/customer-block-request';
import { CustomerContactAssign } from '../models/customer-contact-assign';
import { CustomerDetailAndPaging } from '../models/customer-detail-and-paging';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  constructor(private http: HttpClient, private sysrequest: SysrequestService ) { }
  init(callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/init`, {}, (response: ServerResponse) => {
      callback(response);
    });
  }
  initUpdateView(callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/initupdateview`, {}, (response: ServerResponse) => {
      callback(response);
    });
  }
  create(model: CustomerCreate, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/create`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  edit(model: CustomerUpdateModel, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/edit`, model, (response: ServerResponse) => {
      callback(response);
    });
  }

  searchPending(callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/search`, {}, (response: ServerResponse) => {
      callback(response);
    });
  }
  receiveCustomer(model: CustomerIdRequest, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/receive`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  search(model: CustomerIndex, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/search`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  assignCustomer(customerId: number, saleId: number, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/assign`
      , { CustomerId: customerId, UserId: saleId }
      , (response: ServerResponse) => {
        callback(response);
      });
  }
  assignMultiCustomer(customerIds: string, saleId: number, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/assignmulti`
      , { CustomerIds: customerIds, UserId: saleId }
      , (response: ServerResponse) => {
        callback(response);
      });
  }
  getDetail(customerId: number, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/getdetail/`+ customerId
      , { }
      , (response: ServerResponse) => {
        callback(response);
      });
  }
  searchByContactId(pager: Paging, organization: number, callback: (res: ServerResponse) => void) {
    const url = environment.apiEndPoint + `api/contact/getlistcustomer/` + organization;
    this.sysrequest.sendRequest(url, pager, (response: ServerResponse) => {
      callback(response);
    });
  }
  addNote(customerNote: CustomerNote, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/note`
                                        , customerNote, (response: ServerResponse) => {
          callback(response);
      });
  }
  agreeToPay(model: CustomerIdRequest, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/agreetopay`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  changeToConsidering(model: CustomerIdRequest, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/changetoconsidering`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  rejectAndNote(model: CustomerReject, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/rejectandnote`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  assigncontact(model: CustomerContactAssign, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/assigncontact`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  addBlockRequest(model: CustomerBlock, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/addblockrequest`, model
    , (response: ServerResponse) => {
      callback(response);
    });
  }
  block(model: CustomerBlockRequest, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/block`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  cancel(model: CustomerBlockRequest, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/cancel`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  unblock(model: CustomerBlockRequest, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/unblock`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  getListCareHistory(model: CustomerDetailAndPaging, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/getlistcarehistory`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  getListActivityHistory(model: CustomerDetailAndPaging, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/getlistactivityhistory`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  update(model: CustomerUpdateModel, callback: (res: ServerResponse) => void) {
    this.sysrequest.sendRequest(environment.apiEndPoint + `api/customer/update`, model, (response: ServerResponse) => {
      callback(response);
    });
  }
  // Resolve
  getbyidresolve(customerId: number) {
    const url = environment.apiEndPoint + `api/customer/getbyid/` + customerId;
    return this.sysrequest.sendRequestResolve(url, customerId);
  }
}
