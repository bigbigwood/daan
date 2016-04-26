using daan.webservice.phyReportSystem.Framework.Authenticaition;
using log4net;
using daan.webservice.phyReportSystem.Contract.Models;

namespace daan.webservice.phyReportSystem.AuthenticaitionImpl
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