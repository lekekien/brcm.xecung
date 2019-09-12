export class PaymentIndex {
    FilterKeyword: string;
    AssignTo: string;
    Status: number;
    StartDate: string;
    EndDate: string;
    PageIndex: number;
    PageSize: number;
    constructor() {
        this.Status = -1;
        this.AssignTo = '0';
    }
}
