using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.domain
{
    [Serializable]
    public class LogInfo
    {

        public LogInfo(string _new,string _old,String _caption) {
            this.newValue = _new;
            this.oldValue = _old;
            this.caption = _caption;
        }

        public LogInfo( String _oper)
        {
            this.operation = _oper;

        }

        public LogInfo()
        {

        }

        private String oldValue;

        public String OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }
        private String newValue;

        public String NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }
        private String caption;
        /// <summary>
        /// 中文名
        /// </summary>
        public String Caption
        {
            get { return caption; }
            set { caption = value; }
        }
        private String fieldName;
        /// <summary>
        /// 数据库字段名
        /// </summary>
        public String FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        private string operation;

        public string Operation
        {
            get { return operation; }
            set { operation = value; }
        }

    }
}
