using System.Collections.Generic;
using daan.webservice.PrintingSystem.Contract.Models;
using daan.webservice.PrintingSystem.Contract.Models.User;

namespace daan.ui.PrintingApplication
{
    public class PrintingApp
    {
        static PrintingApp()
        {
            //CurrentUserInfo = MockUserInfo();
        }

        public static UserInfo CurrentUserInfo { get; set; }
        public static UserCredential UserCredential { get; set; }
        public static List<OrganizationInfo> OrganizationAssociations { get; set; }
        public static List<LabInfo> LabAssociations { get; set; }
        public static List<ReportTemplateInfo> ReportTemplates { get; set; }

        private static UserInfo MockUserInfo()
        {
            return new UserInfo()
            {
                UserPrinterConfig = new UserPrinterConfig() { A4Printer = "Microsoft XPS Document Writer", A5Printer = "HP 910", BarcodePrinter = "Fax", PdfPrinter = "OFax" }
            };
        }
    }
}
