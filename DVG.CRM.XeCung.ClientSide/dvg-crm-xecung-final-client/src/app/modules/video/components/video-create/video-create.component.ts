import { Component, OnInit, ViewChild } from '@angular/core';
import { VideoCreate } from 'src/app/models/video-create';
import { BaseComponent } from 'src/app/modules/sharedcomponent/components/base/base.component';
import { VideoViewInit } from 'src/app/models/video-view-init';
import { ServerResponse } from 'src/app/models/response';
import { ResponseCode } from 'src/app/enums/responsecode';
import { KeyValueObject } from 'src/app/models/key-value';
import { VideoService } from 'src/app/services/video.service';
import { DialogService } from 'src/app/services/dialog.service';
import { ProductionCostType } from 'src/app/enums/productioncosttype';
import { ProductionCost } from 'src/app/models/productioncost';
import { ScriptLoaderService } from 'src/app/services/script-loader.service';
import { NgForm } from '@angular/forms';
import { DatepickercustomComponent } from 'src/app/modules/sharedcomponent/components/datepickercustom/datepickercustom.component';

@Component({
  selector: 'app-video-create',
  templateUrl: './video-create.component.html',
  styleUrls: ['./video-create.component.css']
})
export class VideoCreateComponent extends BaseComponent implements OnInit {
  @ViewChild('formProductionCost') formProductionCost: NgForm;
  @ViewChild('pcspenddate') pcspenddate: DatepickercustomComponent;
  videoCreateModel: VideoCreate;
  videoCreateInit: VideoViewInit;
  ProductionCostType: number;//lưu giá trị để kiểm tra xem loại chi phí đang cần thêm là loại nào(1 là chi phí ước tính, 2 là chi phí thực tế)
  ProductionCostCreate: ProductionCost; // lưu thông tin chi phí sản xuất cần thêm
  constructor(
    private videoService: VideoService,
    private dialog: DialogService,
    private script: ScriptLoaderService,
  ) {
    super();
    this.videoCreateModel = new VideoCreate();
    this.videoCreateInit = new VideoViewInit();
    this.ProductionCostCreate = new ProductionCost();
  }

  ngOnInit() {
    this.InitView();
  }
  ngAfterViewInit() {
    this.script.loadScripts('app-video-create', ['./assets/js-custom/select2init.js']);
    $('#expenditureType').on('change', () => {
      this.changeCostType(parseInt($('#expenditureType').val(), 10));
    });
  }
  private InitView(): void {
    this.videoService.initCreateView((resposne: ServerResponse) => {
      if (resposne.Code === ResponseCode.Success) {
        this.videoCreateInit.AllVideoType = resposne.Data.AllVideoType as KeyValueObject[];
        this.videoCreateInit.AllExpenditureInKeyValue = resposne.Data.AllExpenditureInKeyValue as KeyValueObject[];
        this.videoCreateInit.AllCostContent = resposne.Data.AllCostContent as KeyValueObject[];
      } else if (resposne.Code === ResponseCode.Error) {
        this.dialog.showError(resposne.Message);
      } else {
        this.dialog.showWarning(resposne.Message);
      }
    });
  }
  
  public resetProductionCostCreate(): void {
    //console.log(this.ProductionCostCreate.SpendDate);
    this.ProductionCostCreate = new ProductionCost();
    //this.ProductionCostCreate.SpendDate = null;
    this.formProductionCost.resetForm();
    $('#expenditureType').val(null).trigger('change');
    //this.pcspenddate.defaultValue = null;
  }
  public addProductionCost(): void {
    let EstimatedProductionCost: number = Number.isNaN(+this.videoCreateModel.EstimatedProductionCost) ? 0 : +this.videoCreateModel.EstimatedProductionCost;
    let ActualProductionCost: number = Number.isNaN(+this.videoCreateModel.ActualProductionCost) ? 0 : +this.videoCreateModel.ActualProductionCost;
    //--------------------
    this.ProductionCostCreate.CostTypeText = this.videoCreateInit.AllExpenditureInKeyValue.filter(item => item.Key == this.ProductionCostCreate.CostType)[0].Value;
    this.ProductionCostCreate.CostContentText = this.videoCreateInit.AllCostContent.filter(item => item.Key == this.ProductionCostCreate.CostContent)[0].Value;
    this.ProductionCostCreate.ProductionCostType = this.ProductionCostType;
    if (this.ProductionCostType == ProductionCostType.Estimated) {
      this.videoCreateModel.EstimatedProductionCostRecords.push(this.ProductionCostCreate);
      EstimatedProductionCost += +this.ProductionCostCreate.Amount;
      this.videoCreateModel.EstimatedProductionCost = EstimatedProductionCost.toString();
      $('#closeAddProductioncost').trigger('click');
    }
    else if (this.ProductionCostType == ProductionCostType.Actual) {
      this.videoCreateModel.ActualProductionCostRecords.push(this.ProductionCostCreate);
      ActualProductionCost += +this.ProductionCostCreate.Amount;
      this.videoCreateModel.ActualProductionCost = ActualProductionCost.toString();
      $('#closeAddProductioncost').trigger('click');
    }
    else {
      //do nothing for now
    }
  }
  public create():void {
    console.log("Create");
    this.videoService.create(this.videoCreateModel,(response: ServerResponse) => {
      if (response.Code === ResponseCode.Success) {        
      } else if (response.Code === ResponseCode.Error) {
        this.dialog.showError(response.Message);
      } else {
        this.dialog.showWarning(response.Message);
      }
    });
  }
  public changeCostType(expenditureType: number) {
    this.ProductionCostCreate.CostType = expenditureType;
  }
  public deleteProductionCost(productionCost: ProductionCost): void {
    console.log(productionCost);
  }
}
