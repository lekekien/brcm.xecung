import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ResponseCode } from 'src/app/enums/responsecode';
import { CityLocation } from 'src/app/models/citylocation';
import { LeadConverted } from 'src/app/models/lead-converted';
import { ServerResponse } from 'src/app/models/response';
import { SysrequestService } from 'src/app/services/sysrequest.service';
import { DialogService } from 'src/app/services/dialog.service';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
import { LeadConvertedInit } from 'src/app/models/lead-converted-init';
import { LeadService } from 'src/app/services/lead.service';
import { KeyValueObject } from 'src/app/models/key-value';
import { ContactFilter } from 'src/app/models/contact-filter';
import { ContactService } from 'src/app/services/contact.service';
import { ScriptLoaderService } from 'src/app/services/script-loader.service';
declare var $: any;

@Component({
  selector: 'app-customerleadconvert',
  templateUrl: './customerleadconvert.component.html',
  styleUrls: ['./customerleadconvert.component.css']
})
export class CustomerleadconvertComponent extends BaseComponent implements OnInit, AfterViewInit {
  

  leadName: string;
  phoneNumber: string;
  email: string;
  address: string;
  leadConverted: LeadConverted;
  leadConvertedInit: LeadConvertedInit;
  lstChangedCity: any[] = [];
  contactFilter: ContactFilter;
  constructor(private activeRoute: ActivatedRoute,
              private leadService: LeadService,
              private router: Router,
              private dialog: DialogService,
              private script: ScriptLoaderService,
              private contactService: ContactService) {
    super();
    this.leadConvertedInit = new LeadConvertedInit();
    this.leadConverted = new LeadConverted();
    this.contactFilter = new ContactFilter();
    this.contactFilter.PageSize = 5;
    this.contactFilter.PageIndex = 1;
  }

  ngOnInit() {
    this.activeRoute.queryParams.subscribe(params => {
      const lead = JSON.parse(decodeURIComponent(params.data) );
      this.leadConverted.Id = lead.Id;
      this.leadConverted.LeadAdminId = lead.LeadAdminId;
      this.leadConverted.Name = lead.FullName;
      this.leadConverted.PhoneNumber = lead.PhoneNumber;
      this.leadConverted.Email = lead.Email;
      this.leadConverted.Address = lead.Address;
    });
    

    this.leadService.initConvertedView(this.leadConverted, (resposne: ServerResponse) => {
      if (resposne.Code === ResponseCode.Success) {
        this.leadConvertedInit.AllRegion = resposne.Data.AllRegion as KeyValueObject[];
        this.leadConvertedInit.AllCity = resposne.Data.AllCity as CityLocation[];
        this.leadConvertedInit.AllCustomerType = resposne.Data.AllCustomerType as KeyValueObject[];
        this.leadConvertedInit.AllGroup = resposne.Data.AllGroup as KeyValueObject[];
        this.leadConvertedInit.ListOrganization = resposne.Data.ListOrganization as KeyValueObject[];
        this.leadConvertedInit.ListPhone = resposne.Data.ListPhone as [];
      } else if (resposne.Code === ResponseCode.Error) {
        this.dialog.showError(resposne.Message);
      } else {
        this.dialog.showWarning(resposne.Message);
      }
    });
  }
  ngAfterViewInit(): void {
    this.script.loadScripts('body', ['assets/demo/default/custom/components/forms/widgets/select2.js']);
    this.script.loadScripts('app-customerleadconvert', ['./assets/js-custom/select2init.js']);
    const component = this;
    $('#selRegionCreate').on('change', () => {
      this.leadConverted.Region = parseInt($('#selRegionCreate').val(), 10);
      this.changeRegion();
    });
    $('#selCityCreate').on('change', () => {
      this.leadConverted.City = parseInt($('#selCityCreate').val(), 10);
    });
  }
  changeRegion() {
    this.lstChangedCity = [];
    for (const cityItem of this.leadConvertedInit.AllCity) {
      if (cityItem.RegionId.toString() === this.leadConverted.Region.toString()) {
        this.lstChangedCity.push({ Key: cityItem.Id, Value: cityItem.Name });
      }
    }
  }

  closeForm() {
    this.router.navigateByUrl('/lead');
  }

  converted() {
    console.log(this.leadConverted);
    
    this.leadService.converted(this.leadConverted, (resposne: ServerResponse) => {
      if (resposne.Code === ResponseCode.Success) {
        this.dialog.showSuccess(resposne.Message,'', () => {
          this.router.navigateByUrl('/customerlead');
        });
      } else if (resposne.Code === ResponseCode.Error) {
        this.dialog.showError(resposne.Message);
      } else {
        this.dialog.showWarning(resposne.Message);
      }
    });
  }
  searchContact(pageIndex: number) {
    this.contactFilter.PageIndex = pageIndex;
    this.contactService.getbyfilter(this.contactFilter, (response: ServerResponse) => {
      if (response.Code === ResponseCode.Success) {
        this.contactFilter = response.Data as ContactFilter;
        if (this.contactFilter.LstData.length > 0) {
          for (let index = 0; index < this.contactFilter.LstData.length; index++) {
            this.contactFilter.LstData[index].Index = (index + 1);
          }
        }
      } else if (response.Code === ResponseCode.Error) {
        this.dialog.showError(response.Message);
      } else {
        this.dialog.showWarning(response.Message);
      }
    });
  }
  assignCustomer(contactId: number, contactName: string) {
    this.leadConverted.Organization = contactId;
    this.leadConverted.OrganizationName = contactName;
  }
}
