using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;
using Xunit;

// DİKKAT: Namespace'i 'EntityFrameworkCore' projesine uygun hale getirdik.
namespace FinancialBankV2.EntityFrameworkCore.Ai 
{
    // DİKKAT: Base Class artık 'FinancialBankV2EntityFrameworkCoreTestBase'.
    // Bu sınıf, veritabanını test için otomatik hazırlar.
    public class AiAppService_Tests : FinancialBankV2EntityFrameworkCoreTestBase
    {
        private readonly IAiAppService _aiAppService;
        private readonly IRepository<BankAccount, Guid> _bankAccountRepository;
        private readonly IRepository<Transaction, Guid> _transactionRepository;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public AiAppService_Tests()
        {
            // Servisleri kutudan çekiyoruz
            _aiAppService = GetRequiredService<IAiAppService>();
            _bankAccountRepository = GetRequiredService<IRepository<BankAccount, Guid>>();
            _transactionRepository = GetRequiredService<IRepository<Transaction, Guid>>();
            _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        }

        [Fact]
        public async Task Should_Classify_As_Balance_And_Reply()
        {
            // 1. ARRANGE (Hazırlık)
            var myUserId = Guid.NewGuid();

            // Hesabı oluştur (Constructor ile ID veriyoruz)
            await _bankAccountRepository.InsertAsync(new BankAccount(Guid.NewGuid())
            {
                UserId = myUserId,
                AccountNumber = "TR_TEST_1",
                Balance = 5000
            });

            // Sahte giriş yap
            using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.UserId, myUserId.ToString())))
            {
                // 2. ACT (Eylem)
                var input = new AskQuestionDto { Question = "How much money do I have?" };
                var result = await _aiAppService.AskQuestionAsync(input);

                // 3. ASSERT (Kontrol)
                result.ShouldNotBeNull();
                result.Answer.ShouldNotBeNullOrEmpty();
                // "5.000" veya "5000" gelebilir, sadece rakamları kontrol ediyoruz.
                result.Answer.ShouldContain("5"); 
                result.Answer.ShouldContain("000"); 
            }
        }

        [Fact]
        public async Task Should_Classify_As_Transaction_History()
        {
            // 1. ARRANGE
            var myUserId = Guid.NewGuid();
            var accountId = Guid.NewGuid();

            // Hesabı ekle
            await _bankAccountRepository.InsertAsync(new BankAccount(accountId)
            {
                UserId = myUserId,
                AccountNumber = "TR_TEST_HISTORY",
                Balance = 1000
            });

            // Geçmiş işlemleri ekle
            await _transactionRepository.InsertAsync(new Transaction
            {
                SenderAccountId = accountId, 
                ReceiverAccountId = Guid.NewGuid(),
                Amount = 150,
                TransactionDate = DateTime.Now.AddHours(-1)
            });

            await _transactionRepository.InsertAsync(new Transaction
            {
                SenderAccountId = Guid.NewGuid(), 
                ReceiverAccountId = accountId,
                Amount = 900,
                TransactionDate = DateTime.Now.AddHours(-2)
            });

            // Sahte giriş yap
            using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.UserId, myUserId.ToString())))
            {
                // 2. ACT
                var result = await _aiAppService.AskQuestionAsync(new AskQuestionDto { Question = "Son harcamalarım neler?" });

                // 3. ASSERT
                result.Answer.ShouldNotBeNullOrEmpty();
                result.Answer.ShouldContain("150");
                result.Answer.ShouldContain("900");
            }
        }
    }
}