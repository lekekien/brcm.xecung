import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { CustomerIndex } from '../models/customerindex';
import { CustomerService } from '../services/customer.service';
import { ServerResponse } from '../models/response';

@Injectable()
export class CustomerResolve implements Resolve<any> {
    model: CustomerIndex;
    constructor(private dataservice: CustomerService) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.dataservice.search(this.model, (res: ServerResponse) => { });
    }
}