using System.Threading.Tasks;

namespace FinancialBankV2.Data;

public interface IFinancialBankV2DbSchemaMigrator
{
    Task MigrateAsync();
}
