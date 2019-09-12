import { KeyValueObject } from './key-value';
import { CityLocation } from './city-location';

export class LeadConvertedInit {
    constructor() {
        this.AllCity = [];
    }
    AllRegion: KeyValueObject[];
    AllCity: CityLocation[];
    AllCustomerType: KeyValueObject[];
    AllGroup: KeyValueObject[];
    ListOrganization: KeyValueObject[];
    ListPhone: string[];
}