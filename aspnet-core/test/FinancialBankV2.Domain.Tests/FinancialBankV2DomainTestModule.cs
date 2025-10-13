using Volo.Abp.Modularity;

namespace FinancialBankV2;

[DependsOn(
    typeof(FinancialBankV2DomainModule),
    typeof(FinancialBankV2TestBaseModule)
)]
public class FinancialBankV2DomainTestModule : AbpModule
{

}
