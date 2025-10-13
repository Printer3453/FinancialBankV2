using FinancialBankV2.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace FinancialBankV2.Permissions;

public class FinancialBankV2PermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(FinancialBankV2Permissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(FinancialBankV2Permissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FinancialBankV2Resource>(name);
    }
}
