using Volo.Abp.Modularity;

namespace FinancialBankV2;

/* Inherit from this class for your domain layer tests. */
public abstract class FinancialBankV2DomainTestBase<TStartupModule> : FinancialBankV2TestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
