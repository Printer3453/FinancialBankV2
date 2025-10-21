import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BankAccountService } from '../../proxy/bank-account.service';
import { TransactionDetailDto } from '../../proxy/models';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-receipt',
  templateUrl: './receipt.component.html',
  styleUrls: ['./receipt.component.scss'],
  standalone: true,
  imports: [CommonModule]
})
export class ReceiptComponent implements OnInit {
  transaction: TransactionDetailDto | null = null;

  constructor(
    private route: ActivatedRoute,
    private bankAccountService: BankAccountService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.bankAccountService.getTransactionById(id).subscribe(result => {
        this.transaction = result;
      });
    }
  }
}