using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace FinancialBankV2
{
    public interface IAiAppService : IApplicationService
    {
        Task<AiAnswerDto> AskQuestionAsync(AskQuestionDto input);
    }
}