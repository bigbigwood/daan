using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using daan.webservice.PrintingSystem.Contract.Models;
using daan.webservice.PrintingSystem.Contract.Models.User;

namespace daan.ui.PrintingApplication
{
    public class PrintingApp
    {
        public static UserInfo CurrentUserInfo { get; set; }
        public static UserCredential UserCredential { get; set; }
        public static List<OrganizationInfo> OrganizationAssociations { get; set; }
        public static List<LabInfo> LabAssociations { get; set; }
        public static List<ReportTemplateInfo> ReportTemplates { get; set; }
        public static String CurrentApplicationVersion { get; set; }
    }
}
