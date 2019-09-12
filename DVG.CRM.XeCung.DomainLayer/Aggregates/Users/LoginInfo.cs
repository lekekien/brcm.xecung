using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Users
{
    public class LoginInfo
    {
        #region constructor
        internal LoginInfo(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }
        #endregion constructor

        #region properties
        public string UserName { get; private set; }
        public string Password { get; private set; }
        #endregion

        #region behaviors

        #endregion
    }
}
