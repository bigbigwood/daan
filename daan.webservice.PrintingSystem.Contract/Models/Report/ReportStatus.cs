using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models.Report
{
    /// <summary>
    /// Report状态
    /// </summary>
    [DataContract(Namespace = Declarations.NameSpace)]
    public enum ReportStatus
    {
        [EnumMember]
        Normal = 0,

        [EnumMember]
        Delay = 1,

        [EnumMember]
        Refund = 2,
    }
}