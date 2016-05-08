using System;
using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class OrganizationInfo
    {
        [DataMember]
        public Int32 Id { get; set; }
        [DataMember]
        public String Code { get; set; }
        [DataMember]
        public String FastCode { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String AliasName { get; set; }
        [DataMember]
        public String EnglishName { get; set; }
        [DataMember]
        public String EndlishAddress { get; set; }
        [DataMember]
        public String Address { get; set; }
        [DataMember]
        public String Telephone { get; set; }
        [DataMember]
        public String Fax { get; set; }
        [DataMember]
        public String ZipCode { get; set; }
        [DataMember]
        public String ContractMan { get; set; }
        [DataMember]
        public String ContractNumber { get; set; }
        [DataMember]
        public String Email { get; set; }
        [DataMember]
        public Boolean Active { get; set; }
        [DataMember]
        public Boolean EnableSmsNotification { get; set; }
        [DataMember]
        public String Remark { get; set; }
        /// <summary>
        /// 客户状态：合作客户/意向客户/终止客户
        /// </summary>
        [DataMember]
        public String Status { get; set; }
        [DataMember]
        public String ErpCode { get; set; }
        [DataMember]
        public String ErpName { get; set; }
        [DataMember]
        public String DocumentType { get; set; }
        [DataMember]
        public String DocumentCode { get; set; }
        [DataMember]
        public Int32 DictSalemanId { get; set; }
        [DataMember]
        public Int32 DictCheckBillId { get; set; }
        /// <summary>
        /// 分点实验室ID
        /// </summary>		
        [DataMember]
        public Int32 LabId { get; set; }
        /// <summary>
        /// 0- 一般客户 1-外包客户
        /// </summary>		
        [DataMember]
        public Int32 CustomerType { get; set; }
        /// <summary>
        /// 客户自定义报告单抬头
        /// </summary>		
        [DataMember]
        public string CustomReportTitle { get; set; }
        /// <summary>
        /// 是否公用单位
        /// </summary>
        [DataMember]
        public Boolean IsPublic { get; set; }
        /// <summary>
        /// 排序
        /// </summary>	
        [DataMember]
        public Int32 DisplayOrder { get; set; }
        [DataMember]
        public DateTime LastUpdateDate { get; set; }
        /// <summary>
        /// 易感基因系统同步状态
        /// </summary>
        [DataMember]
        public string YGSyncStatus { get; set; }
        /// <summary>
        /// 大众健康系统同步状态
        /// </summary>
        [DataMember]
        public string DZSyncStatus { get; set; }
    }
}
