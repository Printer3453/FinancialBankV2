// Gerekli import'ları en üste ekliyoruz
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BankAccountService } from '../../proxy/bank-account.service';
import { CreateTransactionDto, BankAccountDto } from '../../proxy/models';

@Component({
  selector: 'app-money-transfer',
  templateUrl: './money-transfer.component.html',
  styleUrls: ['./money-transfer.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class MoneyTransferComponent implements OnInit {
  // 1. @ViewChild ile HTML'deki #transferForm referansını yakalıyoruz
  @ViewChild('transferForm') transferForm!: NgForm;

  receiverAccountNumber: string = '';
  amount: number | null = null;
  
  senderAccount: BankAccountDto | null = null;
  isLoading: boolean = false;

  constructor(
    private bankAccountService: BankAccountService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.bankAccountService.getMyAccounts().subscribe(accounts => {
      if (accounts && accounts.length > 0) {
        this.senderAccount = accounts[0];
      }
    });
  }

  submitForm(): void {
    // 2. Artık 'this.transferForm' burada tanınıyor olacak
    if (!this.transferForm.valid || this.isLoading) return;
    
    this.isLoading = true;

    const transactionDto: CreateTransactionDto = {
      receiverAccountNumber: this.receiverAccountNumber,
      amount: this.amount!, // amount'ın null olmayacağından emin olduğumuz için '!' ekleyebiliriz
    };

    this.bankAccountService.createTransaction(transactionDto).subscribe({
      next: (transactionId: string) => {
        this.toastr.success('Para transferi dekont sayfasına yönlendiriliyor...', 'Başarılı!');
        this.isLoading = false;
        
        this.bankAccountService.getMyAccounts().subscribe();

        setTimeout(() => {
          this.router.navigate(['/receipt', transactionId.replace(/"/g, '')]);
        }, 1500);
      },
      error: errorResponse => {
        const errorMessage = errorResponse.error?.error?.message || 'Bilinmeyen bir hata oluştu.';
        this.toastr.error(errorMessage, 'Hata!');
        this.isLoading = false;
      }
    });
  }

}