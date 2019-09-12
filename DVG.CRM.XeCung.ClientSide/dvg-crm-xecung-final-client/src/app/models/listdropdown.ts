import { UserSearchResult } from './usersearchresult';
import { CompileEntryComponentMetadata } from '@angular/compiler';
import { CityLocation } from './city-location';
import { KeyValueObject } from './key-value';

export class ListDropDown {
    Key: number;
    Value: string;
}
export class DropDownDic {
    ListOrganization: { [key: number]: string };
    AllCity: { [key: number]: string };
    ListRole: ListDropDown[];
}
export class ListLocation {
    constructor() {
        this.AllCityRegion = [];
        this.AllCity = [];
        this.AllRegion = [];
        this.AllCityModel = [];
    }
    AllCityRegion: KeyValueObject[];
    AllCity: KeyValueObject[];
    AllRegion: KeyValueObject[];
    AllCityModel: CityLocation[];
}
export class CustomerLeadDropDown {
    constructor() {
        this.AllListing = [];
        this.AllAssignee = [];
        this.AllKeyValueAssignee = [];
        this.AllStatus = [];
        this.AllCustomerType = [];
        this.ListPhone = [];
    }
    AllListing: { [key: number]: string };
    AllAssignee: UserSearchResult[];
    AllKeyValueAssignee: { [key: number]: string };
    AllStatus: ListDropDown[];
    AllCustomerType: { [key: number]: string };
    ListPhone: [];
}
