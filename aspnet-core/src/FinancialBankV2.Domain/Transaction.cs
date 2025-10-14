using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace FinancialBankV2
{
    // FullAuditedEntity, silme dışındaki denetim alanlarını ekler.
    public class Transaction : FullAuditedEntity<Guid>
    {
        public Guid SenderAccountId { get; set; } // Parayı Gönderen Hesabın ID'si
        public Guid ReceiverAccountId { get; set; } // Parayı Alan Hesabın ID'si
        public decimal Amount { get; set; } // Transfer Edilen Miktar
        public DateTime TransactionDate { get; set; } // İşlem Tarihi
    }
}