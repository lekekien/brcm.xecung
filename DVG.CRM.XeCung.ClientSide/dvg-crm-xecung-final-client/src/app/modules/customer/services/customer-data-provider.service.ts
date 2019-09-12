import { Injectable } from '@angular/core';
import { CustomerSearch } from 'src/app/models/customersearch';
import { CustomerIndex } from 'src/app/models/customerindex';

@Injectable()
export class CustomerDataProviderService {
  customerIndex: CustomerIndex;
  lstCustomer: CustomerSearch[];
  totalCustomer: number;
  constructor() {
    this.customerIndex = new CustomerIndex();
   }
}
