using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using daan.domain;
using System.Data;
using System.Collections;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace daan.service.dict
{
    public class DictRuleFormularService : BaseService
    {
        protected const string modulename = "诊断建议规则公式维护";

        #region >>>>新增编辑后保存  ylp
        /// <summary>
        /// 新增编辑后保存
        /// </summary>
        /// <param name="ruleformular"></param>
        /// <returns></returns>
        public double? SaveFuleFormular(Dictruleformular ruleformular, Dictruleformular ruleformularOld)
        {
            double? nflag = -1;
            //新增
            if (ruleformular.Dictruleformularid == null)
            {
                try
                {
                    ruleformular.Dictruleformularid = getSeqID("SEQ_DICTRULEFORMULAR");
                    ruleformular.Createdate = DateTime.Now;
                    this.insert("Dict.InsertDictruleformular", ruleformular);
                    CacheHelper.RemoveAllCache("daan.GetDictruleformularresult");
                    nflag = ruleformular.Dictruleformularid;
                    List<LogInfo> logLst = getLogInfo<Dictruleformular>(new Dictruleformular(), ruleformular);
                    AddMaintenanceLog("Dictruleformular", ruleformular.Dictruleformularid, logLst, "新增", ruleformular.Formularname, ruleformular.Dictrulecode, modulename);
                }
                catch (Exception ex)
                {
                    nflag = -1;
                    throw new Exception(ex.Message);
                }
            }
            else//保存
            {
                try
                {
                    //重新获取旧对象 传过来的值已经变为新对象的值 fhp
                    ruleformularOld = SelectDictruleformularByruleformularid(ruleformular.Dictruleformularid);
                    this.update("Dict.UpdateDictruleformular", ruleformular);
                    CacheHelper.RemoveAllCache("daan.GetDictruleformularresult");
                    nflag = 0;
                    List<LogInfo> logLst = getLogInfo<Dictruleformular>(ruleformularOld, ruleformular);
                    AddMaintenanceLog("Dictruleformular", ruleformular.Dictruleformularid, logLst, "修改", ruleformular.Formularname, ruleformular.Dictrulecode, modulename);
                }
                catch (Exception ex)
                {
                    nflag = -1;
                    throw new Exception(ex.Message);
                }
            }
            return nflag;
        }

        #endregion

        #region >>>>删除规则公式 ylp
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">规则公式主键</param>
        /// <returns></returns>
        public int DelDictRuleFormularByID(Dictruleformular ruleformular)
        {
            int nflag = 0;
            try
            {
                nflag = this.delete("Dict.DeleteDictruleformular", ruleformular.Dictruleformularid);
                CacheHelper.RemoveAllCache("daan.GetDictruleformularresult");
                //日志 fhp
                List<LogInfo> logLst = getLogInfo<Dictruleformular>(ruleformular, new Dictruleformular());
                AddMaintenanceLog("Dictruleformular", ruleformular.Dictruleformularid, logLst, "删除", ruleformular.Formularname, ruleformular.Dictrulecode, modulename);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag;
        }
        #endregion

        #region 获到计算公式动态编译后的结果
        /// <summary>
        /// 获到计算公式动态编译后的结果
        /// </summary>
        /// <param name="sourcecode">编译后的源码</param>
        /// <param name="obj">参数数组，要求各参数数据类型</param>
        /// <returns></returns>
        public static bool GetRuleFormularResult(string sourcecode, object[] obj)
        {
            //开始调用动态编译类
            try
            {
                CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
                ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();
                CompilerParameters objCompilerParameters = new CompilerParameters();
                objCompilerParameters.ReferencedAssemblies.Add("System.dll");
                objCompilerParameters.GenerateExecutable = false;
                objCompilerParameters.GenerateInMemory = true;
                CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, sourcecode);
                Assembly objAssembly = cr.CompiledAssembly;
                object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
                MethodInfo objMl = objHelloWorld.GetType().GetMethod("OutPut");
                object objresult = objMl.Invoke(objHelloWorld, obj);
                return Convert.ToBoolean(objresult);
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region >>>>规则公式分页 ylp
        /// <summary>
        /// 规则公式分页
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dictruleformular> GetDictruleformularPageLst(Hashtable ht)
        {
            return this.QueryList<Dictruleformular>("Dict.GetDictruleformularPageLst", ht).ToList<Dictruleformular>();
        }

        /// <summary>
        /// 规则公式分页总数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int GetDictruleformularPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictruleformularPageLstCount", ht).Tables[0].Rows[0][0]); 
        }
        #endregion

        #region >>>>根据条件查找规则公式 用于导出 ylp
        /// <summary>
        /// 规则公式
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dictruleformular> GetDictruleformularLst(Hashtable ht)
        {
            return this.QueryList<Dictruleformular>("Dict.SelectDictruleformular", ht).ToList<Dictruleformular>();
        }
        #endregion

        #region >>>>根据id得到实体 ylp
        /// <summary>
        /// 根据id得到实体
        /// </summary>
        /// <param name="ruleformularid"></param>
        /// <returns></returns>
        public Dictruleformular SelectDictruleformularByruleformularid(double? ruleformularid)
        {
            try
            {
                return this.selectObj<Dictruleformular>("Dict.SelectDictruleformularById", ruleformularid);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region >>>>根据条件查找规则公式 ylp
        /// <summary>
        /// 根据条件查找规则公式
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public Dictruleformular SelectDictruleformularByrule(Hashtable ht)
        {
            try
            {
                return this.selectObj<Dictruleformular>("Dict.SelectDictruleformularByrule", ht);
            }
            catch
            {
                return null;
            }
        }
        #endregion 

        /// <summary>
        /// 查找规则代码是否唯一
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dictruleformular> SelectDictruleformularByCode(Hashtable ht)
        {
            return this.QueryList<Dictruleformular>("Dict.SelectDictruleformularByCode", ht).ToList<Dictruleformular>();
        }
    }
}
