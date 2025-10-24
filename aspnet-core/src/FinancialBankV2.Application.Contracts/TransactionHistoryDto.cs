using System;

namespace FinancialBankV2
{

    public class TransactionHistoryDto
    {
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}