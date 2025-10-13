using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FinancialBankV2.Data;
using Volo.Abp.DependencyInjection;

namespace FinancialBankV2.EntityFrameworkCore;

public class EntityFrameworkCoreFinancialBankV2DbSchemaMigrator
    : IFinancialBankV2DbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreFinancialBankV2DbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the FinancialBankV2DbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<FinancialBankV2DbContext>()
            .Database
            .MigrateAsync();
    }
}
