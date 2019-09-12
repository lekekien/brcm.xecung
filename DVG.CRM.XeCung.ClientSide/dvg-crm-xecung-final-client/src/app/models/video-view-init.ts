import { KeyValueObject } from './key-value';

export class VideoViewInit {
    constructor() {       
    }
    AllVideoType: KeyValueObject[]; //lưu loại video (Theo HD, ko theo HD, tự SX)
    AllExpenditureInKeyValue: KeyValueObject[]; //lưu loại chi phí của chi phí sản xuất
    AllCostContent: KeyValueObject[]; //lưu nội dung chi của chi phí sản xuất
}