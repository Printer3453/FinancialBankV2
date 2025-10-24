import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { BankAccountDto, AiAnswerDto, AskQuestionDto } from '../../proxy/models';
import { BankAccountService } from '../../proxy/bank-account.service';
import { AiService } from '../../proxy/ai.service';

@Component({
  selector: 'app-accounts-dashboard',
  templateUrl: './accounts-dashboard.component.html',
  styleUrls: ['./accounts-dashboard.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class AccountsDashboardComponent implements OnInit {
  accounts: BankAccountDto[] = [];
  userQuestion: string = '';
  aiAnswer: string | null = null;
  public isChatOpen = false;

  constructor(
    private bankAccountService: BankAccountService,
    private aiService: AiService 
  ) {}

  ngOnInit(): void {
    this.bankAccountService.getMyAccounts().subscribe(result => {
      this.accounts = result;
    });
  } 
toggleChat(): void {
    this.isChatOpen = !this.isChatOpen;
}

  askAi(): void {
    if (!this.userQuestion) return;

    const questionDto: AskQuestionDto = { question: this.userQuestion };
    this.aiAnswer = 'Düşünüyorum...';


    this.aiService.askQuestion(questionDto).subscribe(
      (result: AiAnswerDto) => {
        this.aiAnswer = result.answer;
      },
      error => {
        this.aiAnswer = error.error?.error?.message || 'Bir hata oluştu.';
      }
    );
  }
}