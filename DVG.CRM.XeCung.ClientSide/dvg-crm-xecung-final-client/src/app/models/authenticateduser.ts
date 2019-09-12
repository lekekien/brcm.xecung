import { Role } from 'src/app/enums/roles';

export class AuthenticatedUser {
    Id: number;
    UserName: string;
    Token?: string;
    Roles: Role[];
}
