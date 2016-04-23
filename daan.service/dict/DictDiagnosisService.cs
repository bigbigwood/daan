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
    public class DictDiagnosisService:BaseService
    {
        protected const string modulename = "诊断建议字典维护";

        #region >>>>新增编辑后保存  ylp
        /// <summary>
        /// 新增编辑后保存
        /// </summary>
        /// <param name="diagnosis"></param>
        /// <returns></returns>
        public double? SaveDiagnosis(Dictdiagnosis diagnosis,Dictdiagnosis diagnosisOld)
        {
            double? nflag = -1;
            //新增
            if (diagnosis.Dictdiagnosisid == null)
            {
                try
                {
                    diagnosis.Dictdiagnosisid = getSeqID("SEQ_DICTDIAGNOSIS");
                    this.insert("Dict.InsertDictdiagnosis", diagnosis);
                    nflag = diagnosis.Dictdiagnosisid;
                    List<LogInfo> logLst = getLogInfo<Dictdiagnosis>(new Dictdiagnosis(), diagnosis);
                    AddMaintenanceLog("Dictdiagnosis", diagnosis.Dictdiagnosisid, logLst, "新增", diagnosis.Diagnosisname, diagnosis.Diagnosiscode, modulename);
                    CacheHelper.RemoveAllCache("daan.GetDictdiagnosisresult");
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
                    nflag = this.update("Dict.UpdateDictdiagnosis", diagnosis);
                    CacheHelper.RemoveAllCache("daan.GetDictdiagnosisresult");
                    List<LogInfo> logLst = getLogInfo<Dictdiagnosis>(diagnosisOld, diagnosis);
                    AddMaintenanceLog("Dictdiagnosis", diagnosis.Dictdiagnosisid, logLst, "修改", diagnosis.Diagnosisname, diagnosis.Diagnosiscode, modulename);
                    nflag = 0;
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

        #region >>>>删除诊断建议 ylp
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">诊断建议主键</param>
        /// <returns></returns>
        public int DelDiagnosisByID(Dictdiagnosis diagnosis)
        {
            int nflag = 0;
            try
            {
                nflag = this.delete("Dict.DeleteDictdiagnosis", diagnosis.Dictdiagnosisid);
                CacheHelper.RemoveAllCache("daan.GetDictdiagnosisresult");
                //增加删除日志对象 fhp
                List<LogInfo> logLst = getLogInfo<Dictdiagnosis>(diagnosis, new Dictdiagnosis());
                AddMaintenanceLog("Dictdiagnosis", diagnosis.Dictdiagnosisid, logLst, "删除", diagnosis.Diagnosisname, diagnosis.Diagnosiscode, modulename);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag;
        }
        #endregion

        #region >>>>诊断建议字典分页 ylp
        /// <summary>
        /// 诊断建议字典分页
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dictdiagnosis> GetDictdiagnosisPageLst(Hashtable ht)
        {
            return this.QueryList<Dictdiagnosis>("Dict.GetDictdiagnosisPageLst", ht).ToList<Dictdiagnosis>();
        }

        /// <summary>
        /// 诊断建议字典分页总数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int GetDictdiagnosisPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictdiagnosisPageLstCount", ht).Tables[0].Rows[0][0]);
        }
        #endregion

        #region >>>>根据条件查找数据  用于导出 ylp
        /// <summary>
        /// 根据条件查找数据  用于导出
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dictdiagnosis> GetDictdiagnosisLst(Hashtable ht)
        {
            return this.QueryList<Dictdiagnosis>("Dict.GetDictdiagnosisPageLstCount", ht).ToList<Dictdiagnosis>();
        }


        /// <summary>
        /// 查找疾病代码是否唯一
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dictdiagnosis> GetDictdiagnosisByCode(Hashtable ht)
        {
            return this.QueryList<Dictdiagnosis>("Dict.SelectDictdiagnosisByCode", ht).ToList<Dictdiagnosis>();
        }
        #endregion


        /// <summary>按诊断建议名称模糊查询        
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dictdiagnosis> SelectDictdiagnosisByNameLst(Hashtable ht)
        {
            return this.QueryList<Dictdiagnosis>("Dict.SelectDictdiagnosisByName", ht).ToList<Dictdiagnosis>();
        }

        /// <summary>获取全部的诊断建议
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dictdiagnosis> SelectDictdiagnosisLst()
        {
            return this.QueryList<Dictdiagnosis>("Dict.SelectDictdiagnosis", null).ToList<Dictdiagnosis>();
        }
    }
}
