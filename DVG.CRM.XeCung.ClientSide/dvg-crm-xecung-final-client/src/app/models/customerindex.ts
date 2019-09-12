import { Paging } from './pagingmodel';

export class CustomerIndex extends Paging{
    constructor() {
        super();
        this.FilterKeyword  = '';
        this.Scource = 0;
        this.Type = 0;
        this.Status =0;
        this.AssigneeId = 0;
        this.IsFavorite = -1;
        this.Order = 0;
        this.CreatedDateFrom = '';
        this.CreatedDateTo = '';
    }
        FilterKeyword: string;
        Scource: number;
        Type: number;
        Status: number;
        AssigneeId: number;
        IsFavorite: number;
        Order: number;
        CreatedDateFrom: string;
        CreatedDateTo: string;
}
