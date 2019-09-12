using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.InfrastructureLayer.Utility
{
    public class Security
    {
        private const string KeyFormat = "dvs.it.department.{0:yyyyMMdd}";

        /// <summary>
        /// Create Security Key
        /// </summary>
        /// <returns></returns>
        public static string CreateKey()
        {
            var key = string.Format(KeyFormat, DateTime.Now).ToMD5();
            return key;
        }

        /// <summary>
        /// Check Security Key
        /// </summary>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static bool IsSecretKey(string secretKey)
        {
            var key = string.Format(KeyFormat, DateTime.Now).ToMD5();
            if (key.Equals(secretKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
