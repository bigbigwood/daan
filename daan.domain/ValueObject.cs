 //      Memo: 值对象， 只含有代码、姓名属性 

 using System;

namespace daan.domain
{
    [Serializable]
    public class ValueObject : BaseDomain
    {
        public string CodeNo { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string FastCode { get; set; }

        public new ValueObject Clone()
        {
            return new ValueObject() { CodeNo = CodeNo, Name = Name, Value = Value, FastCode = FastCode, SequenceId = SequenceId };
        }
    }
}