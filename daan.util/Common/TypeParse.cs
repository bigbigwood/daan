/**********************************************
 * �����ã�   ����ת����
 * �����ˣ�   daan
 * ����ʱ�䣺 2012-03-28 
 ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace daan.util.Common
{
    public class TypeParse
    {
        /// <summary>
        /// string��ת��Ϊbool��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����bool���ͽ��</returns>
        public static bool StrToBool(object Expression, bool defValue)
        {
            if (Expression != null)
            {
                if (string.Compare(Expression.ToString(), "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(Expression.ToString(), "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        /// <summary>
        /// ������ת��ΪInt32����
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static int StrToInt(object strValue, int defValue)
        {
            int dValue;
            return int.TryParse(strValue.ToString(), out dValue) ? dValue : defValue;
                      
        }
        /// <summary>
        /// ������ת��ΪInt32����
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static double StrToDouble(string strValue, double defValue)
        {
            double dValue;
            return double.TryParse(strValue.ToString(), out dValue) ? dValue : defValue;

        }
        /// <summary>
        /// object��ת��Ϊfloat��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            float dValue;
            return float.TryParse(strValue.ToString(), out dValue) ? dValue : defValue;
        }

        /// <summary>
        /// object��ת��Ϊdouble��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static double StrToDouble(object strValue, double defValue)
        {            
            if (strValue == null)
                return 0;
            double dValue;
            return double.TryParse(strValue.ToString(), out dValue) ? dValue : defValue;
        }

        /// <summary>
        /// ��long����ֵת��ΪInt32����
        /// </summary>
        /// <param name="objNum"></param>
        /// <returns></returns>
        public static int SafeInt32(object objNum)
        {
            if (objNum == null)
            {
                return 0;
            }
            string strNum = objNum.ToString();
            if (ValidateUtils.IsNumeric(strNum))
            {

                if (strNum.ToString().Length > 9)
                {
                    if (strNum.StartsWith("-"))
                    {
                        return int.MinValue;
                    }
                    else
                    {
                        return int.MaxValue;
                    }
                }
                return Int32.Parse(strNum);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// object��ת��Ϊstring��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����string���ͽ��</returns>
        public static string ObjToStr(object strValue, string defValue)
        {
            return strValue == null ? defValue : strValue.ToString();          
        }

        /// <summary>
        /// int��ת��Ϊstring��
        /// </summary>
        /// <returns>ת�����string���ͽ��</returns>
        public static string IntToStr(int intValue)
        {
            return Convert.ToString(intValue);
        }
    }
}
