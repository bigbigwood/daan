using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using System.IO;
using System.Web.Services.Description;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Text;
using System.Reflection;
using daan.domain;

namespace daan.webservice
{
    public static class WebServiceUtil
    {
        /// <summary>
        /// 执行webservice方法
        /// </summary>
        /// <param name="lab"></param>
        /// <returns></returns>
        public static string Execute(string methodname, object[] obj)
        {
            string url = getURL("pes");
            if (url == string.Empty)
            {
                return ErrorCode.Login_1001;
            }
            return InvokeWebservice(url, "daan.webservice.phy", "PhyService", methodname, obj).ToString();
        }

        

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
        private static object InvokeWebservice(string url, string @namespace, string classname, string methodname, object[] args)
        {
            try
            {
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
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
                //回收
                GC.Collect();
                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                return string.Format("0|" + ErrorCode.Login_1002 + " {0} \r\n 请求地址：[ {1} ] \r\n  {2}", ex.Message, url, ex.InnerException == null ? "" : ex.InnerException.Message);
            }
        }

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="lab"></param>
        /// <returns></returns>
        private static string getURL(string a)
        {
            string URL = string.Empty;

            bool IsExist = ConfigurationManager.AppSettings.AllKeys.Contains(a);
            if (IsExist)
            {
                URL = ConfigurationManager.AppSettings[a];
            }
            return URL;
        }
    }
}