import { ContactAssign } from './contact-assign';
import { Paging } from './pagingmodel';

export class ContactFilter extends Paging {
    FilterKeyword: string;
    LstData: ContactAssign[];
    constructor() {
        super();
        this.LstData = [];
    }
}
