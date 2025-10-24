using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace FinancialBankV2
{
    public interface IBankAccountAppService : IApplicationService
    {
        Task<List<BankAccountDto>> GetMyAccountsAsync();

        Task<Guid> CreateTransactionAsync(CreateTransactionDto input);
        Task<TransactionDetailDto> GetTransactionByIdAsync(Guid id);
        Task<PagedResultDto<TransactionHistoryDto>> GetTransactionHistoryAsync(PagedAndSortedResultRequestDto input);

    }
}