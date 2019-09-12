import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { CustomerService } from '../services/customer.service';
import { Injectable } from '@angular/core';

@Injectable()
export class CustomerUpdateResolve implements Resolve<any> {
    constructor(
        private customerService: CustomerService
    ) { }
    resolve(route: ActivatedRouteSnapshot) {
        const cusomerId = route.params.id;
        if (cusomerId > 0) {
            return this.customerService.getbyidresolve(cusomerId);
        } else {
            return null;
        }
    }
}
