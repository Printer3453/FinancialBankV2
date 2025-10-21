using System;

namespace FinancialBankV2
{

    public class TransactionDetailDto
        {
        public string SenderAccountNumber { get; set; }
        public string ReceiverAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }


}