using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models.User
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UserInfo
    {
        public UserInfo()
        {
            DefaultLab = new LabInfo();
            UserPrinterConfig = new UserPrinterConfig();
            UserCredential = new UserCredential();
            LabAssociations = new List<LabInfo>().ToArray();
            OrganizationAssociations = new List<OrganizationInfo>().ToArray();
        }

        [DataMember]
        public LabInfo DefaultLab { get; set; }

        [DataMember]
        public UserPrinterConfig UserPrinterConfig { get; set; }

        [DataMember]
        public UserCredential UserCredential { get; set; }

        [DataMember]
        public LabInfo[] LabAssociations { get; set; }

        [DataMember]
        public OrganizationInfo[] OrganizationAssociations { get; set; }
    }
}