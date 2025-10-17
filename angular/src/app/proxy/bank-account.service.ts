import type { BankAccountDto, CreateTransactionDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BankAccountService {
  apiName = 'Default';
  

  createTransaction = (input: CreateTransactionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/bank-account/transaction',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  getMyAccounts = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, BankAccountDto[]>({
      method: 'GET',
      url: '/api/app/bank-account/my-accounts',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
