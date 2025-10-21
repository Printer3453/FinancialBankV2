import type { AuditedEntityDto } from '@abp/ng.core';

export interface AiAnswerDto {
  answer?: string;
}

export interface AskQuestionDto {
  question?: string;
}

export interface BankAccountDto extends AuditedEntityDto<string> {
  userId?: string;
  accountNumber?: string;
  balance: number;
}

export interface CreateTransactionDto {
  receiverAccountNumber?: string;
  amount: number;
}

export interface TransactionDetailDto {
  senderAccountNumber?: string;
  receiverAccountNumber?: string;
  amount: number;
  transactionDate?: string;
}
