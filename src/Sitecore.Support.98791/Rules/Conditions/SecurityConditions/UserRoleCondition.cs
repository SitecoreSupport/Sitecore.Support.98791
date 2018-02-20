namespace Sitecore.Support.Rules.Conditions.SecurityConditions
{
    using Sitecore;
    using Sitecore.Diagnostics;
    using Sitecore.Rules;
    using Sitecore.Rules.Conditions;
    using Sitecore.Security.Accounts;
    using System.Linq;

    public class UserRoleCondition<T> : WhenCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            string str = this.Value;
            if (str != null)
            {
                foreach (string str2 in str.Split(new char[] { '|' }))
                {
                    Role role = Role.FromName(str2);

                    #region Modified code
                    if (RolesInRolesManager.GetRolesForUser(Context.User, true).Contains<Role>(role))
                    {
                        return true;
                    }
                    #endregion
                }
            }
            return false;
        }

        public string Value { get; set; }
    }
}