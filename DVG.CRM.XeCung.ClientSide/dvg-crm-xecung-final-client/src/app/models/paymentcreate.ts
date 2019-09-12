export class PaymentCreate {
    constructor() {
        this.Urls = new Array<string>();
        this.AdvertiseType = 0;
        this.Package = 0;
    }
    CustomerId: number;
    Token: string;
    PurchaseValue: string;
    Urls: Array<string>;
    AdvertiseType: number;
    TransactionDate: string;
    Quantity: number;
    Package: number;
}
