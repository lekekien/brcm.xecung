import { Component, OnInit, ViewEncapsulation, AfterViewInit, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
import { CustomerInit } from 'src/app/models/customerinit';
import { CustomerService } from 'src/app/services/customer.service';
import { ScriptLoaderService } from 'src/app/services/script-loader.service';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { UserSearchResult } from 'src/app/models/usersearchresult';
import { DialogService } from 'src/app/services/dialog.service';
import { CustomerSearchResult } from 'src/app/models/customersearchresult';
import { CustomerDataProviderService } from '../../services/customer-data-provider.service';
import { Router } from '@angular/router';
import { KeyValueObject } from 'src/app/models/key-value';
import { CustomerIdRequest } from 'src/app/models/customer-id-request';
import { TokenService } from 'src/app/services/token.service';
import { CustomerNote } from 'src/app/models/customer-note';
import { CustomerBlock } from 'src/app/models/customer-block';
import { CustomerReject } from 'src/app/models/customer-reject';
import { GetdropdownlistService } from 'src/app/services/getdropdownlist.service';
import { ListLocation } from 'src/app/models/listdropdown';
import { CustomerUpdateModel } from 'src/app/models/customer-edit';
declare var $: any;
@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  encapsulation: ViewEncapsulation.None
})
export class CustomerComponent extends BaseComponent implements OnInit {
  customerInit: CustomerInit;
  lstChangedCity: KeyValueObject[];
  searchAssignee: UserSearchResult[];
  totalSearchAssignee: number;
  customerIdWaintingAssign: number;
  customerNote: CustomerNote;
  customerBlock: CustomerBlock;
  customerReject: CustomerReject;
  location: ListLocation;
  isSelectAll: boolean;
  customerUpdate: CustomerUpdateModel;
  typeBlock = 0;
  index = 0;
  constructor(
    private customerService: CustomerService,
    private script: ScriptLoaderService,
    private dialogService: DialogService,
    public customerDataProvider: CustomerDataProviderService,
    private router: Router,
    private tokenService: TokenService,
    private dropdownlist: GetdropdownlistService,
  ) {
    super();
    this.lstChangedCity = [];
    this.customerInit = new CustomerInit();
    this.customerNote = new CustomerNote();
    this.customerBlock = new CustomerBlock();
    this.customerReject = new CustomerReject();
    this.location = new ListLocation();
    this.isSelectAll = false;
    this.customerUpdate = new CustomerUpdateModel();
    this.lstChangedCity = [];
    this.searchAssignee = [];
  }

