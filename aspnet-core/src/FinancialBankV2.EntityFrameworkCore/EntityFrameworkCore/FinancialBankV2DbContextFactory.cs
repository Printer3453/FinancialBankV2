using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FinancialBankV2.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class FinancialBankV2DbContextFactory : IDesignTimeDbContextFactory<FinancialBankV2DbContext>
{
    public FinancialBankV2DbContext CreateDbContext(string[] args)
    {
        FinancialBankV2EfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<FinancialBankV2DbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new FinancialBankV2DbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FinancialBankV2.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
