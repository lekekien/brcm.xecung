import { Component, OnInit, AfterViewInit } from '@angular/core';
import { LeadIndex } from 'src/app/models/lead-index';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
import { LeadInit } from 'src/app/models/lead-init';
import { LeadService } from 'src/app/services/lead.service';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { UserSearchResult } from 'src/app/models/usersearchresult';
import { LeadSearch } from 'src/app/models/lead-search';
import { LeadSearchResult } from 'src/app/models/lead-search-result';
import { DialogService } from 'src/app/services/dialog.service';
import { LeadReceive } from 'src/app/models/lead-receive';
import { TokenService } from 'src/app/services/token.service';
import { LeadAssign } from 'src/app/models/lead-assign';
import { LeadReject } from 'src/app/models/lead-reject';
import { LeadDataProviderService } from '../../services/lead-data-provider.service';
import { KeyValueObject } from 'src/app/models/key-value';
import { Role } from 'src/app/enums/roles';
import { LeadBlock } from 'src/app/models/lead-block';
import { LeadNote } from 'src/app/models/lead-note';
declare var $: any;

@Component({
  selector: 'app-customerlead',
  templateUrl: './customerlead.component.html',
  styleUrls: ['./customerlead.component.css']
})
export class CustomerleadComponent extends BaseComponent implements OnInit, AfterViewInit {
  leadInit: LeadInit;
  leadReject: LeadReject;
  totalPendingLead: number;
  totalConsideringLead: number;
  totalRejectedLead: number;
  lstPendingLead: LeadSearch[];
  lstConsideringLead: LeadSearch[];
  lstRejectedLead: LeadSearch[];
  adminIdWaintingAssign: number;
  searchAssignee: UserSearchResult[];
  totalSearchAssignee: number;
  assigneePageSize: number;
  isSelectAll: boolean;
  isAssignMulti: boolean;
  leadBlock: LeadBlock;
  leadNote: LeadNote;

