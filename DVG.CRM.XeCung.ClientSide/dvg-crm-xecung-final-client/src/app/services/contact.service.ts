import { Injectable } from '@angular/core';
import { SysrequestService } from './sysrequest.service';
import { Paging } from '../models/pagingmodel';
import { ServerResponse } from '../models/response';
import { environment } from 'src/environments/environment';
import { Contactmodels } from '../models/contactmodels';
import { ContactFilter } from '../models/contact-filter';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  constructor(private systemRequest: SysrequestService) { }
  getList(pager: Paging, organizationtType: number, callback: (response: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/contact/getlist/' + organizationtType;
    this.systemRequest.sendRequest(url, pager, (res: ServerResponse) => {
      callback(res);
    });
  }
  update(contract: Contactmodels, callback: (response: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/contact/update';
    this.systemRequest.sendRequest(url, contract, (res: ServerResponse) => {
      callback(res);
    });
  }
  getById(id: number, callback: (response: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/contact/getbyid/' + id;
    this.systemRequest.sendRequest(url, id, (res: ServerResponse) => {
      callback(res);
    });
  }
  getbyfilter(model: ContactFilter, callback: (response: ServerResponse) => void ) {
    const url = environment.apiEndPoint + 'api/contact/getbyfilter' ;
    this.systemRequest.sendRequest(url, model, (res: ServerResponse) => {
      callback(res);
    });
  }
  downloadTemplate(callback: (response: ServerResponse) => void ) {
    const url = environment.apiEndPoint + 'api/contact/downloadTemplate' ;
    this.systemRequest.sendRequest(url, {}, (res: ServerResponse) => {
      callback(res);
    });
  }
  importExcel(model: FormData, callback: (response: ServerResponse) => void) {
    const url = environment.apiEndPoint + 'api/contact/uploadFile';
    this.systemRequest.importExcel(url, model, (res: ServerResponse) => {
      callback(res);
    });
  }
}
