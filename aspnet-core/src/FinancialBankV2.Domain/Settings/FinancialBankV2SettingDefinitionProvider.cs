using Volo.Abp.Settings;

namespace FinancialBankV2.Settings;

public class FinancialBankV2SettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(FinancialBankV2Settings.MySetting1));
    }
}
