import { Component, OnInit, AfterViewInit, ViewEncapsulation, ChangeDetectorRef, ViewChild } from '@angular/core';
import { CustomerUpdateInit } from 'src/app/models/customer-create-init';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
import { CustomerService } from 'src/app/services/customer.service';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { DialogService } from 'src/app/services/dialog.service';
import { ScriptLoaderService } from 'src/app/services/script-loader.service';
import { Router, ActivatedRoute } from '@angular/router';
import { KeyValueObject } from 'src/app/models/key-value';
import { CustomerUpdateModel } from 'src/app/models/customer-edit';
import { TokenService } from 'src/app/services/token.service';
import { GetdropdownlistService } from 'src/app/services/getdropdownlist.service';
import { ListLocation } from 'src/app/models/listdropdown';
import { ContactService } from 'src/app/services/contact.service';
import { NgForm } from '@angular/forms';
declare var $: any;
@Component({
  selector: 'app-customerupdate',
  templateUrl: './customerupdate.component.html',
})
export class CustomerUpdateComponent extends BaseComponent implements OnInit {
  customerUpdateInit: CustomerUpdateInit;
  customerUpdate: CustomerUpdateModel;
  oldCustomerUpdate: CustomerUpdateModel;
  lstChangedCity: KeyValueObject[];
  isEdit = false;
  location: ListLocation;
  lang: any;
  @ViewChild('f') customerUpdateForm: NgForm;
  constructor(
      private customerService: CustomerService
    , private script: ScriptLoaderService
    , private router: Router
    , private routerActive: ActivatedRoute
    , private tokenService: TokenService
    , private dialog: DialogService
    , private dropdownlist: GetdropdownlistService
    , private cd: ChangeDetectorRef
  ) {
    super();
    this.customerUpdate = new CustomerUpdateModel();
    this.oldCustomerUpdate = new CustomerUpdateModel();
    this.customerUpdateInit = new CustomerUpdateInit();
    this.lstChangedCity = [];
    this.location = new ListLocation();
  }
  ngOnInit() {
    const response = this.routerActive.snapshot.data.item;
    if (response != null) {
      switch (response.Code) {
        case ResponseCode.Success:
          if (response.Data != null) {
            this.customerUpdate = response.Data as CustomerUpdateModel;
            this.customerUpdate.BirthdayConvert = this.customerUpdate.Birthday != null ? this.customerUpdate.Birthday : '';
            this.isEdit = true;
            this.tokenService.getTokenUpdateView(response.Data.Id, (tokenResponse: ServerResponse) => {
              if (tokenResponse.Code === ResponseCode.Success) {
                this.customerUpdate.Token = tokenResponse.Data;
                this.oldCustomerUpdate = Object.assign({}, this.customerUpdate) as CustomerUpdateModel;
              }
            });
          }
          break;
        case ResponseCode.Error:
          this.dialog.showError(response.Message);
          break;
        case ResponseCode.Warning:
          this.dialog.showWarning(response.Message);
          break;
        default: break;
      }
    }
    this.getdropdownlist();
  }
  getdropdownlist() {
    this.customerService.initUpdateView((res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        this.customerUpdateInit.AllCustomerScource = res.Data.AllCustomerScource;
        this.customerUpdateInit.AllCustomerType = res.Data.AllCustomerType;
        this.customerUpdateInit.AllCustomerStatus = res.Data.AllCustomerStatus;
      }
    });
  }
  update() {
    console.log(this.customerUpdate);
    return;
    this.customerService.update(this.customerUpdate, (resposne: ServerResponse) => {
      if (resposne.Code === ResponseCode.Success) {
        this.customerUpdate = Object.assign({}, this.oldCustomerUpdate) as CustomerUpdateModel;
        this.dialog.showSuccess(resposne.Message, '', () => {
          this.router.navigate(['/customer']);
        });
      } else if (resposne.Code === ResponseCode.Error) {
        this.dialog.showError(resposne.Message);
      } else {
        this.dialog.showWarning(resposne.Message);
      }
    });

  }
  // reset() {
  //   if (this.isEdit) {
  //     const stateId = this.customerUpdate.State;
  //     const cityId = this.customerUpdate.City;
  //     this.customerUpdate = Object.assign({}, this.oldCustomerUpdate) as CustomerEdit;
  //     // this.cd.detectChanges();
  //     if (stateId !== this.oldCustomerUpdate.State || cityId !== this.oldCustomerUpdate.City) {
  //       this.changeState(this.oldCustomerUpdate.State);
  //       this.location.AllRegion = JSON.parse(localStorage.getItem('Location')).AllRegion as { [key: number]: string };
  //       this.location.AllCityModel = JSON.parse(localStorage.getItem('Location')).AllCityModel;
  //     }
  //   } else {
  //     this.customerUpdateForm.onReset();
  //     this.cd.detectChanges();
  //     this.customerUpdate = new CustomerEdit();
  //     this.location.AllRegion = JSON.parse(localStorage.getItem('Location')).AllRegion as { [key: number]: string };
  //     this.location.AllCityModel = JSON.parse(localStorage.getItem('Location')).AllCityModel;
  //   }
  // }
  // changeState(newValue: number) {
  //   if (newValue !== null && !Number.isNaN(newValue)) {
  //     this.customerUpdate.State = newValue;
  //   } else {
  //     this.customerUpdate.State = 0;
  //     this.customerUpdate.City = 0;
  //   }
  //   this.lstChangedCity = [];
  //   for (const key in this.location.AllCityModel) {
  //     if (this.location.AllCityModel[key].StateId === this.customerUpdate.State) {
  //       const newObject = new KeyValueObject();
  //       newObject.Key = this.location.AllCityModel[key].Id;
  //       newObject.Value = this.location.AllCityModel[key].CityName;
  //       this.lstChangedCity.push(newObject);
  //     }
  //   }
  // }
  // changeCity(newValue: number, cityName: string) {
  //   if (newValue !== null && !Number.isNaN(newValue)) {
  //     this.customerUpdate.City = newValue;
  //     this.customerUpdate.CityName = cityName;
  //   } else {
  //     this.customerUpdate.City = 0;
  //     this.customerUpdate.CityName = '';
  //   }
  // }
  // isUpdated() {
  //   if (this.isEdit
  //     && (this.oldCustomerUpdate.Name !== this.customerUpdate.Name
  //       || this.oldCustomerUpdate.PhoneNumber !== this.customerUpdate.PhoneNumber
  //       || this.oldCustomerUpdate.Email !== this.customerUpdate.Email
  //       || this.oldCustomerUpdate.State !== this.customerUpdate.State
  //       || this.oldCustomerUpdate.City !== this.customerUpdate.City
  //       || this.oldCustomerUpdate.Organization !== this.customerUpdate.Organization
  //       || this.oldCustomerUpdate.IsFavorite !== this.customerUpdate.IsFavorite
  //       || this.oldCustomerUpdate.Type !== this.customerUpdate.Type
  //       || this.oldCustomerUpdate.Group !== this.customerUpdate.Group
  //       || this.oldCustomerUpdate.Address !== this.customerUpdate.Address
  //       || this.oldCustomerUpdate.Description !== this.customerUpdate.Description)) {
  //     return true;
  //   } else {
  //     return false;
  //   }
  // }
}
