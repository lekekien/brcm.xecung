using System.Collections.Specialized;
using System.Configuration;

namespace DVG.CRM.XeCung.InfrastructureLayer.Utility.Configs
{
    public class AvatarConfigs : NameValueCollection
    {
        private static NameValueCollection avatarConfigs = ConfigurationManager.GetSection("AvatarConfigs") as NameValueCollection;

        public static string Value(string name, string defaultValue = "")
        {
            return avatarConfigs != null && avatarConfigs[name] != null ? avatarConfigs[name] : defaultValue;
        }
    }
}