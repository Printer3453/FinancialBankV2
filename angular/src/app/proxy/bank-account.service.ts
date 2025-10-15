import type { BankAccountDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BankAccountService {
  apiName = 'Default';
  

  getMyAccounts = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, BankAccountDto[]>({
      method: 'GET',
      url: '/api/app/bank-account/my-accounts',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
