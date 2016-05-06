using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models.Order
{
    /// <summary>
    /// Orders表状态
    /// </summary>
    [DataContract(Namespace = Declarations.NameSpace)]
    public enum OrdersStatus
    {
        [EnumMember]
        Register = 5,

        [EnumMember]
        BarCodePrint = 10,

        [EnumMember]
        WaitCheck = 15,

        [EnumMember]
        FirstCheck = 20,

        [EnumMember]
        FinishCheck = 25,

        [EnumMember]
        FinishPrint = 30,
    }
}