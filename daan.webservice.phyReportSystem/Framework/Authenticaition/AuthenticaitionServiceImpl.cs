using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Daan.Authority.Handler;
using log4net;

namespace daan.webservice.phyReportSystem.Framework.Authenticaition
{
    public class AuthenticaitionServiceImpl
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool Authenticate(UserCredential userCredential)
        {
            SecurityHandler security = SecurityHandler.Login(userCredential.Username, userCredential.Password);
            if (security.LoginResult.SystemCode != null)
            {
                Log.Info("Authenticate OK");

                // Do something, for example load user data in the context
                return true;
            }
            else
            {
                Log.Info("Authenticate fail.");
                return false;
            }
        }
    }
}