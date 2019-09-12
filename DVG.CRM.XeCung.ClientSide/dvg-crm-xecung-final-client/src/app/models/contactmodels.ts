export class Contactmodels {
    Id: number;
    Name: string;
    OrganizationType: number;
    OrganizationName: string;
    TaxCode: string;
    PhoneNumber: string;
    Email: string;
    CityRegion: number;
    CitiRegionName: string;
    Address: string;
    Note: string;
    Token: string;
    ListContactNote: ContactNote[];
}
export class ContactNote {
    Id: number;
    NoteCreatedDate: string;
    Note: string;
    UserId: number;
    UserName: string;
    ContactId: number;
}