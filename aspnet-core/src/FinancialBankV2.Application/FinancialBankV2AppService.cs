using System;
using System.Collections.Generic;
using System.Text;
using FinancialBankV2.Localization;
using Volo.Abp.Application.Services;

namespace FinancialBankV2;

/* Inherit your application services from this class.
 */
public abstract class FinancialBankV2AppService : ApplicationService
{
    protected FinancialBankV2AppService()
    {
        LocalizationResource = typeof(FinancialBankV2Resource);
    }
}
