import type { AuditedEntityDto } from '@abp/ng.core';

export interface BankAccountDto extends AuditedEntityDto<string> {
  userId?: string;
  accountNumber?: string;
  balance: number;
}

export interface CreateTransactionDto {
  receiverAccountNumber?: string;
  amount: number;
}
