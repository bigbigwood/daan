#region Copyright HuangCunWei 2010
//
// All rights are reserved. Reproduction or transmission in whole or in part， in
// any form or by any means， electronic， mechanical or otherwise， is prohibited
// without the prior written consent of the copyright owner.
//
// Author:	   HuangCunWei 
// MSN:	       HuangCunWei@gmail.com 
// CreateDate: 2010-2-9
//
// Modified   :  
// ChangedDate: 
// Reason: 
#endregion

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Net;
using System.Web.Services.Description;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Configuration;
using System.Linq;
namespace daan.util.Web
{
    public static class WebUtils
    {

        /// <summary>
        /// 从byte数组生成对象。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object BytesToObj(string str)
        {
            IFormatter formatter = new BinaryFormatter();
            //byte[] byt = Encoding.UTF8.GetBytes(str);
            byte[] byt = Convert.FromBase64String(str);
            object obj = null;
            using (Stream stream = new MemoryStream(byt, 0, byt.Length))
            {
                obj = formatter.Deserialize(stream);
            }
            return obj;
        }

        public static string ObjToBytes(object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            string result = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);

                byte[] byt = new byte[stream.Length];
                byt = stream.ToArray();
                //result = Encoding.UTF8.GetString(byt, 0, byt.Length);
                result = Convert.ToBase64String(byt);
                stream.Flush();
            }
            return result;
        }

        /// <summary>
        /// 删除文件夹及其下所有文件
        /// </summary>
        /// <param name="dir">文件夹的完整物理路径</param>
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件 
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
                Directory.Delete(dir); //删除已空文件夹
            }
        }


        ///// <summary>
        ///// 获取地址
        ///// </summary>
        ///// <param name="lab"></param>
        ///// <returns></returns>
        //public static string getURL(string lab)
        //{
        //    string URL = string.Empty;

        //    bool IsExist = ConfigurationSettings.AppSettings.AllKeys.Contains(lab);
        //    if (IsExist)
        //    {
        //        URL = ConfigurationSettings.AppSettings[lab];
        //    }
        //    return URL;
        //}

        ///// <summary>
        ///// 执行各地webservice方法
        ///// </summary>
        ///// <param name="lab"></param>
        ///// <returns></returns>
        //public static string Execute(string lab, string methodname, object[] obj)
        //{
        //    if (lab == string.Empty) { return ErrorCode.Login_1001; }

        //    string url = getURL(lab);

        //    if (url == string.Empty)
        //    {
        //        return string.Format(ErrorCode.Login_1002 + " 分点代码[ {0} ]！", lab);
        //    }
        //    return InvokeWebservice(url, "LISWebService", "LISService", methodname, obj).ToString();
        //}


        /// <summary> 
        /// 根据指定的信息，调用远程WebService方法 
        /// </summary> 
        /// <param name="url">WebService的http形式的地址</param> 
        /// <param name="namespace">欲调用的WebService的命名空间</param> 
        /// <param name="classname">欲调用的WebService的类名（不包括命名空间前缀）</param> 
        /// <param name="methodname">欲调用的WebService的方法名</param> 
        /// <param name="args">参数列表</param> 
        /// <returns>WebService的执行结果</returns> 
        /// <remarks> 
        /// 如果调用失败，将会抛出Exception。请调用的时候，适当截获异常。 
        /// 异常信息可能会发生在两个地方： 
        /// 1、动态构造WebService的时候，CompileAssembly失败。 
        /// 2、WebService本身执行失败。 
        /// </remarks> 
        /// <example> 
        /// <code> 
        /// object obj = InvokeWebservice("http://localhost/GSP_WorkflowWebservice/common.asmx","Genersoft.Platform.Service.Workflow","Common","GetToolType",new object[]{"1"}); 
        /// </code> 
        /// </example> 
        public static object InvokeWebservice(string url, string @namespace, string classname, string methodname, object[] args)
        {
            WebClient wc = null;
            Stream stream = null;
            try
            {
                wc = new WebClient();
                stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);

                CSharpCodeProvider csc = new CSharpCodeProvider();
                //CodeDomProvider icc = csc.crete;

                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");


                CompilerResults cr = CodeDomProvider.CreateProvider("C#").CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }
                Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                MethodInfo mi = t.GetMethod(methodname);
                csc.Dispose();
                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                return string.Format(" {0} \r\n 请求地址：[ {1} ] \r\n  {2}", ex.Message, url, ex.InnerException == null ? "" : ex.InnerException.Message);
            }
            finally
            {
                stream.Close();
                wc.Dispose();
                GC.Collect();
            }
        }



    }
}

