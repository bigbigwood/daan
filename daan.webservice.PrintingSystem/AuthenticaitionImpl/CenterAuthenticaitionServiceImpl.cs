using System;
using System.Configuration;
using Daan.Authority.Handler.AuthorityServiceReference;
using daan.webservice.PrintingSystem.Framework.Authenticaition;
using log4net;

namespace daan.webservice.PrintingSystem.AuthenticaitionImpl
{
    public class CenterAuthenticaitionServiceImpl : IAuthenticaitionService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AuthenticaitionResultCode Authenticate(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace((username)) || string.IsNullOrWhiteSpace((password)))
                    return AuthenticaitionResultCode.UserOrPasswordIsEmpty;

                var service = new AuthorizationSoapClient();
                string systemCode = ConfigurationManager.AppSettings.Get("AuthorizationSystemCode");
                var securiResult = service.Login(systemCode, username, password);
                if (securiResult.SystemCode != null)
                {
                    Log.Info("Authenticate OK");

                    // Do something, for example load user data in the context
                    return AuthenticaitionResultCode.Ok;
                }
                else
                {
                    Log.Info("Authenticate fail.");
                    return AuthenticaitionResultCode.UserOrPasswordIsIncorrect;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Authenticaition exception.", ex);
                return AuthenticaitionResultCode.Error;
            }
        }
    }
}