using System;
using System.Diagnostics;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Authenticaition;
using daan.webservice.PrintingSystem.Framework.Operation;
using log4net;

namespace daan.webservice.PrintingSystem.Framework
{
    public static class MessageProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static TResponse Process<TRequest, TResponse>(TRequest request, IOperation<TRequest, TResponse> processor)
         where TResponse : ResponseBase, new()
         where TRequest : RequestBase, new()
        {
            TResponse result = new TResponse();
            var sw = Stopwatch.StartNew();

            try
            {
                // 1. get session

                // 2. authentication
                Log.Info("Check user credential.");
                var authenticaitionResultCode = ObjectFactory.GetImpl<IAuthenticaitionService>().Authenticate(request.Username, request.Password);
                if (authenticaitionResultCode != AuthenticaitionResultCode.Ok)
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
                result.ResultType = ResultTypes.UnknownError;
                // transaction rollback
            }
            finally
            {
                Log.InfoFormat("Finish processing {0}, cost {1} milliseconds", processor.ToString(), sw.ElapsedMilliseconds);

                // release session
            }

            return (result);
        }

    }
}