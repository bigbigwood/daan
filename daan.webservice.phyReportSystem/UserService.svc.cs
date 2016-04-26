using daan.webservice.phyReportSystem.Contract.Messages;
using daan.webservice.phyReportSystem.Framework;
using daan.webservice.phyReportSystem.Operations;
using daan.webservice.phyReportSystem.Contract.Interface;

namespace daan.webservice.phyReportSystem
{
    public class UserService : IUserService
    {
        public AuthenticateResponse Authenticate(AuthenticateRequest request)
        {
            return MessageProcessor.Process(request, new AuthenticateOp());
        }

        public UpdateUserPrinterConfigResponse UpdateUserPrinterConfig(UpdateUserPrinterConfigRequest request)
        {
            return MessageProcessor.Process(request, new UpdateUserPrinterConfigOp());
        }
    }
}
