export class PaymentInput {
    constructor() {
        this.ShowInfo = false;
        this.Urls = [];
        this.AdvertiseType = 0;
        this.Package = 0;
    }
    Id: number;
    CustomerId: number;
    Token: string;
    Name: string;
    PhoneNumber: string;
    PurchaseValue: string;
    Email: string;
    Type: number;
    CustomerSource: number;
    Status: number;
    City: number;
    Address: string;
    TransactionDate: string;
    State: number;
    Description: string;
    AssigneeId: number;
    AdvertiseType: number;
    Quantity: number;
    Package: number;
    Note: string;
    ShowInfo: boolean;
    Urls: Array<string>;
    ConfirmBy: string;
    CreatorName: string;
    AdvertiseTypeValue: string;
    PackageValue: string;
    ConfirmationDate: string;
}
