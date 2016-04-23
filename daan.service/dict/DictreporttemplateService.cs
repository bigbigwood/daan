using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using daan.domain;
using System.Collections;
using System.Data;

namespace daan.service.dict
{
  public  class DictreporttemplateService : BaseService
    {
      //日志列名 fhp
      protected const string modulename = "报表管理";
      //取得数据
      public Dictreporttemplate GetDictreporttemplateByID(string reportID)
      {
          return this.selectObj<Dictreporttemplate>("Dict.GetDictreporttemplateByID", reportID);
      }
      //取得数据
      public Dictreporttemplate GetDictreporttemplateByreportType(string reportType)
      {
          return this.selectObj<Dictreporttemplate>("Dict.GetDictreporttemplateByreportType", reportType);
      }
      //取得所有数据
      public List<Dictreporttemplate> GetDictreporttemplateAll()
      {
          return this.QueryList<Dictreporttemplate>("Dict.GetDictreporttemplateAll", null).ToList<Dictreporttemplate>();
      }
        ///<summary>
        ///新增编辑后保存
        ///</summary>
        ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveReporttemplate(Dictreporttemplate library)
        {

            DictreporttemplateService service = new DictreporttemplateService();
            int nflag = 0;
            //新增
            if (library.Dictreporttemplateid == 0)
            {
                try
                {
                    library.Dictreporttemplateid = getSeqID("SEQ_DICTREPORTTEMPLATE");
                    service.insert("Dict.Insertreporttemplate", library);
                    nflag = 1;
                    //日志 fhp
                    List<LogInfo> logLst = getLogInfo<Dictreporttemplate>(new Dictreporttemplate(), library);
                    AddMaintenanceLog("DICTREPORTTEMPLATE", int.Parse(library.Dictreporttemplateid.ToString()), logLst, "新增", library.Templatename, library.Templatecode, modulename);
                    
                }
                catch (Exception ex)
                {
                    nflag = 0;
                    throw new Exception(ex.Message);
                }
            }
            else//保存
            {
                try
                {
                    //获取旧对象fhp
                    Dictreporttemplate dictreporttemplate = GetDictreporttemplateByID(library.Dictreporttemplateid.ToString());
                    nflag = service.update("Dict.Updatereporttemplate", library);
                    //日志 fhp
                    List<LogInfo> logLst = getLogInfo<Dictreporttemplate>(dictreporttemplate, library);
                    AddMaintenanceLog("DICTREPORTTEMPLATE", int.Parse(library.Dictreporttemplateid.ToString()), logLst, "修改", library.Templatename, library.Templatecode, modulename);
                    
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return nflag > 0 ? true : false;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int DelReporttemplateByID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用 fhp
                List<Dictreporttemplate> dictreporttemplateList = new List<Dictreporttemplate>();
                foreach (string strid in arrayId)
                {
                    dictreporttemplateList.Add(GetDictreporttemplateByID(strid));
                }
                nflag = this.delete("Dict.Deletereporttemplate", strId);
                //记录日志 fhp
                foreach (Dictreporttemplate item in dictreporttemplateList)
                {
                    //增加删除日志对象 fhp
                    List<LogInfo> logLst = getLogInfo<Dictreporttemplate>(item, new Dictreporttemplate());
                    AddMaintenanceLog("DICTREPORTTEMPLATE", item.Dictreporttemplateid, logLst, "删除", item.Templatename, item.Templatecode, modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag;
        }
    }


}
