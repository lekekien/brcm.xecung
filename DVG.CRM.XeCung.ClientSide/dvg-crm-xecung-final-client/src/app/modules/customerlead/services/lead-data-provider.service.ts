import { Injectable } from '@angular/core';
import { LeadIndex } from 'src/app/models/lead-index';

@Injectable()
export class LeadDataProviderService {
  leadIndex: LeadIndex;
  constructor() {
    this.leadIndex = new LeadIndex();
  }
}
