import { UserSearchResult } from './usersearchresult';
import { KeyValueObject } from './key-value';
import { CityLocation } from './city-location';

export class CustomerInit {
    constructor() {
        this.AllCustomerScource = [];
        this.AllCustomerStatus = [];
        this.AllCustomerType = [];
    }
    AllCustomerScource: KeyValueObject[];
    AllCustomerStatus: KeyValueObject[];
    AllCustomerType: KeyValueObject[];
}
