using daan.service.dict;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models.User;
using daan.webservice.PrintingSystem.Framework.Operation;

namespace daan.webservice.PrintingSystem.Operations
{
    public class AuthenticateOp : IOperation<AuthenticateRequest, AuthenticateResponse>
    {
        public AuthenticateResponse Process(AuthenticateRequest request)
        {
            var userInfo = new UserInfo();

            var initlocalsetting = new InitlocalsettingService().GetInitlocalsetting(request.HostMac);
            if (initlocalsetting != null)
            {
                userInfo.UserPrinterConfig = new UserPrinterConfig()
                {
                    A4Printer = initlocalsetting.A4printer,
                    A5Printer = initlocalsetting.A5printer,
                    BarcodePrinter = initlocalsetting.Barcodeprinter,
                    PdfPrinter = initlocalsetting.Pdfprinter
                };
            }

            return new AuthenticateResponse() { ResultType = ResultTypes.Ok, UserInfo = userInfo};
        }
    }
}