import { Component, OnInit } from '@angular/core';
// Gerekli servisleri ve DTO'yu import ediyoruz
import { BankAccountDto } from '@proxy/models';
import { BankAccountService } from '@proxy/bank-account.service';
@Component({
  selector: 'app-accounts-dashboard',
  templateUrl: './accounts-dashboard.component.html',
  styleUrls: ['./accounts-dashboard.component.scss'],
})
export class AccountsDashboardComponent implements OnInit {
  // Gelen hesapları tutacağımız bir dizi oluşturuyoruz
  accounts: BankAccountDto[] = [];

  // Constructor'da proxy'den gelen servisimizi inject ediyoruz
  constructor(private bankAccountService: BankAccountService) {}

  // Component ilk yüklendiğinde bu metot çalışır
  ngOnInit(): void {
    this.bankAccountService.getMyAccounts().subscribe(result => {
      this.accounts = result;
    });
  }
}