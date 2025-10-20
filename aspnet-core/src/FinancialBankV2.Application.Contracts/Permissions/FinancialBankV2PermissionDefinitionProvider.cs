using FinancialBankV2.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;


namespace FinancialBankV2.Permissions
{
    public class FinancialBankV2PermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(FinancialBankV2Permissions.GroupName);

            
            myGroup.AddPermission(FinancialBankV2Permissions.Ai.Default, L("Permission:AiService"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FinancialBankV2Resource>(name);
        }
    }
}