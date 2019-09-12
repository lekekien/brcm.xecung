export class CustomerUpdateModel {
    constructor() { 
        this.Scource = -1;
        this.Type = -1;
        this.Status = -1;
        this.AssigneeId = -1;
    }
    Id: number;
    CustomerCode: string;
    Name: string;
    PhoneNumber: string;
    Email: string;
    Scource: number;
    Type: number;
    Status: number;
    FacebookLink: string;
    Company: string;
    Position: string;
    Birthday: string;
    BirthdayConvert: string;
    AssigneeId: number;
    AssigneeName: number;
    Description: string;
    BlockStatus: number;
    IsFavorite: number;
    CreatedBy: string;
    CreatedDate: string;
    Token: string;
}
