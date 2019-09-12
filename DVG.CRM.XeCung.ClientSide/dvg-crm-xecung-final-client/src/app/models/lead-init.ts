import { KeyValueObject } from './key-value';
import { UserSearchResult } from './usersearchresult';

export class LeadInit {
    constructor() {
        this.AllListing = [];
        this.AllAssignee = [];
        this.AllKeyValueAssignee = [];
        this.AllReasonBlock = [];
    }
    AllListing: KeyValueObject[];
    AllAssignee: UserSearchResult[];
    AllKeyValueAssignee: KeyValueObject[];
    AllReasonBlock: KeyValueObject[];
}
