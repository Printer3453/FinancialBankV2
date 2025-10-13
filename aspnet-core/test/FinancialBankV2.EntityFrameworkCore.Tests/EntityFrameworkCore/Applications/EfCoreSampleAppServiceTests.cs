using FinancialBankV2.Samples;
using Xunit;

namespace FinancialBankV2.EntityFrameworkCore.Applications;

[Collection(FinancialBankV2TestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<FinancialBankV2EntityFrameworkCoreTestModule>
{

}
