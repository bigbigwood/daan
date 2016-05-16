using System;
using System.Linq;
using System.Collections.Generic;
using daan.domain;
using daan.service.dict;
using daan.service.login;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models;
using daan.webservice.PrintingSystem.Contract.Models.User;
using daan.webservice.PrintingSystem.Framework.Operation;
using daan.webservice.PrintingSystem.Helper;
using daan.webservice.PrintingSystem.Repository;
using daan.webservice.PrintingSystem.Repository.Interfaces;
using UserInfo = daan.webservice.PrintingSystem.Contract.Models.User.UserInfo;

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

            var userRepo = RepositoryManager.GetRepository<IDictUserRepository>();
            var dictUser = userRepo.GetByUserCode(request.Username);
            if (dictUser != null && dictUser.Dictlabid.HasValue)
            {
                var dictLabInfo = new DictlabService().GetDictlabById(dictUser.Dictlabid.Value);
                if (dictLabInfo != null) userInfo.DefaultLab = dictLabInfo.ToLabInfo();
            }

            return new AuthenticateResponse() { ResultType = ResultTypes.Ok, UserInfo = userInfo };
        }
    }
}