using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models
{
    [Serializable]
    [XmlRoot("ClientApplicationVersionConfig")]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ClientApplicationVersionConfig
    {
        [XmlElement("ClientApplicationVersion")]
        [DataMember]
        public List<ClientApplicationVersion> ClientApplicationVersionList { get; set; }
    }

    [Serializable]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ClientApplicationVersion
    {
        [XmlAttribute("ApplicationIdentifier")]
        [DataMember]
        public String ApplicationIdentifier { get; set; }
        [XmlAttribute("ApplicationVersion")]
        [DataMember]
        public String ApplicationVersion { get; set; }
        [XmlAttribute("DownloadUrl")]
        [DataMember]
        public String DownloadUrl { get; set; }
        [XmlAttribute("ReportTemplateVersion")]
        [DataMember]
        public String ReportTemplateVersion { get; set; }
    }
}
