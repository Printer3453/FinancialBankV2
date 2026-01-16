using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace FinancialBankV2
{
    // FullAuditedAggregateRoot, bu sınıf için "Oluşturma, Güncelleme, Silme"
    // gibi tüm denetim alanlarını (kim, ne zaman yaptı) otomatik ekler.
    public class BankAccount : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; set; } // Bu hesabın sahibi olan kullanıcının ID'si
        public string AccountNumber { get; set; } = string.Empty;// Banka Hesap Numarası (IBAN gibi)
        public decimal Balance { get; set; } // Hesaptaki Bakiye

        // Boş constructor (EF Core için lazım)
    protected BankAccount() { }

    // ID alan constructor (Test için lazım)
    public BankAccount(Guid id) : base(id)
    {
    }
    }
}