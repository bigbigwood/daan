using System;
using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class LabInfo
    {
        [DataMember] 
        public Int32 Id { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String EnglishName { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public String Address { get; set; }
        [DataMember]
        public String Zipcode { get; set; }
        [DataMember]
        public String City { get; set; }
        /// <summary>
        /// 地点代号，例GZ，SH
        /// </summary>
        [DataMember]
        public String CityCode { get; set; }
        [DataMember]
        public String ContractNumber { get; set; }
        [DataMember]
        public String ContractMan { get; set; }
        [DataMember]
        public String Fax { get; set; }
        /// <summary>
        /// 分点小图片路径
        /// </summary>
        [DataMember]
        public String LabImagePath { get; set; }
        [DataMember]
        public String WebsiteUrl { get; set; }
        [DataMember]
        public Int32 DisplayOrder { get; set; }
        /// <summary>
        ///  0未启用1启用[主要指是否有实验室,判断是否扫描有标本]
        /// </summary>
        [DataMember]
        public Boolean IsActive { get; set; }
    }
}
