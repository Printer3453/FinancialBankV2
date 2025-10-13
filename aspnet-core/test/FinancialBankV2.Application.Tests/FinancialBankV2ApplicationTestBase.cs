using Volo.Abp.Modularity;

namespace FinancialBankV2;

public abstract class FinancialBankV2ApplicationTestBase<TStartupModule> : FinancialBankV2TestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
