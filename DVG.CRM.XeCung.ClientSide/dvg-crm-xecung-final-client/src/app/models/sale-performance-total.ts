import { SalePerformance } from './sale-performance';

export class SalePerformanceTotal {
    Leads: number;
    ConvertedFile: number;
    AddedCustomers: number;
    TotalCustomers: number;
    ActiveCustomers: number;
    TotalListings: number;
    WorkingCustomers: number;

    constructor() {
        this.refresh();
    }

    refresh() {
        this.Leads = 0;
        this.ConvertedFile = 0;
        this.AddedCustomers = 0;
        this.TotalCustomers = 0;
        this.ActiveCustomers = 0;
        this.TotalListings = 0;
        this.WorkingCustomers = 0;
    }
    calcPercentConverted() {
        return this.Leads === 0 ? 0 : (100 * Math.round(100 * this.ConvertedFile / this.Leads ) / 100);
    }
}
