using FinancialBankV2.Samples;
using Xunit;

namespace FinancialBankV2.EntityFrameworkCore.Domains;

[Collection(FinancialBankV2TestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<FinancialBankV2EntityFrameworkCoreTestModule>
{

}
