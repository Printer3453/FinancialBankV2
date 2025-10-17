import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // <-- 1. BURAYI EKLE
import { BankAccountDto } from '../../proxy/models';
import { BankAccountService } from '../../proxy/bank-account.service';

@Component({
  selector: 'app-accounts-dashboard',
  templateUrl: './accounts-dashboard.component.html',
  styleUrls: ['./accounts-dashboard.component.scss'],
  standalone: true,
  imports: [CommonModule], // <-- 2. VE BURAYI EKLE
})
export class AccountsDashboardComponent implements OnInit {
  accounts: BankAccountDto[] = [];

  constructor(private bankAccountService: BankAccountService) {}

  ngOnInit(): void {
    this.bankAccountService.getMyAccounts().subscribe(result => {
      this.accounts = result;
    });
  }
}