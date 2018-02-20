namespace Sitecore.Support.Hooks
{
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using Sitecore.Events.Hooks;
    using Sitecore.SecurityModel;
    using System;

    public class UpdateUserRoleItem : IHook
    {
        public void Initialize()
        {
            using (new SecurityDisabler())
            {
                var databaseName = "master";
                var itemPath = "/sitecore/system/Settings/Rules/Definitions/Elements/Security/User Role";
                var fieldName = "Type";
                
                var type = typeof(Sitecore.Support.Rules.Conditions.SecurityConditions.UserRoleCondition<>);
                var assemblyName = type.Assembly.GetName().Name;
                var fieldValue = $"Sitecore.Support.Rules.Conditions.SecurityConditions.UserRoleCondition, {assemblyName}";

                var database = Factory.GetDatabase(databaseName);
                var item = database.GetItem(itemPath);

                if (string.Equals(item[fieldName], fieldValue, StringComparison.Ordinal))
                {
                    // already installed
                    return;
                }

                Log.Audit($"Installing {assemblyName}", this);
                Log.Info($"Updating {item.Paths.FullPath} in the {databaseName} database", this);

                item.Editing.BeginEdit();
                item[fieldName] = fieldValue;
                item.Editing.EndEdit();
            }
        }
    }
}