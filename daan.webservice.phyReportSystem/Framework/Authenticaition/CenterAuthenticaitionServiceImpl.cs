using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Daan.Authority.Handler;
using log4net;

namespace daan.webservice.phyReportSystem.Framework.Authenticaition
{
    public class CenterAuthenticaitionServiceImpl : IAuthenticaitionService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AuthenticaitionResultCode Authenticate(UserCredential userCredential)
        {
            AuthenticaitionResultCode result = AuthenticaitionResultCode.Error; 
            try
            {
                SecurityHandler security = SecurityHandler.Login(userCredential.Username, userCredential.Password);
                if (security.LoginResult.SystemCode != null)
                {
                    Log.Info("Authenticate OK");

                    // Do something, for example load user data in the context
                    result = AuthenticaitionResultCode.OK;
                }
                else
                {
                    Log.Info("Authenticate fail.");
                    result = AuthenticaitionResultCode.PasswordIsIncorrect;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Authenticaition exception.", ex);
            }

            return result;
        }
    }
}