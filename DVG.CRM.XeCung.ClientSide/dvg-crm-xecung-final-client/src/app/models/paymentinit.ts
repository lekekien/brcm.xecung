import { UserSearchResult } from './usersearchresult';

export class PaymentInit {
    constructor() {
        this.AllState = [];
        this.AllCity = [];
        this.AllCustomerType = [];
        this.AllListing = [];
        this.AllState = [];
        this.AllAssignee = [];
    }
    AllState: { [key: number ]: string};
    AllCity: { [key: number ]: string};
    AllCustomerType: { [key: number ]: string};
    AllListing: { [key: number ]: string};
    AllStatus: { [key: number ]: string};
    AllAssignee: UserSearchResult[];
}
