import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerService } from 'src/app/services/customer.service';
import { ContactService } from 'src/app/services/contact.service';
import { ResponseCode } from 'src/app/enums/responsecode';
import { DialogService } from 'src/app/services/dialog.service';
import { CustomerDetail } from 'src/app/models/customer-detail';
import { ServerResponse } from 'src/app/models/response';
import { ContactAssign } from 'src/app/models/contact-assign';
import { CustomerContactAssign } from 'src/app/models/customer-contact-assign';
import { TokenService } from 'src/app/services/token.service';
import { ContactFilter } from 'src/app/models/contact-filter';
import { from } from 'rxjs';
import { CustomerDetailAndPaging } from 'src/app/models/customer-detail-and-paging';
import { CustomerPostingLoginModel } from 'src/app/models/customer-posting-login-model';
import { CustomerActivityHistory } from 'src/app/models/customer-activity-history';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
declare var $: any;

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.css']
})
export class CustomerDetailComponent extends BaseComponent implements OnInit {
  customerId: number;
  customerDetail: CustomerDetail;
  contactList: ContactAssign[];
  customerContactAssign: CustomerContactAssign;
  contactFilter: ContactFilter;
  constructor(
    private activeRoute: ActivatedRoute
    , private customerService: CustomerService
    , private dialogService: DialogService
    , private tokenService: TokenService
    , private router: Router
    , private contactService: ContactService) {
    super();
    this.customerDetail = new CustomerDetail();
    this.contactList = [];
    this.customerContactAssign = new CustomerContactAssign();
    this.contactFilter = new ContactFilter();
  }

  ngOnInit() {
    this.activeRoute.params.subscribe(params => {
      this.customerId = +params.id;
    });
    this.customerService.getDetail(this.customerId, (response: ServerResponse) => {
      if (response.Code === ResponseCode.Success) {
        this.customerDetail = response.Data as CustomerDetail;

      } else if (response.Code === ResponseCode.Error) {
        this.dialogService.showError(response.Message);
        this.router.navigateByUrl('/customer');
      } else {
        this.dialogService.showWarning(response.Message);
        this.router.navigateByUrl('/customer');
      }
    });
  }
  getTokenAssignContact(customerId: number) {
    this.customerContactAssign.CustomerId = customerId;
    this.tokenService.getTokenUpdateView(customerId, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        this.customerContactAssign.Token = tokenResponse.Data;
        this.searchContact(1);
      } else {
        this.customerContactAssign.Token = '';
      }
    });
  }
  searchContact(pageIndex: number) {
    this.contactFilter.PageIndex = pageIndex;
    this.contactFilter.PageSize = 10;
    this.contactService.getbyfilter(this.contactFilter, (response: ServerResponse) => {
      if (response.Code === ResponseCode.Success) {
        this.contactFilter = response.Data as ContactFilter;
      } else if (response.Code === ResponseCode.Error) {
        this.dialogService.showError(response.Message);
      } else {
        this.dialogService.showWarning(response.Message);
      }
    });
  }
  getListCareHistory(pageIndex: number) {
    const model = new CustomerDetailAndPaging();
    model.CustomerId = this.customerDetail.Id;
    model.PageIndex = pageIndex;
    this.customerService.getListCareHistory(model, (response: ServerResponse) => {
      if (response.Code === ResponseCode.Success) {
      } else if (response.Code === ResponseCode.Error) {
        this.dialogService.showError(response.Message);
      } else {
        this.dialogService.showWarning(response.Message);
      }
    });
  }
  getListActivityHistory(pageIndex: number) {
    const model = new CustomerDetailAndPaging();
    model.CustomerId = this.customerDetail.Id;
    model.PageIndex = pageIndex;
    this.customerService.getListActivityHistory(model, (response: ServerResponse) => {
      if (response.Code === ResponseCode.Success) {
      } else if (response.Code === ResponseCode.Error) {
        this.dialogService.showError(response.Message);
      } else {
        this.dialogService.showWarning(response.Message);
      }
    });
  }
}
