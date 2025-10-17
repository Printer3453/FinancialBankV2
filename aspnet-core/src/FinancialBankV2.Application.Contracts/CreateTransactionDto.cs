namespace FinancialBankV2
{
    public class CreateTransactionDto
    {
        public string ReceiverAccountNumber { get; set; } = string.Empty; // Para gönderilecek hesap numarası
        public decimal Amount { get; set; } // Gönderilecek miktar
    }
}