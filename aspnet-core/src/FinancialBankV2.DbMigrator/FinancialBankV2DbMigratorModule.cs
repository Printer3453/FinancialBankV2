using FinancialBankV2.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace FinancialBankV2.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(FinancialBankV2EntityFrameworkCoreModule),
    typeof(FinancialBankV2ApplicationContractsModule)
    )]
public class FinancialBankV2DbMigratorModule : AbpModule
{
}
