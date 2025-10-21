import { Component } from '@angular/core';
// Gerekli servis ve DTO'ları import ediyoruz 
import { BankAccountService } from '../../proxy/bank-account.service';
import { CreateTransactionDto } from '../../proxy/models';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-money-transfer',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './money-transfer.component.html',
  styleUrls: ['./money-transfer.component.scss'],
})
export class MoneyTransferComponent {
  // HTML'deki form alanlarına bağlanacak değişkenler
  receiverAccountNumber: string = '';
  amount: number | null = null;

  // Başarı ve Hata durumlarını kontrol edecek değişkenler
  isSuccess: boolean = false;
  error: string | null = null;

  // 1. Backend servisimizi constructor'da inject ediyoruz
  // 2. Constructor'a Router'ı enjekte ediyoruz
  constructor(
    private bankAccountService: BankAccountService,
    private router: Router
  ) { }

  // Form gönderildiğinde bu metot çalışacak
  submitForm(): void {
    // Her denemede eski mesajları temizle
    this.isSuccess = false;
    this.error = null;

    if (!this.receiverAccountNumber || !this.amount || this.amount <= 0) {
      this.error = 'Lütfen tüm alanları doğru bir şekilde doldurun.';
      return;
    }

    // DTO'yu formdaki verilerle oluştur
    const transactionDto: CreateTransactionDto = {
      receiverAccountNumber: this.receiverAccountNumber,
      amount: this.amount,
    };

    // Backend'deki CreateTransactionAsync metodunu çağır
    this.bankAccountService.createTransaction(transactionDto).subscribe(
      (transactionId: string) => {
        // Başarılı olursa tek yapmamız gereken,
        // kullanıcıyı işlem ID'si ile birlikte dekont sayfasına yönlendirmek.
        console.log('Transfer başarılı! İşlem ID:', transactionId);
        this.router.navigate(['/receipt', transactionId.replace(/"/g, '')]);
      },
      errorResponse => {
        // Hata olursa, BU SAYFADA kalıp hatayı gösteriyoruz.
        this.isSuccess = false; // Başarı durumunu sıfırla
        this.error = errorResponse.error?.error?.message || 'Bilinmeyen bir hata oluştu.';
        console.error('Transfer hatası:', errorResponse);
      }
    );
  }

}