import { PaymentNote } from './paymentnote';

export class PaymentEditInit {
    constructor() {
    }
    AllAdvertiseType: { [key: number ]: string};
    AllPackageType: { [key: number ]: string};
    AllPaymentNote: PaymentNote[];
}
