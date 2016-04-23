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
    public class DictSmsModuleService : BaseService
    {
        protected const string modulename = "短信模板维护";

        #region >>>基本查询

        /// <summary>
        /// 获取基础字典详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public DictSmsModule GetDictSmsModuleInfo(string dictSmsModuleId)
        {
            return this.selectObj<DictSmsModule>("Dict.GetDictscoresInfo", dictSmsModuleId);
        }
        /// <summary>
        /// 按条件获取，为空时取全部
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        public IList<DictSmsModule> GetDictSmsModuleLst(DictSmsModule dictSmsModule)
        {
            return this.QueryList<DictSmsModule>("Dict.GetDictSmsModuleLst", dictSmsModule);
        }           
        #endregion

        #region >>>新增编辑后保存
        /// <summary>
        /// 新增编辑后保存
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        public bool SaveDictSmsModule(DictSmsModule dictSmsModule)
        {
            int nflag = 0;
            //新增
            if (dictSmsModule.DictSmsModuleid == 0)
            {
                try
                {
                    dictSmsModule.DictSmsModuleid = getSeqID("SEQ_DICTSMSMODULE");
                    insert("Dict.InsertDictSmsModule", dictSmsModule);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<DictSmsModule>(new DictSmsModule(), dictSmsModule);
                    AddMaintenanceLog("DictSmsModule", dictSmsModule.DictSmsModuleid, logLst, "新增", dictSmsModule.SmsTitle, null, modulename);
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
                    //获取待修改原始数据
                    DictSmsModule tempDictSmsModule = new DictSmsModule();
                    tempDictSmsModule.DictSmsModuleid = dictSmsModule.DictSmsModuleid;
                    DictSmsModule oldDictSmsModule = GetDictSmsModuleInfo(tempDictSmsModule.DictSmsModuleid.ToString());

                    nflag = update("Dict.UpdateDictSmsModule", dictSmsModule);
                    List<LogInfo> logLst = getLogInfo<DictSmsModule>(oldDictSmsModule, dictSmsModule);
                    AddMaintenanceLog("DictSmsModule", dictSmsModule.DictSmsModuleid, logLst, "修改", dictSmsModule.SmsTitle, null, modulename);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            CacheHelper.RemoveAllCache("daan.GetDictSmsModuleLst");
            return nflag > 0;
        }
        #endregion

        #region >>>删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="usercode">由逗号组成的ID字符串</param>
        /// <returns></returns>
        public int DelDictSmsModuleByID(string strid)
        {
            int nflag = 0;
            try
            {
                //临时存储待删除对象，备写日志用
                DictSmsModule dictSmsModule = GetDictSmsModuleInfo(strid);

                //删除
                nflag = this.delete("Dict.DeleteDictSmsModule", strid);
                //记录日志

                AddMaintenanceLog("DictSmsModule", dictSmsModule.DictSmsModuleid, null, "删除", dictSmsModule.SmsTitle, null, modulename);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            CacheHelper.RemoveAllCache("daan.GetDictSmsModuleLst");
            return nflag;
        }
        #endregion
    }
}
