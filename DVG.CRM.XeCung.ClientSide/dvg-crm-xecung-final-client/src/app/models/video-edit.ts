import { MonoTypeOperatorFunction } from 'rxjs';
import { ProductionCost } from './productioncost';

export class VideoEdit {
    constructor() {
        this.EstimatedProductionCostRecords = [];
        this.ActualProductionCostRecords = [];
        this.EstimatedProductionCost ="0";
        this.ActualProductionCost ="0";
    }
    Id: number;
    VideoCode: string;
    VideoType: number;
    Title: string;
    Link: string;
    Note: string;
    PublishDate: string = null;
    SpendDate: string = null;
    InvoiceIssuedDate: string = null;
    Revenue: string;
    ReceiptIssuedDate: string = null;
    ContractID: string;
    EstimatedProductionCost: string;
    EstimatedProductionCostRecords: ProductionCost[];
    ActualProductionCost: string;
    ActualProductionCostRecords: ProductionCost[];
}