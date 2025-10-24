using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp.Uow;
using Volo.Abp.Application.Dtos;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace FinancialBankV2
{
    public class BankAccountAppService : ApplicationService, IBankAccountAppService
    {
        private readonly IRepository<BankAccount, Guid> _bankAccountRepository;
        private readonly IRepository<Transaction, Guid> _transactionRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly ICurrentUser _currentUser;


        public BankAccountAppService(
            IRepository<BankAccount, Guid> bankAccountRepository,
            IRepository<Transaction, Guid> transactionRepository,
            ICurrentUser currentUser,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _bankAccountRepository = bankAccountRepository;
            _transactionRepository = transactionRepository;
            _currentUser = currentUser;
            _unitOfWorkManager = unitOfWorkManager;
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



        public async Task<Guid> CreateTransactionAsync(CreateTransactionDto input)
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

            return transaction.Id;
        }

        public async Task<TransactionDetailDto> GetTransactionByIdAsync(Guid id)
        {

            using (var uow = _unitOfWorkManager.Begin())
            {
                // 1. Önce ID'ye göre işlem kaydını bul
                var transaction = await _transactionRepository.GetAsync(id);

                // 2. İşlem kaydındaki ID'leri kullanarak gönderen ve alıcı hesapları bul
                var senderAccount = await _bankAccountRepository.GetAsync(transaction.SenderAccountId);
                var receiverAccount = await _bankAccountRepository.GetAsync(transaction.ReceiverAccountId);

                await uow.CompleteAsync();
                // 3. Topladığımız tüm bilgileri DTO'ya doldur ve geri döndür
                return new TransactionDetailDto
                {
                    SenderAccountNumber = senderAccount.AccountNumber,
                    ReceiverAccountNumber = receiverAccount.AccountNumber,
                    Amount = transaction.Amount,
                    TransactionDate = transaction.TransactionDate
                };
            }
        }

        public async Task<PagedResultDto<TransactionHistoryDto>> GetTransactionHistoryAsync(PagedAndSortedResultRequestDto input)
        {
            var userAccount = await _bankAccountRepository.FirstOrDefaultAsync(x => x.UserId == _currentUser.Id.Value);
            if (userAccount == null)
            {
                return new PagedResultDto<TransactionHistoryDto>(0, new List<TransactionHistoryDto>());
            }

            // 1. Önce sorgulanabilir nesneleri al (await ile)
            var transactionQueryable = await _transactionRepository.GetQueryableAsync();
            var accountQueryable = await _bankAccountRepository.GetQueryableAsync();

            // 2. LINQ sorgusunu bu nesneler üzerinde kur (içinde await YOK)
            var query = from transaction in transactionQueryable
                        where transaction.SenderAccountId == userAccount.Id || transaction.ReceiverAccountId == userAccount.Id
                        join sender in accountQueryable on transaction.SenderAccountId equals sender.Id
                        join receiver in accountQueryable on transaction.ReceiverAccountId equals receiver.Id
                        // Sıralamayı veritabanında yapmak için orderby ekliyoruz
                        orderby transaction.CreationTime descending
                        select new TransactionHistoryDto
                        {
                            TransactionDate = transaction.TransactionDate,
                            Amount = transaction.Amount,
                            Type = transaction.SenderAccountId == userAccount.Id ? "Giden" : "Gelen",
                            Description = transaction.SenderAccountId == userAccount.Id
                                ? $"{receiver.AccountNumber} hesabına gönderildi"
                                : $"{sender.AccountNumber} hesabından geldi"
                        };

            // 3. Toplam sayıyı al (sayfalama için)
            var totalCount = await AsyncExecuter.CountAsync(query);

            // 4. Sorguyu sayfalara böl ve sonucu listeye çevir (await ile)
            var result = await AsyncExecuter.ToListAsync(
                query.PageBy(input)
            );

            return new PagedResultDto<TransactionHistoryDto>(totalCount, result);
        }
    }
}







