using FinancialBankV2.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace FinancialBankV2.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class FinancialBankV2Controller : AbpControllerBase
{
    protected FinancialBankV2Controller()
    {
        LocalizationResource = typeof(FinancialBankV2Resource);
    }
}
