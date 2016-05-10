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
            UserPrinterConfig = new UserPrinterConfig();
            DefaultLab = new LabInfo();
        }

        [DataMember]
        public UserPrinterConfig UserPrinterConfig { get; set; }

        [DataMember]
        public LabInfo DefaultLab { get; set; }
    }
}