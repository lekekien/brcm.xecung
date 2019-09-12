import { CityLocation } from './citylocation';

export class PaymentCreateInit {
    constructor() {
        this.AllCity = [];
    }
    AllRegion: { [key: number ]: string};
    AllState: { [key: number ]: string};
    AllStatus: { [key: number ]: string};
    AllCity: CityLocation[];
    AllCustomerType: { [key: number ]: string};
    AllGroup: { [key: number ]: string};
}
