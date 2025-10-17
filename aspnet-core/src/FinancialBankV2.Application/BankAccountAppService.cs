using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace FinancialBankV2
{
    public class BankAccountAppService : ApplicationService, IBankAccountAppService
    {
        private readonly IRepository<BankAccount, Guid> _bankAccountRepository;
        private readonly IRepository<Transaction, Guid> _transactionRepository;

        private readonly ICurrentUser _currentUser;


        public BankAccountAppService(
            IRepository<BankAccount, Guid> bankAccountRepository,
            IRepository<Transaction, Guid> transactionRepository,
            ICurrentUser currentUser)
        {
            _bankAccountRepository = bankAccountRepository;
            _transactionRepository = transactionRepository;
            _currentUser = currentUser;
        }

        
        public async Task<List<BankAccountDto>> GetMyAccountsAsync()
        {
            
            if (!_currentUser.IsAuthenticated || !_currentUser.Id.HasValue)
            {
                return new List<BankAccountDto>();
            }

            var userId = _currentUser.Id.Value;
            var accounts = await _bankAccountRepository.GetListAsync(x => x.UserId == userId);
            return ObjectMapper.Map<List<BankAccount>, List<BankAccountDto>>(accounts);
        }
        

       
        public async Task CreateTransactionAsync(CreateTransactionDto input)
        {
            var senderAccount = await _bankAccountRepository.FirstOrDefaultAsync(x => x.UserId == CurrentUser.Id.Value);
            if (senderAccount == null) throw new UserFriendlyException("Adınıza kayıtlı bir banka hesabı bulunamadı!");

            var receiverAccount = await _bankAccountRepository.FirstOrDefaultAsync(x => x.AccountNumber == input.ReceiverAccountNumber);
            if (receiverAccount == null) throw new UserFriendlyException("Alıcı hesap numarası bulunamadı!");

            if (senderAccount.Balance < input.Amount) throw new UserFriendlyException("Yetersiz bakiye!");
            if (input.Amount <= 0) throw new UserFriendlyException("Transfer miktarı sıfırdan büyük olmalıdır!");

            senderAccount.Balance -= input.Amount;
            receiverAccount.Balance += input.Amount;

            // UpdateAsync komutlarını bırakıyoruz, çünkü bunlar değişikliği net bir şekilde belirtiyor
            await _bankAccountRepository.UpdateAsync(senderAccount);
            await _bankAccountRepository.UpdateAsync(receiverAccount);

            var transaction = new Transaction
            {
                SenderAccountId = senderAccount.Id,
                ReceiverAccountId = receiverAccount.Id,
                Amount = input.Amount,
                TransactionDate = DateTime.Now
            };

            await _transactionRepository.InsertAsync(transaction);

            // Metot bittiğinde ABP, tüm bu işlemleri tek bir pakette otomatik olarak veritabanına kaydedecek
        }
    }
}