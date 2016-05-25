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
    public class UpdateUserInfoOp : IOperation<UpdateUserInfoRequest, UpdateUserInfoResponse>
    {
        public UpdateUserInfoResponse Process(UpdateUserInfoRequest request)
        {
            if(request.UserInfo == null 
                || request.UserInfo.UserComputerConfig == null
                || request.UserInfo.UserPrinterConfig == null)
                return new UpdateUserInfoResponse() { ResultType = ResultTypes.DataValidationError };

            var initlocalsettingRepo = RepositoryManager.GetRepository<IInitlocalsettingRepository>();

            var initlocalsetting = initlocalsettingRepo.GetByKey(request.UserInfo.UserComputerConfig.HostMac);
            if (initlocalsetting != null)
            {
                initlocalsetting.Pdfprinter = request.UserInfo.UserPrinterConfig.PdfPrinter;
                initlocalsetting.A4printer = request.UserInfo.UserPrinterConfig.A4Printer;
                initlocalsetting.A5printer = request.UserInfo.UserPrinterConfig.A5Printer;
                initlocalsetting.Barcodeprinter = request.UserInfo.UserPrinterConfig.BarcodePrinter;

                initlocalsettingRepo.Update(initlocalsetting);
            }
            else
            {
                initlocalsetting = new Initlocalsetting();
                initlocalsetting.Hostmac = request.UserInfo.UserComputerConfig.HostMac;
                initlocalsetting.Hostname = request.UserInfo.UserComputerConfig.HostName;
                initlocalsetting.Pdfprinter = request.UserInfo.UserPrinterConfig.PdfPrinter;
                initlocalsetting.A4printer = request.UserInfo.UserPrinterConfig.A4Printer;
                initlocalsetting.A5printer = request.UserInfo.UserPrinterConfig.A5Printer;
                initlocalsetting.Barcodeprinter = request.UserInfo.UserPrinterConfig.BarcodePrinter;

                initlocalsettingRepo.Insert(initlocalsetting);
            }

            //new InitlocalsettingService().SaveDictlab(initlocalsetting);

            return new UpdateUserInfoResponse() { ResultType = ResultTypes.Ok };
        }
    }
}