import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BankAccountService } from '../../proxy/bank-account.service';
import { TransactionHistoryDto } from '../../proxy/models';

@Component({
  selector: 'app-transaction-history',
  templateUrl: './transaction-history.component.html',
  styleUrls: ['./transaction-history.component.scss'],
  standalone: true,
  imports: [CommonModule],
  providers: [ListService], // Sayfalama için ABP'nin hazır servisi
})
export class TransactionHistoryComponent implements OnInit {
  // Gelen işlem listesini ve toplam sayısını tutacak nesne
  transactions = { items: [], totalCount: 0 } as PagedResultDto<TransactionHistoryDto>;

  constructor(
    public readonly list: ListService,
    private bankAccountService: BankAccountService
  ) {}

  ngOnInit(): void {
    // Sayfa ilk yüklendiğinde veya sayfa numarası değiştiğinde çalışacak olan kod
    const transactionStreamCreator = (query) => this.bankAccountService.getTransactionHistory(query);

    this.list.hookToQuery(transactionStreamCreator).subscribe((response) => {
      this.transactions = response;
    });
  }
}