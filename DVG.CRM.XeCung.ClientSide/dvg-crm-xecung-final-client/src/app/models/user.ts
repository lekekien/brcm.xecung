import { Role } from 'src/app/enums/roles';

export class User {
    Id: number;
    UserName: string;
    token?: string;
    Roles: Role[];
}
