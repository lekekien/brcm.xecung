export class LeadBlock {
    constructor() {
        this.TypeIsInvalid = false;
        this.NoteIsInvalid = false;
    }
    Id: number;
    LeadId: number;
    BlockReasonDescription: string;
    BlockReasonType: number;
    Token: string;
    TypeIsInvalid: boolean;
    NoteIsInvalid: boolean;
    BlockStatus: number;
    ReasonType: number;
}

