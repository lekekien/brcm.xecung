import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VideoEdit } from 'src/app/models/video-edit';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
import { VideoIdRequest } from 'src/app/models/video-id-request';
import { VideoService } from 'src/app/services/video.service';
import { ResponseCode } from 'src/app/enums/responsecode';
import { ServerResponse } from 'src/app/models/response';
import { DialogService } from 'src/app/services/dialog.service';
import { ScriptLoaderService } from 'src/app/services/script-loader.service';
import { VideoViewInit } from 'src/app/models/video-view-init';
import { KeyValueObject } from 'src/app/models/key-value';
import { NgModel } from '@angular/forms';
import { VideoType } from 'src/app/enums/videotype';

@Component({
  selector: 'app-video-edit',
  templateUrl: './video-edit.component.html',
  styleUrls: ['./video-edit.component.css']
})
export class VideoEditComponent extends BaseComponent implements OnInit {
  videoCode: string;
  videoEditModel: VideoEdit;
  oldVideoEditModel: VideoEdit;
  videoCreateInit: VideoViewInit;
  @ViewChild('videocode') videoCodeTextBox: NgModel;
  constructor(
    private activeRoute: ActivatedRoute,
    private videoService: VideoService,
    private dialog: DialogService,
    private script: ScriptLoaderService,
    private router: Router,
  ) { 
    super();
    this.videoEditModel = new VideoEdit();
    this.videoCreateInit = new VideoViewInit();
  }

  ngOnInit() {
    this.activeRoute.params.subscribe(params => {
      this.videoCode = params.id;
      console.log(this.videoCode);
    });
    const modelForEdit = new VideoIdRequest();
    modelForEdit.VideoCode = this.videoCode;
    modelForEdit.Token = '';
    this.videoService.initEditView(modelForEdit, (response: ServerResponse) => {
      if (response.Code === ResponseCode.Success) {
        this.videoEditModel = response.Data.VideoEdit as VideoEdit;
        this.videoCreateInit.AllVideoType = response.Data.AllVideoType as KeyValueObject[];
        this.videoCreateInit.AllExpenditureInKeyValue = response.Data.AllExpenditureInKeyValue as KeyValueObject[];
        this.videoCreateInit.AllCostContent = response.Data.AllCostContent as KeyValueObject[];
        this.oldVideoEditModel = Object.assign({}, this.videoEditModel) as VideoEdit;
      } else if (response.Code === ResponseCode.Error) {
        this.dialog.showError(response.Message);
        this.router.navigateByUrl('/videos');
      } else {
        this.dialog.showWarning(response.Message);
        this.router.navigateByUrl('/videos');
      }
    });
  }
  public changeVideoType(event: number){
    
    this.videoEditModel.VideoType = event;
    if(this.videoEditModel.VideoType == this.oldVideoEditModel.VideoType) {
      this.videoEditModel.VideoCode = this.oldVideoEditModel.VideoCode;
      //disable textbox
    }
    else{
      //gen mã video mới
    }
    //---------------------
      this.videoEditModel.ReceiptIssuedDate = this.oldVideoEditModel.ReceiptIssuedDate;
      this.videoEditModel.ContractID = this.oldVideoEditModel.ContractID;
      this.videoEditModel.Revenue = this.oldVideoEditModel.Revenue;
  }
  public update() {
    if (this.videoEditModel.VideoType == VideoType.ContractVideo){
      this.videoEditModel.ReceiptIssuedDate = null;
    }
    else if (this.videoEditModel.VideoType == VideoType.NonContractVideo) {
      this.videoEditModel.ContractID = null;
    }
    else if (this.videoEditModel.VideoType == VideoType.SelfProductionVideo)
    {
      this.videoEditModel.ReceiptIssuedDate = null;
      this.videoEditModel.ContractID = null;
      this.videoEditModel.Revenue = null;
    }
    else {

    }
    this.videoService.edit(this.videoEditModel, (resposne: ServerResponse) => {
      if (resposne.Code === ResponseCode.Success) {
        this.dialog.showSuccess(resposne.Message, '', () => {
          this.router.navigateByUrl('/videos');
        });
      } else if (resposne.Code === ResponseCode.Error) {
        //---------------------
        this.videoEditModel.ReceiptIssuedDate = this.oldVideoEditModel.ReceiptIssuedDate;
        this.videoEditModel.ContractID = this.oldVideoEditModel.ContractID;
        this.videoEditModel.Revenue = this.oldVideoEditModel.Revenue;
        this.dialog.showError(resposne.Message);
      } else {
        //---------------------
        this.videoEditModel.ReceiptIssuedDate = this.oldVideoEditModel.ReceiptIssuedDate;
        this.videoEditModel.ContractID = this.oldVideoEditModel.ContractID;
        this.videoEditModel.Revenue = this.oldVideoEditModel.Revenue;
        this.dialog.showWarning(resposne.Message);
      }
    });
  }
}
