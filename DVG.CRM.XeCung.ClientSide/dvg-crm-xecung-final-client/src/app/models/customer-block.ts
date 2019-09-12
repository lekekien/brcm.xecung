export class CustomerBlock {
    constructor() {
        this.TypeIsInvalid = false;
        this.NoteIsInvalid = false;
    }
    Id: number;
    CustomerId: number;
    BlockReasonDescription: string;
    BlockReasonType: number;
    Token: string;
    TypeIsInvalid: boolean;
    NoteIsInvalid: boolean;
    BlockStatus: number;
    ReasonType: number;
}
