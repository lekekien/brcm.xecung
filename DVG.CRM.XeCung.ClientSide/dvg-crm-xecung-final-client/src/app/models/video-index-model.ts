import { Paging } from './pagingmodel';

export class VideoIndexModel extends Paging {
    constructor() {
        super();
        this.VideoType = 0;
        this.SpendDate = null;
    }
    FilterKeyWord: string;
    VideoType: number;
    SpendDate: string;
}
