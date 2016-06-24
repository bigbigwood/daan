using System.Runtime.Serialization;
using System.ComponentModel;

namespace daan.webservice.PrintingSystem.Contract.Models.Order
{
    /// <summary>
    /// 报告财务审核状态
    /// </summary>
    [DataContract(Namespace = Declarations.NameSpace)]
    public enum FinanceAuditStatus
    {
        [EnumMember]
        Audit = 1,
        [EnumMember]
        UnAudit = 0,
    }
}