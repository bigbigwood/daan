using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using daan.domain;

namespace daan.service.common
{
    public class LogHelper
    {
        /// <summary>
        /// 取得新增日志片段
        /// </summary>
        /// <param name="dtobject"></param>
        /// <returns></returns>
        public static string GetAddString(object dtobject)
        {
            string result = "";
            IEnumerator enumator =
                dtobject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).GetEnumerator();
            while (enumator.MoveNext())
            {
                var info = (PropertyInfo)enumator.Current;
                if (info == null) continue;

                var att = (LogInfoAttribute)Attribute.GetCustomAttribute(info, typeof(LogInfoAttribute));
                if (att == null) continue;

                //此字段需要进行写日志操作
                if (att.option != LogInfoAttribute.LOGFIELDOPTION) continue;
                PropertyInfo thisprop = info;

                //记录日志的字段不是使用此字段
                if (info.Name != att.LinkField && att.LinkField.Length > 0)
                {
                    thisprop = dtobject.GetType().GetProperty(att.LinkField);
                }

                if (thisprop == null) continue;
                var sourcevalue = thisprop.GetValue(dtobject, null) ?? "";

                if (result.Length > 0) result += ",";
                result += string.Format("“{0}”=“{1}”", att.Caption, GetTypeValue(sourcevalue));
            }
            return result;
        }

        /// <summary>
        /// 取得修改日志片段
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newobject"></param>
        /// <returns></returns>
        public static string GetModifyString(object source, object newobject)
        {
            var result = "";
            var propenum =
                source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).GetEnumerator();
            while (propenum.MoveNext())
            {
                var info = (PropertyInfo)propenum.Current;
                if (info == null) continue;

                var att = (LogInfoAttribute)Attribute.GetCustomAttribute(info, typeof(LogInfoAttribute));
                if (att == null) continue;

                //判断此字段才进行写日志操作
                if (att.option != LogInfoAttribute.LOGFIELDOPTION) continue;

                PropertyInfo sourceprop = null;
                PropertyInfo newprop = null;

                sourceprop = info;
                newprop = newobject.GetType().GetProperty(sourceprop.Name);

                //对应字段不一样通过memo 来维护
                if (att.Memo != "") newprop = newobject.GetType().GetProperty(att.Memo);

                //记录日志的字段不是使用此字段
                if (info.Name != att.LinkField && att.LinkField.Length > 0)
                {
                    sourceprop = source.GetType().GetProperty(att.LinkField);
                    newprop = newobject.GetType().GetProperty(att.LinkField);
                }

                if (sourceprop == null) continue;
                if (newprop == null) continue;

                //两个字段不为空

                var sourcevalue = sourceprop.GetValue(source, null) ?? "";
                var newvalue = newprop.GetValue(newobject, null) ?? "";
                if (sourcevalue == newvalue) continue;

                if (result.Length > 0) result += ",";
                result += string.Format("{0}由“{1}” 更改为“{2}”", att.Caption, GetTypeValue(sourcevalue), GetTypeValue(newvalue));
            }

            return result;
        }

        /// <summary>
        /// 类型值转换，提供默认的类型值转换
        /// </summary>
        private const string TRUE_VALUE = "是";
        private const string FALSE_VALUE = "否";
        private static string GetTypeValue(object value)
        {
            var result = value;
            if (value is bool)
            {
                result = FALSE_VALUE;
                if ((bool)value) result = TRUE_VALUE;
            }
            return result.ToString();
        }
    }
}
