using Xunit;

namespace FinancialBankV2.EntityFrameworkCore;

[CollectionDefinition(FinancialBankV2TestConsts.CollectionDefinitionName)]
public class FinancialBankV2EntityFrameworkCoreCollection : ICollectionFixture<FinancialBankV2EntityFrameworkCoreFixture>
{

}
