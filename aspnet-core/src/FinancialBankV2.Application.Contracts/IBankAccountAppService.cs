using System;
using Volo.Abp.Application.Services;
using System.Threading.Tasks;
using System.Collections.Generic;




namespace FinancialBankV2
{
    // IBankAccountAppService, BankAccount varlığı için uygulama servis arayüzüdür.
    // Bu arayüz, BankAccount ile ilgili işlemleri tanımlar.
    public interface IBankAccountAppService : IApplicationService
    {
        Task<List<BankAccountDto>> GetMyAccountsAsync();

    }
}