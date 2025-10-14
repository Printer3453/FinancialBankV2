using System;
using Volo.Abp.Application.Dtos;



namespace FinancialBankV2
{
    // BankAccountDto, BankAccount varlığının veri aktarım nesnesidir (DTO).
    // Bu sınıf, BankAccount varlığının istemciye veya diğer katmanlara
    // aktarılması için kullanılır.
    public class BankAccountDto : AuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; } // Bu hesabın sahibi olan kullanıcının ID'si
        public string AccountNumber { get; set; } = string.Empty;// Banka Hesap Numarası (IBAN gibi)
        public decimal Balance { get; set; } // Hesaptaki Bakiye

    }
}