  ngOnInit() {
    // get list
    this.customerDataProvider.customerIndex.AssigneeId = -1;
    this.customerService.init((res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        this.customerInit.AllCustomerScource = res.Data.AllCustomerScource as KeyValueObject[];
        this.customerInit.AllCustomerStatus = res.Data.AllCustomerStatus as KeyValueObject[];
        this.customerInit.AllCustomerType = res.Data.AllCustomerType as KeyValueObject[];
        // if (this.customerInit.AllAssignee != null && this.customerInit.AllAssignee.length > 0) {
        //   this.searchAssigneeByKeyword('', 1);
        // }
        this.search(this.customerDataProvider.customerIndex.PageIndex);
      }
    });
  }
  search(pageIndex: number = 1) {
    this.isSelectAll = false;
    if (this.customerDataProvider.customerIndex.FilterKeyword == null) {
      this.customerDataProvider.customerIndex.FilterKeyword = '';
    }
    this.customerDataProvider.customerIndex.PageIndex = pageIndex;
    this.customerService.search(this.customerDataProvider.customerIndex, (response: ServerResponse) => {
      if (response.Code === ResponseCode.Success) {
        const result = response.Data as CustomerSearchResult;
        this.customerDataProvider.lstCustomer = result.List;
        this.customerDataProvider.totalCustomer = result.TotalRow;
      } else if (response.Code === ResponseCode.Error) {
        this.dialogService.showError(response.Message);
      } else {
        this.dialogService.showWarning(response.Message);
      }
    });
  }
  
  changeAssignee(newValue: number) {
    if (newValue !== undefined && !Number.isNaN(newValue)) {
      this.customerDataProvider.customerIndex.AssigneeId = newValue;
    } else {
      this.customerDataProvider.customerIndex.AssigneeId = -1;
    }
  }
  getCustomer(customerId: number) {
    const receivedCustomer = new CustomerIdRequest();
    receivedCustomer.Id = customerId;
    this.tokenService.getTokenPopup(customerId, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        receivedCustomer.Token = tokenResponse.Data;
        this.dialogService.showConfirm("", () => {
          this.customerService.receiveCustomer(receivedCustomer, (res: ServerResponse) => {
            if (res.Code === ResponseCode.Success) {
              this.dialogService.showSuccess(res.Message);
              this.search(this.customerDataProvider.customerIndex.PageIndex);
            } else if (res.Code === ResponseCode.Warning) {
              this.dialogService.showWarning(res.Message);
            } else {
              this.dialogService.showError(res.Message);
            }
          });
        });
      }
    });
  }
  addNote() {
    if (this.customerNote.Note == null || this.customerNote.Note === '') {
      this.customerNote.IsInvalid = true;
      return false;
    }
    if (!this.customerNote.IsInvalid) {
      this.customerService.addNote(this.customerNote, (response: ServerResponse) => {
        this.dialogService.closeModal();
        if (response.Code === ResponseCode.Success) {
          this.dialogService.showSuccess(response.Message);
          this.customerDataProvider.lstCustomer
            .filter(item => item.Id === this.customerNote.Id)
            .map((el, index) => { el.Description = this.customerNote.Note; });
        } else if (response.Code === ResponseCode.Error) {
          this.dialogService.showError(response.Message);
        } else {
          this.dialogService.showWarning(response.Message);
        }
      });
    }

  }
  resetBlockReason(customerId: number) {
    this.customerBlock = new CustomerBlock();
    this.customerBlock.CustomerId = customerId;
  }
  blockCustomer() {
    if (this.customerBlock.BlockReasonType <= 0 || this.customerBlock.BlockReasonType === undefined) {
      this.customerBlock.TypeIsInvalid = true;
      return;
    }

    this.customerBlock.TypeIsInvalid = false;
    if (this.customerBlock.BlockReasonType.toString() === '3') {
      if (this.customerBlock.BlockReasonDescription == null || this.customerBlock.BlockReasonDescription === '') {
        this.customerBlock.NoteIsInvalid = true;
        return;
      }
    }
    this.customerBlock.NoteIsInvalid = false;
    this.tokenService.getTokenPopup(this.customerBlock.CustomerId, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        this.customerBlock.Token = tokenResponse.Data;
        this.customerService.block(this.customerBlock, (res: ServerResponse) => {
          this.dialogService.closeModal();
          if (res.Code === ResponseCode.Success) {
            this.dialogService.showSuccess(res.Message);
            this.search(this.customerDataProvider.customerIndex.PageIndex);
          } else if (res.Code === ResponseCode.Warning) {
            this.dialogService.showWarning(res.Message);
          } else {
            this.dialogService.showError(res.Message);
          }
        });
      }
    });
  }
  unBlockCustomer(customerId: number) {
    const customer = new CustomerBlock();
    customer.CustomerId = customerId;
    this.tokenService.getTokenPopup(customerId, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        customer.Token = tokenResponse.Data;
        const message = "Bạn muốn mở khóa cho khách hàng này?";
        this.dialogService.showConfirm(message, () => {
          this.customerService.unblock(customer, (res: ServerResponse) => {
            if (res.Code === ResponseCode.Success) {
              this.dialogService.showSuccess(res.Message);
              this.search(this.customerDataProvider.customerIndex.PageIndex);
            } else if (res.Code === ResponseCode.Warning) {
              this.dialogService.showWarning(res.Message);
            } else {
              this.dialogService.showError(res.Message);
            }
          });
        });
      }
    });
  }
}
