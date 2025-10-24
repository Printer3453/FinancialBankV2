import type { BankAccountDto, CreateTransactionDto, TransactionDetailDto, TransactionHistoryDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BankAccountService {
  apiName = 'Default';
  

  createTransaction = (input: CreateTransactionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
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
  

  getTransactionById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TransactionDetailDto>({
      method: 'GET',
      url: `/api/app/bank-account/${id}/transaction-by-id`,
    },
    { apiName: this.apiName,...config });
  

  getTransactionHistory = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<TransactionHistoryDto>>({
      method: 'GET',
      url: '/api/app/bank-account/transaction-history',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
