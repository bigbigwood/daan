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

            var dictUser = new Dictuser() { Usercode = request.Username };
            dictUser = new DictuserService().GetDictuserInfoByUserCode(dictUser);
            if (dictUser == null)
            {
                throw new Exception("Canoot find dictUser");
            }

            var domainUserInfo = new daan.domain.UserInfo();
            domainUserInfo.userCode = dictUser.Usercode;
            domainUserInfo.userName = dictUser.Username;
            domainUserInfo.userId = Convert.ToInt32(dictUser.Dictuserid);
            domainUserInfo.loginTime = DateTime.Now;
            domainUserInfo.joinLabidstr = dictUser.Joinlabid;
            domainUserInfo.dictlabid = dictUser.Dictlabid;
            domainUserInfo.joinDeptstr = dictUser.Joindeptid;
            domainUserInfo.dictlabdeptid = dictUser.Dictlabdeptid;
            

            bool enablePermissionControl = true;
            LoginService loginservice = new LoginService();
            List<Dictlab> lablist = new List<Dictlab>();
            if (enablePermissionControl)
            {
                lablist = loginservice.GetPermissionDictlab(domainUserInfo);
            }
            else
            {
                lablist = loginservice.GetLoginDictlab();
            }
            List<LabInfo> labInfos = lablist.Select(l => l.ToLabInfo()).ToList();
            userInfo.LabAssociations = labInfos.ToArray();

            if (dictUser.Dictlabid.HasValue)
                userInfo.DefaultLab = lablist.FirstOrDefault(l => l.Dictlabid == dictUser.Dictlabid).ToLabInfo();

            List<Dictcustomer> dictcustomerback = new List<Dictcustomer>();
            string Customertype = "0";
            if (dictUser.Dictlabid.HasValue)
            {
                dictcustomerback = loginservice.GetDictcustomer().FindAll(c => (c.Dictlabid == dictUser.Dictlabid && c.Customertype == Customertype && c.Active == "1") || (c.IsPublic == "1" && c.Active == "1"));
            }
            else   //全部
            {
                List<Dictcustomer> CustomerList = loginservice.GetDictcustomer();
                List<Dictlab> dictList = loginservice.GetPermissionDictlab(domainUserInfo);

                foreach (Dictlab dict in dictList)
                {
                    List<Dictcustomer> dictcustomerfirt = CustomerList.FindAll(c => (c.Dictlabid == dict.Dictlabid && c.Customertype == Customertype && c.Active == "1") || (c.IsPublic == "1" && c.Active == "1"));
                    foreach (Dictcustomer dictcust in dictcustomerfirt)
                    {
                        if (!dictcustomerback.Contains(dictcust))
                            dictcustomerback.Add(dictcust);
                    }
                }
            }
            List<OrganizationInfo> organizationInfos = dictcustomerback.Select(c => c.ToOrganizationInfo()).ToList();
            userInfo.OrganizationAssociations = organizationInfos.ToArray();

            return new AuthenticateResponse() { ResultType = ResultTypes.Ok, UserInfo = userInfo};
        }
    }
}