import { SalePerformance } from './sale-performance';

export class SalePerformanceResult {
    constructor() {
        this.Leads = 0;
        this.ConvertedFile = 0;
        this.AddedCustomers = 0;
        this.TotalCustomers = 0;
        this.ActiveCustomers = 0;
        this.TotalListings = 0;
        this.WorkingCustomers = 0;
    }

    Result: SalePerformance[];
    TotalRecord: number;
    Leads: number;
    ConvertedFile: number;
    AddedCustomers: number;
    TotalCustomers: number;
    ActiveCustomers: number;
    TotalListings: number;
    WorkingCustomers: number;
}
