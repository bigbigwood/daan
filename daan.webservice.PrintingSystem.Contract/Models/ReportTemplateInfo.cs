using System;
using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ReportTemplateInfo
    {
        [DataMember]
        public Int32 Id { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        [DataMember]
        public String Name { get; set; }
        /// <summary>
        /// 模板代码, 文件名称
        /// </summary>
        [DataMember]
        public String Code { get; set; }
        /// <summary>
        /// 报告类型对应dictlibary表报告类型的ID,有常规报告，HPV报告，易感基因报告
        /// </summary>
        [DataMember]
        public Int32 ReportType { get; set; }
        /// <summary>
        /// 纸张大小
        /// </summary>
        [DataMember]
        public String PaperSize { get; set; }
        /// <summary>
        /// 是否单独评价、总检。0，不是；1，是。易感基因项目总检单独评价
        /// </summary>
        [DataMember]
        public String SingleAppraise { get; set; }
    }
}
