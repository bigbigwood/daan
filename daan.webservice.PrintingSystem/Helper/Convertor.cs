using System;
using System.IO;
using System.Xml.Serialization;
using daan.domain;
using daan.webservice.PrintingSystem.Contract.Models;

namespace daan.webservice.PrintingSystem.Helper
{
    public static class Convertor
    {
        public static LabInfo ToLabInfo(this Dictlab domainDictlab)
        {
            if (domainDictlab == null)
                return null;

            var model = new LabInfo();
            model.Id = domainDictlab.Dictlabid.HasValue ? (int)domainDictlab.Dictlabid : 0;
            model.Name = domainDictlab.Labname;
            model.EnglishName = domainDictlab.Esitename;
            model.Description = domainDictlab.Labdescription;
            model.Address = domainDictlab.Addres;
            model.Zipcode = domainDictlab.Zpcode;
            model.City = domainDictlab.City;
            model.CityCode = domainDictlab.Labcode;
            model.ContractNumber = domainDictlab.Phone;
            model.ContractMan = domainDictlab.Contactman;
            model.Fax = domainDictlab.Fax;
            model.LabImagePath = domainDictlab.Labimage;
            model.WebsiteUrl = domainDictlab.Website;
            model.DisplayOrder = domainDictlab.Displayorder.HasValue ? (int)domainDictlab.Displayorder : 0;
            model.IsActive = domainDictlab.IsActive == '1';

            return model;
        }

        public static OrganizationInfo ToOrganizationInfo(this Dictcustomer domainDictcustomer)
        {
            if (domainDictcustomer == null)
                return null;

            var model = new OrganizationInfo();
            model.Id = domainDictcustomer.Dictcustomerid.HasValue ? (int)domainDictcustomer.Dictcustomerid : 0;
            model.Code = domainDictcustomer.Customercode;
            model.FastCode = domainDictcustomer.Fastcode;
            model.Name = domainDictcustomer.Customername;
            model.AliasName = domainDictcustomer.Customername2;
            model.EnglishName = domainDictcustomer.Customerengname;
            model.EndlishAddress = domainDictcustomer.Engaddress;
            model.Address = domainDictcustomer.Address;
            model.Telephone = domainDictcustomer.Telephone;
            model.Fax = domainDictcustomer.Fax;
            model.ZipCode = domainDictcustomer.Postcode;
            model.ContractMan = domainDictcustomer.Contactman;
            model.ContractNumber = domainDictcustomer.Contactphone;
            model.Email = domainDictcustomer.Email;
            model.Active = (domainDictcustomer.Active == "1");
            model.EnableSmsNotification = (domainDictcustomer.Issms == "1");
            model.Remark = domainDictcustomer.Remark;
            model.Status = domainDictcustomer.Status;
            model.ErpCode = domainDictcustomer.Erpcode;
            model.ErpName = domainDictcustomer.Erpname;
            model.DocumentType = domainDictcustomer.Documenttype;
            model.DocumentCode = domainDictcustomer.Documentcode;
            model.DictSalemanId = domainDictcustomer.Dictsalemanid.HasValue ? (int)domainDictcustomer.Dictsalemanid : 0;
            model.DictCheckBillId = domainDictcustomer.Dictcheckbillid.HasValue ? (int)domainDictcustomer.Dictcheckbillid : 0;
            model.LabId = domainDictcustomer.Dictlabid.HasValue ? (int)domainDictcustomer.Dictlabid : 0;
            //model.CustomerType = int.Parse(domainDictcustomer.Customertype);
            model.CustomReportTitle = domainDictcustomer.Reporttitle;
            //model.IsPublic = domainDictcustomer.IsPublic;
            model.DisplayOrder = domainDictcustomer.Displayorder.HasValue ? (int)domainDictcustomer.Displayorder : 0;
            model.LastUpdateDate = domainDictcustomer.Lastupdatedate;
            model.YGSyncStatus = domainDictcustomer.YGSyncStatus;
            model.DZSyncStatus = domainDictcustomer.DZSyncStatus;

            return model;
        }
    }
}


