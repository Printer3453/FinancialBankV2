using Volo.Abp.Modularity;

namespace FinancialBankV2;

[DependsOn(
    typeof(FinancialBankV2ApplicationModule),
    typeof(FinancialBankV2DomainTestModule)
)]
public class FinancialBankV2ApplicationTestModule : AbpModule
{

}
