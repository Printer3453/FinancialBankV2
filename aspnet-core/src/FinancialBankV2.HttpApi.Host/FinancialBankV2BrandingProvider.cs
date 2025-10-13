using Microsoft.Extensions.Localization;
using FinancialBankV2.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace FinancialBankV2;

[Dependency(ReplaceServices = true)]
public class FinancialBankV2BrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<FinancialBankV2Resource> _localizer;

    public FinancialBankV2BrandingProvider(IStringLocalizer<FinancialBankV2Resource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
