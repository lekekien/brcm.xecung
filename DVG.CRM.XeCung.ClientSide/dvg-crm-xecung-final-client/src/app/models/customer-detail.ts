import { CustomerActivityHistory } from './customer-activity-history';
import { CustomerPostingLoginModel } from './customer-posting-login-model';

export class CustomerDetail {
    constructor() {
     
    }
    Id: number;
    CustomerCode: string
    Name: string
    PhoneNumber: string
    Email: string
    Scource: number
    ScourceName: string;
    Type: number
    TypeName: string;
    Status: number
    StatusName: string;
    FacebookLink: string
    Company: string
    Position: string
    Birthday: string
    AssigneeId: number
    AssigneeName: string
    Description: string
    BlockStatus: number
    BlockStatusName: string;
    IsFavorite: string
    CreatedBy: string
    CreatedDate: string
    LastModifiedBy: string
    LastModifiedDate: string
}
