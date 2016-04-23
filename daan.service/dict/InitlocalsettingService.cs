using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using daan.service.common;
using daan.domain;

namespace daan.service.dict
{
    public class InitlocalsettingService : BaseService
    {

        protected const string modulename = "本地参数维护";

        public Initlocalsetting GetInitlocalsetting(string hostmac)
        {
            return this.selectObj<Initlocalsetting>("Dict.SelectInitlocalsetting", hostmac);
        }
           

       /// <summary>
       /// 获取本地参数详细信息
       /// </summary>
       /// <param name="usercode"></param>
       /// <returns></returns>
       public Initlocalsetting GetInitlocalsettingInfo(Initlocalsetting library)
       {
           return this.selectObj<Initlocalsetting>("Dict.GetInitlocalsettingInfo", library);
       }

       #region >>>> 新增编辑后保存 zhangwei
       ///<summary>
       ///新增编辑后保存
       ///</summary>
       ///<param name="library"></param>
       /// <returns></returns>
       public bool SaveDictlab(Initlocalsetting library)
       {
           int nflag = 0;
           //新增
           Initlocalsetting initlocalsettint = new Initlocalsetting();
           initlocalsettint.Hostmac = library.Hostmac;
           Initlocalsetting initlocalsettintList = new InitlocalsettingService().GetInitlocalsettingInfo(initlocalsettint);
          
           if (initlocalsettintList == null)
           {
               try
               {
                   //library.Dictlabid = getSeqID("SEQ_DICTLABID");
                   insert("Dict.InsertInitlocalsetting", library);
                   nflag = 1;
                   List<LogInfo> logLst = getLogInfo<Initlocalsetting>(new Initlocalsetting(), library);
                   AddMaintenanceLog("Initlocalsetting", 0, logLst, "新增", library.Hostmac, library.Hostname, modulename);
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
                   Initlocalsetting dictlab = GetInitlocalsettingInfo(library);
                   nflag = update("Dict.UpdateInitlocalsetting", library);
                   List<LogInfo> logLst = getLogInfo<Initlocalsetting>(dictlab, library);
                   AddMaintenanceLog("Initlocalsetting", 0, logLst, "修改", library.Hostmac, library.Hostname, modulename);
               }
               catch (Exception ex)
               {
                   throw new Exception(ex.Message);
               }
           }
           return nflag > 0;
       }
       #endregion

    }
}
