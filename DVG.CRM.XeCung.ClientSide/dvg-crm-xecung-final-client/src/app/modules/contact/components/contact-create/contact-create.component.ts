import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DialogService } from 'src/app/services/dialog.service';
import { ContactService } from 'src/app/services/contact.service';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { GetdropdownlistService } from 'src/app/services/getdropdownlist.service';
import { ListDropDown, DropDownDic, ListLocation } from 'src/app/models/listdropdown';
import { Contactmodels } from 'src/app/models/contactmodels';
import { TokenService } from 'src/app/services/token.service';
import { KeyValueObject } from 'src/app/models/key-value';
import { ScriptLoaderService } from 'src/app/services/script-loader.service';

@Component({
  selector: 'app-contact-create',
  templateUrl: './contact-create.component.html',
  styleUrls: ['./contact-create.component.css']
})
export class ContactCreateComponent implements OnInit {
  contactModel: Contactmodels;
  dropdown: DropDownDic;
  location: ListLocation;
  contactcreate = true;
  constructor(
    private contactService: ContactService,
    private router: Router,
    private dialog: DialogService,
    private dropdownlist: GetdropdownlistService,
    private token: TokenService,
    private activedRoute: ActivatedRoute,
    private script: ScriptLoaderService
  ) {
    this.contactModel = new Contactmodels();
    this.dropdown = new DropDownDic();
    this.location = new ListLocation();
  }

  ngOnInit() {
    this.activedRoute.params.subscribe(param => {
      const contactId = +param.id;
      if (contactId > 0) {
        this.contactService.getById(contactId, (response: ServerResponse) => {
          switch (response.Code) {
            case ResponseCode.Success:
              this.token.getTokenUpdateView(contactId, (tokenRes: ServerResponse) => {
                if (tokenRes.Code === ResponseCode.Success) {
                  this.contactModel = response.Data as Contactmodels;
                  this.contactcreate = false;
                  if (response.Data != null) {
                    // Get các dropdownlistUser
                    this.getDropdownList();
                    // Get city region
                    if (localStorage.getItem('Location') == null || localStorage.getItem('Location') === undefined) {
                      this.dropdownlist.getdropdownLocation((res: ServerResponse) => {
                        if (res.Code === ResponseCode.Success) {
                          localStorage.setItem('Location', JSON.stringify(res.Data as ListLocation));
                        }
                      });
                    }
                    this.location.AllCityRegion = JSON.parse(localStorage.getItem('Location')).AllCityRegion as KeyValueObject[];
                  }
                  this.contactModel.Token = tokenRes.Data;
                }
              });
              break;
            case ResponseCode.Error:
              this.dialog.showError('ERROR: ' + response.Message);
              break;
            case ResponseCode.Warning:
              this.dialog.showWarning('WARNING: ' + response.Message);
              break;
            default: break;
          }
        });
      } else {
        // Get các dropdownlistUser
        this.getDropdownList();
        // Get city region
        if (localStorage.getItem('Location') == null || localStorage.getItem('Location') === undefined) {
          this.dropdownlist.getdropdownLocation((res: ServerResponse) => {
            if (res.Code === ResponseCode.Success) {
              localStorage.setItem('Location', JSON.stringify(res.Data as ListLocation));
            }
          });
        }
        this.location.AllCityRegion = JSON.parse(localStorage.getItem('Location')).AllCityRegion as KeyValueObject[];
      }
    });

  }
  // Get list CityRegion
  public getDropdownList() {
    this.dropdownlist.getdropdownlistContact((res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        this.dropdown.ListOrganization = res.Data.ListOrganization as { [key: number]: string };
      }
    });
  }
  ngAfterViewInit(): void {
    this.script.loadScripts('body', ['assets/demo/default/custom/components/forms/widgets/select2.js']);
    this.script.loadScripts('app-contact-create', ['./assets/js-custom/select2init.js']);
    const component = this;
    $('#dllCityRegionCreate').on('change', () => {
      component.changeCity(parseInt($('#dllCityRegionCreate').val(), 10));
    });
  }
  // submit form
  public update() {
    this.contactService.update(this.contactModel, (res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        if (this.contactModel.Id > 0) {
          this.router.navigate(['/contact']);
        } else {
          this.dialog.showSuccess(res.Message, '', () => {
            this.router.navigate(['/contact']);
          });
        }
      } else {
        this.dialog.showWarning(res.Message);
      }
    });
  }
  // reset form
  public refresh() {
      $('#dllCityRegionCreate').val(null).trigger('change');
      this.contactModel = new Contactmodels();
  }
  closeForm() {
    this.router.navigate(['/contact']);
  }
  changeCity(newValue: number) {
    this.contactModel.CityRegion = newValue;
  }
}
