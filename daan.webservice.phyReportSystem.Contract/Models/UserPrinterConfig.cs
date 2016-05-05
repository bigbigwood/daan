using System;
using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UserPrinterConfig
    {
        [DataMember]
        public String A4Printer { get; set; }
        [DataMember]
        public String A5Printer { get; set; }
        [DataMember]
        public String BarcodePrinter { get; set; }
        [DataMember]
        public String PdfPrinter { get; set; }
    }
}