  constructor(private leadService: LeadService,
              private dialogService: DialogService,
              private tokenService: TokenService,
              public leadDataService: LeadDataProviderService) {
    super();
    if (this.leadDataService.leadIndex == null) {
      this.leadDataService.leadIndex = new LeadIndex();
    }
    this.leadInit = new LeadInit();
    this.leadReject = new LeadReject();
    this.leadReject.Type = 'Not interested';
    this.leadBlock = new LeadBlock();
    this.leadNote = new LeadNote();
    this.lstPendingLead = [];
    this.totalPendingLead = 0;
    this.totalConsideringLead = 0;
    this.totalRejectedLead = 0;
    this.searchAssignee = [];
    this.totalSearchAssignee = 0;
    this.assigneePageSize = 5;
    this.isSelectAll = false;
    this.isAssignMulti = false;
  }
  ngOnInit() {
    this.search(1);
    this.leadService.init((res: ServerResponse) => {
      if (res.Code === ResponseCode.Success) {
        this.leadInit.AllListing = res.Data.AllListing as KeyValueObject[];
        this.leadInit.AllAssignee = res.Data.AllAssignee as UserSearchResult[];
        this.leadInit.AllKeyValueAssignee = res.Data.AllKeyValueAssignee as KeyValueObject[];
        this.leadInit.AllReasonBlock = res.Data.AllReasonBlock as KeyValueObject[];
        if (this.leadInit.AllAssignee != null && this.leadInit.AllAssignee.length > 0) {
          this.searchAssigneeByKeyword('', 1);
        }
      }
    });
  }
  search(pageIndex: number) {
    if (this.leadDataService.leadIndex.FilterKeyword == null) {
      this.leadDataService.leadIndex.FilterKeyword = '';
    }
    this.leadDataService.leadIndex.PageIndex = pageIndex;
    this.leadDataService.leadIndex.PageSize = 15;
    this.leadService.search(this.leadDataService.leadIndex, (response: ServerResponse) => {
      if (response.Code === ResponseCode.Success) {
        if (this.leadDataService.leadIndex.Status === 1) {
          const result = response.Data as LeadSearchResult;
          this.lstPendingLead = result.ListData;
          this.totalPendingLead = result.TotalRecord;
        } else if (this.leadDataService.leadIndex.Status === 2) {
          const result = response.Data as LeadSearchResult;
          this.lstConsideringLead = result.ListData;
          this.totalConsideringLead = result.TotalRecord;
        } else if (this.leadDataService.leadIndex.Status === 4) {
          const result = response.Data as LeadSearchResult;
          this.lstRejectedLead = result.ListData;
          this.totalRejectedLead = result.TotalRecord;
        }
        this.isSelectAll = false;
      } else if (response.Code === ResponseCode.Error) {
        this.dialogService.showError(response.Message);
      } else {
        this.dialogService.showWarning(response.Message);
      }
    });
  }
  ngAfterViewInit(): void {
    const component = this;
    $('.date-picker').datepicker();
    $('input[name=assignfrom]').off('change').on('change', () => {
      component.leadDataService.leadIndex.CreatedFrom =  $('input[name=assignfrom]').val();
    });
    $('input[name=assignto]').off('change').on('change', () => {
      component.leadDataService.leadIndex.CreatedTo =  $('input[name=assignto]').val();
    });
  }
  getLead(id: number, adminId: number) {
    const model = new LeadReceive();
    model.Id = id;
    model.AdminId = adminId;
    this.tokenService.getTokenPopup(adminId, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        model.Token = tokenResponse.Data;
        this.dialogService.showConfirm('You are kindly required to contact this customer as soon as you confirm .'
        + ' Otherwise, it will immediately be coming back to the lead pool.', () => {
          this.leadService.receiveLead(model, (res: ServerResponse) => {
              if (res.Code === ResponseCode.Success) {
                this.dialogService.showSuccess(res.Message);
                this.search(1);
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
  assignLead(id: number) {
    const model = new LeadAssign();
    this.tokenService.getTokenPopup(this.adminIdWaintingAssign, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        model.Token = tokenResponse.Data;
        model.LeadAdminId = this.adminIdWaintingAssign;
        model.AssigneeId = id;
        this.leadService.assignLead(model, (res: ServerResponse) => {
          $('button[name=close]', $('#wrapAssignTo')).trigger('click');
          if (res.Code === ResponseCode.Success) {
            this.dialogService.showSuccess(res.Message);
            this.search(1);
          } else if (res.Code === ResponseCode.Warning) {
            this.dialogService.showWarning(res.Message);
          } else {
            this.dialogService.showError(res.Message);
          }
        });

      }
    });
  }
  assignMultiLead(id: number) {
    let leadIds = '';
    const lstLeadId = [];
    if (this.leadDataService.leadIndex.Status === 1) {
      const lstSelectedLead = this.lstPendingLead.filter((el, idx) => el.IsCheckedAssign === true);
      lstSelectedLead.map((el, idx) => lstLeadId.push(el.AdminId));
    } else if (this.leadDataService.leadIndex.Status === 2) {
      const lstSelectedLead = this.lstConsideringLead.filter((el, idx) => el.IsCheckedAssign === true);
      lstSelectedLead.map((el, idx) => lstLeadId.push(el.AdminId));
    } else if (this.leadDataService.leadIndex.Status === 4) {
      const lstSelectedLead = this.lstRejectedLead.filter((el, idx) => el.IsCheckedAssign === true);
      lstSelectedLead.map((el, idx) => lstLeadId.push(el.AdminId));
    }
    leadIds = lstLeadId.join();
    this.leadService.assignMultiLead(leadIds, id, (res: ServerResponse) => {
      $('button[name=close]', $('#wrapAssignTo')).trigger('click');
      if (res.Code === ResponseCode.Success) {
        this.dialogService.showSuccess(res.Message);
        this.search(1);
      } else if (res.Code === ResponseCode.Warning) {
        this.dialogService.showWarning(res.Message);
      } else {
        this.dialogService.showError(res.Message);
      }
    });
  }
  gettokenforreject(id: number) {
    this.leadReject.Id = id;
    this.tokenService.getTokenUpdateView(id, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        this.leadReject.Token = tokenResponse.Data;
      }
    });
  }
  rejectandnote(id: number) {
    if (this.leadReject.Type == null || this.leadReject.Type === '') {
      if (this.leadReject.Note == null || this.leadReject.Note === '' ) {
        this.leadReject.IsInvalid = true;
        return ;
      }
    }
    if (this.leadReject.Type !== '') {
      this.leadReject.Note = this.leadReject.Type;
    }
    this.leadService.reject(this.leadReject, (response: ServerResponse) => {
      $('button[name=close]', $('#wrapRejection')).trigger('click');
      if (response.Code === ResponseCode.Success) {
        this.dialogService.showSuccess(response.Message);
        this.search(1);
      } else if (response.Code === ResponseCode.Error) {
        this.dialogService.showError(response.Message);
      } else {
        this.dialogService.showWarning(response.Message);
      }
    });
  }
  searchAssigneeByKeyword(filterKeyword: string, pageIndex: number) {
    if (filterKeyword != null && filterKeyword !== '') {
      filterKeyword = filterKeyword.toLowerCase().trim();
    }
    this.searchAssignee = this.leadInit.AllAssignee.filter((item, index) =>
      (item.DisplayName + item.Email + item.UserName).toLowerCase().indexOf(filterKeyword) >= 0
      ) ;
    this.totalSearchAssignee = this.searchAssignee.length;
    this.searchAssignee = this.searchAssignee.filter((item, index) =>  index >= (pageIndex - 1) * 5 && index < pageIndex * 5 );
    for ( let index = 0; index < this.searchAssignee.length; index++) {
      this.searchAssignee[index].Index = (index + 1);
    }
  }
  isRoledManagerOrHigher() {
    return this.hasRole(Role.Manager) || this.hasRole(Role.Admin);
  }
  selectAll() {
    if (this.leadDataService.leadIndex.Status === 1) {
      this.lstPendingLead.map((el, index) => {
        if (el.BlockStatus === 0) {
          el.IsCheckedAssign = true;
        }
      });
    } else if (this.leadDataService.leadIndex.Status === 2) {
      this.lstConsideringLead.map((el, index) => {
        if (el.BlockStatus === 0) {
          el.IsCheckedAssign = true;
        }
      });
    } else if (this.leadDataService.leadIndex.Status === 4) {
      this.lstRejectedLead.map((el, index) => {
        if (el.BlockStatus === 0) {
          el.IsCheckedAssign = true;
        }
      });
    }
    this.isSelectAll = true;
  }
  unselectAll() {
    if (this.leadDataService.leadIndex.Status === 1) {
      this.lstPendingLead.map((el, index) => el.IsCheckedAssign = false);
    } else if (this.leadDataService.leadIndex.Status === 2) {
      this.lstConsideringLead.map((el, index) => el.IsCheckedAssign = false);
    } else if (this.leadDataService.leadIndex.Status === 4) {
      this.lstRejectedLead.map((el, index) => el.IsCheckedAssign = false);
    }
    this.isSelectAll = false;
  }
  isAssignPermission() {
    let isAssignMulti = true;
    if (this.leadDataService.leadIndex.Status === 1) {
      this.lstPendingLead.map((el, idx) => {
        if (el.IsCheckedAssign === true) {
          isAssignMulti = false;
        }
      });
    } else if (this.leadDataService.leadIndex.Status === 2) {
      this.lstConsideringLead.map((el, idx) => {
        if (el.IsCheckedAssign === true) {
          isAssignMulti = false;
        }
      });
    } else if (this.leadDataService.leadIndex.Status === 4) {
      this.lstRejectedLead.map((el, idx) => {
        if (el.IsCheckedAssign === true) {
          isAssignMulti = false;
        }
      });
    }
    return isAssignMulti;
  }
  addBlockRequest() {
    if (this.leadBlock.BlockReasonType <= 0 || this.leadBlock.BlockReasonType === undefined) {
      this.leadBlock.TypeIsInvalid = true;
      return ;
    }
    this.leadBlock.TypeIsInvalid = false;
    if (this.leadBlock.BlockReasonType.toString() === '3') {
      if (this.leadBlock.BlockReasonDescription == null || this.leadBlock.BlockReasonDescription === '' ) {
        this.leadBlock.NoteIsInvalid = true;
        return ;
      }
    }
    this.leadBlock.NoteIsInvalid = false;
    this.tokenService.getTokenPopup(this.leadBlock.LeadId, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        this.leadBlock.Token = tokenResponse.Data;
        this.leadService.addBlockRequest(this.leadBlock, (response: ServerResponse) => {
          $('button[name=close]', $('#wrapBlockUser')).trigger('click');
          if (response.Code === ResponseCode.Success) {
            this.dialogService.showSuccess(response.Message);
            this.lstConsideringLead.map((el, idx) => {
              if (el.Id === this.leadBlock.LeadId) {
                el.BlockStatus = 1;
                el.BlockReasonType = this.leadBlock.BlockReasonType;
                el.BlockReasonDescription = this.leadBlock.BlockReasonDescription;
              }
            });
          } else if (response.Code === ResponseCode.Error) {
            this.dialogService.showError(response.Message);
          } else {
            this.dialogService.showWarning(response.Message);
          }
        });
      }
    });
  }
  block(id: number) {
    const customer = new LeadBlock();
    customer.LeadId = id;
    this.tokenService.getTokenPopup(id, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        customer.Token = tokenResponse.Data;
        const message = 'Do you want to block this lead?';

        this.dialogService.showConfirm(message, () => {
          this.leadService.block(customer, (res: ServerResponse) => {
              if (res.Code === ResponseCode.Success) {
                this.dialogService.showSuccess(res.Message);
                this.search(1);
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
  cancel(id: number) {
    const leadBlock = new LeadBlock();
    leadBlock.LeadId = id;
    this.tokenService.getTokenPopup(id, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        leadBlock.Token = tokenResponse.Data;
        const message = 'Do you want cancel this block request ?';
        this.dialogService.showConfirm(message, () => {
          this.leadService.cancel(leadBlock, (res: ServerResponse) => {
              if (res.Code === ResponseCode.Success) {
                this.dialogService.showSuccess(res.Message);
                this.search(1);
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
  unblock(id: number) {
    const leadBlock = new LeadBlock();
    leadBlock.LeadId = id;
    this.tokenService.getTokenPopup(id, (tokenResponse: ServerResponse) => {
      if (tokenResponse.Code === ResponseCode.Success) {
        leadBlock.Token = tokenResponse.Data;
        const message = 'Do you want to unblock this lead ?';
        this.dialogService.showConfirm(message, () => {
          this.leadService.unblock(leadBlock, (res: ServerResponse) => {
              if (res.Code === ResponseCode.Success) {
                this.dialogService.showSuccess(res.Message);
                this.search(1);
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
    if (this.leadNote.Note == null || this.leadNote.Note === '' ) {
      this.leadNote.IsInvalid = true;
      return ;
    }
    this.leadNote.Note = this.currUser.UserName + ' : ' + this.leadNote.Note;
    this.leadService.addNote(this.leadNote, $('#wrapNote'), (response: ServerResponse) => {
      $('button[name=close]', $('#wrapNote')).trigger('click');
      if (response.Code === ResponseCode.Success) {
        this.dialogService.showSuccess(response.Message);
        this.lstConsideringLead.map((el, idx) => {
          if (el.Id === this.leadNote.Id) {
            el.Note = this.leadNote.Note;
          }
          this.leadNote = new LeadNote();
        });
      } else if (response.Code === ResponseCode.Error) {
        this.dialogService.showError(response.Message);
      } else {
        this.dialogService.showWarning(response.Message);
      }
    });
  }
}
