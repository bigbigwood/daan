#region Copyright HuangCunWei 2009
//
// All rights are reserved. Reproduction or transmission in whole or in part�� in
// any form or by any means�� electronic�� mechanical or otherwise�� is prohibited
// without the prior written consent of the copyright owner.
//
// Author:	   HuangCunWei 
// MSN:	       HuangCunWei@gmail.com 
// CreateDate: 2009-08-13
//
// Modified   :  
// ChangedDate: 
// Reason: 
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TimeHelper
{
    /// <summary>
    /// SerializationHelper ��ժҪ˵����
    /// </summary>
    public class SerializationHelper
    {
        private SerializationHelper()
        {
        }

        private static readonly Dictionary<int, XmlSerializer> serializer_dict = new Dictionary<int, XmlSerializer>();

        ///<summary>
        ///</summary>
        ///<param name="t"></param>
        ///<returns></returns>
        public static XmlSerializer GetSerializer(Type t)
        {
            int type_hash = t.GetHashCode();

            if (!serializer_dict.ContainsKey(type_hash))
            {
                serializer_dict.Add(type_hash, new XmlSerializer(t));
            }

            return serializer_dict[type_hash];
        }

        /// <summary>
        /// �����л�
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="filename">�ļ�·��</param>
        /// <returns></returns>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream...
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }


        /// <summary>
        /// ���л�
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="filename">�ļ�·��</param>
        public static bool Save(object obj, string filename)
        {
            FileStream fs = null;
            // serialize it...
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return true;

        }

        /// <summary>
        /// xml���л����ַ���
        /// </summary>
        /// <param name="obj">����</param>
        /// <returns>xml�ַ���</returns>
        public static string Serialize(object obj)
        {
            string returnStr = "";

            XmlSerializer serializer = GetSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = null;
            StreamReader sr = null;
            try
            {
                xtw = new XmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = Formatting.Indented;
                serializer.Serialize(xtw, obj);
                ms.Seek(0, SeekOrigin.Begin);
                sr = new StreamReader(ms);
                returnStr = sr.ReadToEnd();
            }
            finally
            {
                if (xtw != null)
                {
                    xtw.Close();
                }
                if (sr != null)
                {
                    sr.Close();
                }
                ms.Close();
            }
            return returnStr;
        }

        ///<summary>
        ///</summary>
        ///<param name="type"></param>
        ///<param name="s"></param>
        ///<returns></returns>
        public static object DeSerialize(Type type, string s)
        {
            byte[] b = Encoding.UTF8.GetBytes(s);
            XmlSerializer serializer = GetSerializer(type);
            return serializer.Deserialize(new MemoryStream(b));
        }
    }
}
