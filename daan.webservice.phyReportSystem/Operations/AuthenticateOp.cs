using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Operation;

namespace daan.webservice.PrintingSystem.Operations
{
    public class AuthenticateOp : IOperation<AuthenticateRequest, AuthenticateResponse>
    {
        public AuthenticateResponse Process(AuthenticateRequest request)
        {
            return new AuthenticateResponse() { ResultType = ResultTypes.Ok };
        }
    }
}