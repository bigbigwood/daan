using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using daan.webservice.phyReportSystem.Contract.Messages;
using daan.webservice.phyReportSystem.Framework.Authenticaition;

namespace daan.webservice.phyReportSystem.Framework.Operation
{
    public static class MessageProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static TResponse Process<TRequest, TResponse>(TRequest request, IOperation<TRequest, TResponse> processor)
         where TResponse : ResponseBase, new()
        {
            TResponse result = new TResponse();

            try
            {
                // 1. get session

                // 2. authentication
                Log.Info("Check user credential.");
                var userCredential = new UserCredentialProvider().GetUserCredential();
                var authenticaitionResultCode  = new MockAuthenticaitionServiceImpl().Authenticate(userCredential);
                if (authenticaitionResultCode != AuthenticaitionResultCode.OK)
                {
                    result.ResultType = ResultTypes.AuthenticationError;
                    result.Messages = new String[] { "User password is incorrect" };
                    return result;
                }

                // business
                result = processor.Process(request);

                // transaction commit
            }
            catch (Exception ex)
            {
                Log.Error(ex);

                // transaction rollback
            }
            finally
            {
                Log.Info("Operation is done.");

                // release session
            }

            return (result);
        }

    }
}