import { CityLocation } from './city-location';
import { KeyValueObject } from './key-value';

export class CustomerUpdateInit {
    constructor() {
        this.AllCustomerScource = [];
        this.AllCustomerStatus = [];
        this.AllCustomerType = []; 
    }
    AllCustomerScource: KeyValueObject[];
    AllCustomerType: KeyValueObject[];
    AllCustomerStatus: KeyValueObject[];
}
