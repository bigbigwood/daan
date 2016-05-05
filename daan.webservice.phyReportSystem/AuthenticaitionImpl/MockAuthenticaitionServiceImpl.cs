using daan.webservice.PrintingSystem.Framework.Authenticaition;
using log4net;

namespace daan.webservice.PrintingSystem.AuthenticaitionImpl
{
    public class MockAuthenticaitionServiceImpl
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AuthenticaitionResultCode Authenticate(string username, string password)
        {
            Log.Info("Authenticate OK");
            return AuthenticaitionResultCode.Ok;
        }
    }
}