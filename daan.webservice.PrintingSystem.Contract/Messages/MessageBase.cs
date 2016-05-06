using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public enum ResultTypes
    {
        [EnumMember]
        Ok = 0,
        [EnumMember]
        AuthenticationError = 1,
        [EnumMember]
        AuthorizationError = 2,
        [EnumMember]
        DataValidationError = 3,
        [EnumMember]
        BussinessLogicError = 4,
        [EnumMember]
        PartiallyOk = 5,
        [EnumMember]
        Queued = 98,
        [EnumMember]
        UnknownError = 99,
    }

    [MessageContract(IsWrapped = true)]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class RequestBase
    {
        [MessageHeader(Name = "Username")]
        public string Username { get; set; }
        [MessageHeader(Name = "Password")]
        public string Password { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ResponseBase
    {
        [DataMember(Order = 900)]
        [MessageBodyMember(Order = 900)]
        public ResultTypes ResultType { get; set; }

        //[DataMember(Order = 950)]
        //[MessageBodyMember(Order = 950)]
        //public String JsonResult { get; set; }

        [DataMember(Order = 999)]
        [MessageBodyMember(Order = 999)]
        public String[] Messages { get; set; }
    }
}