using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace FinancialBankV2
{
    public class BankAccountAppService : ApplicationService, IBankAccountAppService
    {
        private readonly IRepository<BankAccount, Guid> _bankAccountRepository;

        public BankAccountAppService(IRepository<BankAccount, Guid> bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }



        public async Task<List<BankAccountDto>> GetMyAccountsAsync()
        {
            // Mevcut giriş yapmış kullanıcının ID'sini al
            var userId = CurrentUser.Id.Value;

            // Veritabanından sadece bu kullanıcıya ait hesapları çek
            var accounts = await _bankAccountRepository.GetListAsync(x => x.UserId == userId);

            // Veritabanından gelen BankAccount listesini, dışarıya göndereceğimiz BankAccountDto listesine dönüştür
            return ObjectMapper.Map<List<BankAccount>, List<BankAccountDto>>(accounts);
        }
    }
}