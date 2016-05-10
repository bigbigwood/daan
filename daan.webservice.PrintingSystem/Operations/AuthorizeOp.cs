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

namespace daan.webservice.PrintingSystem.Operations
{
    public class AuthorizeOp : IOperation<AuthorizeRequest, AuthorizeResponse>
    {
        public AuthorizeResponse Process(AuthorizeRequest request)
        {
            var response = new AuthorizeResponse() { ResultType = ResultTypes.Ok };

            var dictUser = new Dictuser() { Usercode = request.Username };
            dictUser = new DictuserService().GetDictuserInfoByUserCode(dictUser);
            if (dictUser == null)
            {
                response.ResultType = ResultTypes.DataValidationError;
                response.Messages = new[] {string.Format("Cannot find dictUser by username={0}", request.Username)};
                return response;
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
            response.LabAssociations = lablist.Select(l => l.ToLabInfo()).ToArray();


            string CustomerType = "0";
            var dictSelectedCustomerList = new List<Dictcustomer>();
            var dictAllCustomerList = loginservice.GetDictcustomer();
            if (dictUser.Dictlabid.HasValue)
            {
                dictSelectedCustomerList = dictAllCustomerList.FindAll(c => (c.Dictlabid == dictUser.Dictlabid && c.Customertype == CustomerType && c.Active == "1") || (c.IsPublic == "1" && c.Active == "1"));
            }
            else  //全部
            {
                foreach (Dictlab dict in lablist)
                {
                    List<Dictcustomer> dictcustomerfirt = dictAllCustomerList.FindAll(c => (c.Dictlabid == dict.Dictlabid && c.Customertype == CustomerType && c.Active == "1") || (c.IsPublic == "1" && c.Active == "1"));
                    foreach (Dictcustomer dictcust in dictcustomerfirt)
                    {
                        if (!dictSelectedCustomerList.Contains(dictcust))
                            dictSelectedCustomerList.Add(dictcust);
                    }
                }
            }
            response.OrganizationAssociations = dictSelectedCustomerList.Select(c => c.ToOrganizationInfo()).ToArray();

            DictreporttemplateService dictreporttemplateService = new DictreporttemplateService();
            var dictReportTemplates = dictreporttemplateService.GetDictreporttemplateAll();
            response.ReportTemplates = dictReportTemplates.Select(r => r.ToReportTemplateInfo()).ToArray();

            return response;
        }
    }
}