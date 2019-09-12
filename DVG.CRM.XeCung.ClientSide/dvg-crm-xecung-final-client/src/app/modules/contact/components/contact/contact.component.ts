import { Component, OnInit } from '@angular/core';
import { ContactService } from 'src/app/services/contact.service';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { Router } from '@angular/router';
import { DialogService } from 'src/app/services/dialog.service';
import { Contactmodels } from 'src/app/models/contactmodels';
import { GetdropdownlistService } from 'src/app/services/getdropdownlist.service';
import { DropDownDic } from 'src/app/models/listdropdown';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
import { Role } from 'src/app/enums/roles';
import { Paging } from 'src/app/models/pagingmodel';
import { CustomerCreate } from 'src/app/models/customer-create';
import { CustomerService } from 'src/app/services/customer.service';
declare var $: any;

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent extends BaseComponent implements OnInit {
  lstContact: Contactmodels[];
  listCustomer: CustomerCreate[];
  dropdown: DropDownDic;
  index = 0;
  organizationType = 0;
  isAdmin: boolean;
  isManager: boolean;
  isSaler: boolean;
  paging: Paging;
  pagingCustomer: Paging;
  contactId = 0;
  constructor(
    private contactService: ContactService,
    private dropdownlist: GetdropdownlistService,
    private customerService: CustomerService,
    private dialogService: DialogService
  ) {
    super();
    this.dropdown = new DropDownDic();
    this.isAdmin = this.hasRole(Role.Admin);
    this.isManager = this.hasRole(Role.Manager);
    //this.isSaler = this.hasRole(Role.Saler);
    this.paging = new Paging();
    this.pagingCustomer = new Paging();
  }

  ngOnInit() {
    this.getList(this.paging.PageIndex);
    this.getDropdownList();
  }
  public getList(pageIndex: number) {
    this.paging.PageIndex = pageIndex;
    this.contactService.getList(this.paging, this.organizationType, (res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        this.lstContact = res.Data.ListContact as Contactmodels[];
        this.paging.TotalRecord = res.Data.TotalRecord;
      }
    });
  }
  // Get list customer
  public getListCustomer(pageIndex: number, itemContactId: number) {
    this.pagingCustomer.PageIndex = pageIndex;
    this.contactId = itemContactId;
    this.customerService.searchByContactId(this.pagingCustomer, this.contactId, (res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        this.listCustomer = res.Data.ListCustomer as CustomerCreate[];
        this.pagingCustomer.TotalRecord = res.Data.TotalRecord;
      }
    });
  }
  // Get list Organization
  public getDropdownList() {
    this.dropdownlist.getdropdownlistContact((res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        this.dropdown.ListOrganization = res.Data.ListOrganization as { [key: number]: string };
      }
    });
  }
  handleFileInput(files) {
    if (files.length === 0) {
      return;
    }

    const formData = new FormData();

    for (const file of files) {
      formData.append(file.name, file);
    }

    this.contactService.importExcel(formData, (res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        this.dialogService.showSuccess(res.Message);
      }
      if (res.Code === ResponseCode.Warning) {
        this.dialogService.showWarning(res.Message);
      }
      if (res.Code === ResponseCode.Error) {
        this.dialogService.showError(res.Message);
      }
    });
  }
  //temp
  // downloadTemplate() {
  //   this.contactService.downloadTemplate()
  //     .subscribe(result => {
  //       const url = window.URL.createObjectURL(result);
  //       window.open(url);
  //     });
  // }
  downloadTemplate() {
    this.contactService.downloadTemplate((res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        const blob = new Blob([this.s2ab(atob(res.Data))], {
            type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,'
        });
        const url = window.URL.createObjectURL(blob);
        window.open(url);
      } else if (res.Code === ResponseCode.Error) {
        this.dialogService.showError(res.Message);
      } else {
        this.dialogService.showWarning(res.Message);
      }
    });
  }
}
