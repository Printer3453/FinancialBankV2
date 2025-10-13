using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace FinancialBankV2.Data;

/* This is used if database provider does't define
 * IFinancialBankV2DbSchemaMigrator implementation.
 */
public class NullFinancialBankV2DbSchemaMigrator : IFinancialBankV2DbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
