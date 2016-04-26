using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace daan.webservice.phyReportSystem.Framework.Authenticaition
{
    public class MockAuthenticaitionServiceImpl
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AuthenticaitionResultCode Authenticate(UserCredential userCredential)
        {
            Log.Info("Authenticate OK");
            return AuthenticaitionResultCode.OK;
        }
    }
}