using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace FinancialBankV2
{
    public interface IBankAccountAppService : IApplicationService
    {
        Task<List<BankAccountDto>> GetMyAccountsAsync();

        Task CreateTransactionAsync(CreateTransactionDto input);
    }
}