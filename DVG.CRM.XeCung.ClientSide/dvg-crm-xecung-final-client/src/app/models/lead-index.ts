export class LeadIndex {
    constructor() {
        this.ListingRange = -1;
        this.Status = 1;
        this.AssigneeId = 0;
    }
    FilterKeyword: string;
    ListingRange: number;
    AssigneeId: number;
    // DateOfCreationFrom: string;
    // DateOfCreationTo: string;
    CreatedFrom: string;
    CreatedTo: string;
    Status: number;
    PageIndex: number;
    PageSize: number;
}
