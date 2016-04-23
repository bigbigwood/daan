using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using daan.webservice.phyReportSystem.Contract;
using daan.webservice.phyReportSystem.Messages;
using log4net;
using daan.webservice.phyReportSystem.Framework.Authenticaition;
using daan.webservice.phyReportSystem.Framework;

namespace daan.webservice.phyReportSystem
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class PhyReportService : IPhyReportService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public QueryPhyReportResponse QueryPhyReport(QueryPhyReportRequest request)
        {
            Log.Info("reqeust received");
            var result = new QueryPhyReportResponse();

            try
            {
                // authentication
                Log.Info("Check user credential.");
                var userCredential = GetUserPassword();
                bool passAuthenticaition = new MockAuthenticaitionServiceImpl().Authenticate(userCredential);
                if (!passAuthenticaition)
                {
                    return new QueryPhyReportResponse() { ResultType = ResultTypes.AuthenticationError };
                }

                // business

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return result;
        }

        private UserCredential GetUserPassword()
        {
            var userCredential = new UserCredential();

            int userCodeHeaderIndex = OperationContext.Current.IncomingMessageHeaders.FindHeader("Username", Declarations.NameSpace);
            if (userCodeHeaderIndex >= 0)
            {
                userCredential.Username = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(userCodeHeaderIndex).ToString();
            }

            int passWordHeaderIndex = OperationContext.Current.IncomingMessageHeaders.FindHeader("Password", Declarations.NameSpace);
            if (passWordHeaderIndex >= 0)
            {
                userCredential.Password = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(passWordHeaderIndex).ToString();
            }

            return userCredential;
        }
    }
}
