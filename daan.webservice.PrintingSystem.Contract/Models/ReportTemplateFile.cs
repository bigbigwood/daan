using System;
using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ReportTemplateFile
    {
        /// <summary>
        /// Mapped to Code field
        /// </summary>
        [DataMember]
        public String FileName { get; set; }

        [DataMember]
        public String FileContent { get; set; }

    }
}
