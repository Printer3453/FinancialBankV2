import { Component } from '@angular/core';
// Gerekli servis ve DTO'ları import ediyoruz (daha önce yaptığımız gibi direkt yoldan)
import { BankAccountService } from '../../proxy/bank-account.service';
import { CreateTransactionDto } from '../../proxy/models';
import { FormsModule } from '@angular/forms';

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

  // Backend servisimizi constructor'da inject ediyoruz
  constructor(private bankAccountService: BankAccountService) {}

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
      () => {
        // Başarılı olursa...
        this.isSuccess = true;
        console.log('Transfer başarılı!');
        // Formu temizleyebiliriz
        this.receiverAccountNumber = '';
        this.amount = null;
      },
      errorResponse => {
        // Hata olursa...
        this.error = errorResponse.error?.error?.message || 'Bilinmeyen bir hata oluştu.';
        console.error('Transfer hatası:', errorResponse);
      }
    );
  }
}