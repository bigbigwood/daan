using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using daan.domain;
using System.Data;
using System.Collections;

namespace daan.service.dict
{
    public class DictdiagnosesmutexService : BaseService
    {
        protected const string modulename = "诊断建议互斥表";

        /// <summary>指定诊断建议对应的互斥列表
        /// 
        /// </summary>
        /// <param name="dictdiagnoseid">诊断ID</param>
        /// <returns></returns>
        public List<Dictdiagnosesmutex> SelectDictdiagnosesmutexLst(string dictdiagnoseid)
        {
            return this.QueryList<Dictdiagnosesmutex>("Dict.SelectDictdiagnosesmutex",dictdiagnoseid).ToList<Dictdiagnosesmutex>();
        }

        /// <summary>新增
        /// 
        /// </summary>
        /// <param name="diagnosis"></param>
        /// <returns></returns>
        public bool AddDictdiagnosesmutex(Dictdiagnosesmutex dictdiagnosesmutex)
        {
            try
            {
                dictdiagnosesmutex.Dictdiagnosesmutexid = getSeqID("SEQ_DICTDIAGNOSESMUTEX");
                this.insert("Dict.InsertDictdiagnosesmutex", dictdiagnosesmutex);
                CacheHelper.RemoveAllCache("daan.GetDictdiagnosesmutexLst");
                List<LogInfo> logLst = getLogInfo<Dictdiagnosesmutex>(new Dictdiagnosesmutex(), dictdiagnosesmutex);
                AddMaintenanceLog("Dictdiagnosesmutex", dictdiagnosesmutex.Dictdiagnosisid, logLst, "新增", dictdiagnosesmutex.Diagnosisname, dictdiagnosesmutex.Dictdiagnosesmutexid.ToString(), modulename);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>删除
        /// 
        /// </summary>
        /// <param name="id">诊断建议主键</param>
        /// <returns></returns>
        public int DelDictdiagnosesmute(Dictdiagnosesmutex dictdiagnosesmutex)
        {
            int nflag = 0;
            try
            {
                nflag = this.delete("Dict.DeleteDictdiagnosesmutex", dictdiagnosesmutex.Dictdiagnosesmutexid);
                CacheHelper.RemoveAllCache("daan.GetDictdiagnosesmutexLst");
                List<LogInfo> logLst = getLogInfo<Dictdiagnosesmutex>(dictdiagnosesmutex, new Dictdiagnosesmutex());
                AddMaintenanceLog("Dictdiagnosesmutex", dictdiagnosesmutex.Dictdiagnosisid, logLst, "删除", dictdiagnosesmutex.Diagnosisname, dictdiagnosesmutex.Dictdiagnosesmutexid.ToString(), modulename);
            }
            catch (Exception ex)
            {         
                throw new Exception(ex.Message);
            }
            return nflag;
        }
        /// <summary>按互斥建议ID查询是否已包含该项
        /// 
        /// </summary>
        /// <param name="dictmutexdiagnosisid"></param>
        /// <returns>true 没有 false 已有</returns>
        public bool SelectIsHaveMutexd(Hashtable htPara)
        {
            DataTable dtTemp = this.selectDS("Dict.SelectIsHaveMutexd", htPara).Tables[0];
            return dtTemp.Rows[0]["counts"].ToString() == "0" ? true : false;
        }
    }
}
