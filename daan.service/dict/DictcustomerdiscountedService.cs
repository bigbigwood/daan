using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using System.Data;
using System.Collections;
using daan.domain;
using daan.util.Common;

namespace daan.service.dict
{
    public class DictcustomerdiscountedService : BaseService
    {
        protected const string modulename = "体检单位总体价格维护";

        #region  >>>> 分页 获取体检单位总体价格列表 zhangwei
        /// <summary>
        /// 获取体检单位总体价格列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<Dictcustomerdiscounted> GetDictcustomerdiscountedPageLst(Hashtable ht)
        {
            return this.QueryList<Dictcustomerdiscounted>("Dict.GetDictcustomerdiscountedPageLst", ht);
        }

        public DataTable GetDictcustomerdiscountedPageDt(Hashtable ht)
        {
            return selectDS("Dict.GetDictcustomerdiscountedPageDt", ht).Tables[0];
        }
        #endregion

        #region >>>> 获取体检单位总体价格列表总项数 zhangwei
        /// <summary>
        /// 获取体检单位总体价格列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictcustomerdiscountedPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictcustomerdiscountedPageLstCount", ht).Tables[0].Rows[0][0]);
        }
        #endregion

        #region >>>> 根据输入的时间判断是否存在记录 zhangwei
        /// <summary>
        ///  修改 根据时间判断是否存在记录
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Dictcustomerdiscounted> GetDictcustomerByTime(Hashtable ht)
        {
            return this.QueryList<Dictcustomerdiscounted>("Dict.GetDictcustomerdiscountedByTime", ht);
        }


        /// <summary>
        /// 新增 根据时间判断是否存在记录
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Dictcustomerdiscounted> GetDictcustomerByTimeTo(Hashtable ht)
        {
            return this.QueryList<Dictcustomerdiscounted>("Dict.GetDictcustomerdiscountedByTimeTo", ht);
        }
        #endregion

        #region >>>> 获取所有信息  zhangwei
        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <returns></returns>
        public IList<Dictcustomerdiscounted> GetDictcustomerdiscountedList()
        {
            return this.QueryList<Dictcustomerdiscounted>("Dict.SelectDictcustomerdiscounted", null);
        }
        #endregion

        #region >>>> 根据ID获取详细信息 zhangwei
        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public Dictcustomerdiscounted GetDictcustomerdiscountedById(double dictlabId)
        {
            //Dictcustomerdiscounted obj = null;
            //IList lst = this.selectIList("Dict.GetDictcustomerdiscountedInfo", dictlabId);
            //if (lst.Count > 0)
            //    obj = (Dictcustomerdiscounted)lst[0];
            //return obj;
            return this.selectObj<Dictcustomerdiscounted>("Dict.GetDictcustomerdiscountedInfo", dictlabId);
        }

        public DataTable GetDictcustomerdiscountedById(string dictid)
        {
            return selectDS("Dict.GetDictcustomerdiscountedInfoByID", dictid).Tables[0];
        }
        #endregion

        #region >>>> 根据对象获取体检单位总体价格详细信息 zhangwei
        /// <summary>
        /// 获取体检单位总体价格详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Dictcustomerdiscounted GetDictcustomerdiscountedInfo(Dictcustomerdiscounted library)
        {
            //Dictcustomerdiscounted obj = null;
            //IList lst = this.selectIList("Dict.GetDictcustomerdiscountedInfo", library);
            //if (lst.Count > 0)
            //    obj = (Dictcustomerdiscounted)lst[0];
            //return obj;
            return this.selectObj<Dictcustomerdiscounted>("Dict.GetDictcustomerdiscountedInfo", library);
           
        }
        #endregion

        #region >>>> 新增编辑后保存  zhangwei
        ///<summary>
        ///新增编辑后保存
        ///</summary>
        ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictcustomerdiscounted(Dictcustomerdiscounted library)
        {
            SortedList SQLlist = new SortedList(new MySort());
            int nflag = 0;
            //新增
            if (library.Dictcustomerdiscountid == 0 || library.Dictcustomerdiscountid  == null)
            {
                try
                {
                    library.Dictcustomerdiscountid = getSeqID("SEQ_DICTCUSTOMERDISCOUNTED");
                    insert("Dict.InsertDictcustomerdiscounted", library);
                    SQLlist.Add(new Hashtable() { { "INSERT", "Dict.InsertDictcustomerdiscounted" } }, library);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictcustomerdiscounted>(new Dictcustomerdiscounted(), library);
                    Dictcustomer dictcustomer = new DictCustomerService().GetDictCustomerById(Convert.ToDouble(library.Dictcustomerid));
                    AddMaintenanceLog("Dictcustomerdiscounted", int.Parse(library.Dictcustomerdiscountid.ToString()), logLst, "新增", dictcustomer.Customername.ToString(), library.Sendoutprice.ToString(),modulename);
                    SQLlist.Add(new Hashtable() { { "INSERT", "dict.InsertMaintenancelog" } }, library);
                    CacheHelper.RemoveAllCache("daan.SelectDictcustomerdiscountedresult");
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
                    Dictcustomerdiscounted dictcustomer = GetDictcustomerdiscountedById(Convert.ToDouble(library.Dictcustomerdiscountid));
                    nflag = update("Dict.UpdateDictcustomerdiscounted", library);
                    List<LogInfo> logLst = getLogInfo<Dictcustomerdiscounted>(dictcustomer, library);
                    Dictcustomer dictcustomerUpdate = new DictCustomerService().GetDictCustomerById(Convert.ToDouble(library.Dictcustomerid));
                    AddMaintenanceLog("Dictcustomerdiscounted", int.Parse(library.Dictcustomerdiscountid.ToString()), logLst, "修改", dictcustomerUpdate.Customername.ToString(), library.Sendoutprice.ToString(),modulename);
                    CacheHelper.RemoveAllCache("daan.SelectDictcustomerdiscountedresult");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return nflag > 0 ;
        }
        #endregion

        #region >>>> 删除 zhangwei
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int DelDictcustomerdiscountedByID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictcustomerdiscounted> dictLibraryList = new List<Dictcustomerdiscounted>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictcustomerdiscountedById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictcustomerdiscounted", strId);
                CacheHelper.RemoveAllCache("daan.SelectDictcustomerdiscountedresult");
                //记录日志
                foreach (Dictcustomerdiscounted item in dictLibraryList)
                {
                    Dictcustomer dictcustomerDelete = new DictCustomerService().GetDictCustomerById(Convert.ToDouble(item.Dictcustomerid));
                    List<LogInfo> logLst = getLogInfo<Dictcustomerdiscounted>(item, new Dictcustomerdiscounted());
                    AddMaintenanceLog("Dictcustomerdiscounted", item.Dictcustomerdiscountid, logLst, "删除", dictcustomerDelete.Customername.ToString(), item.Sendoutprice.ToString(), modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag;
        }
        #endregion

        #region >>>> 审核
        public bool AuditDictcustomerdiscounted(string ids,string dictcustomerid,string active,double? auditby)
        {
            int nflag = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("ids",ids);
                ht.Add("Active", active);
                ht.Add("AuditBy", auditby );
                ht.Add("AuditDate", DateTime.Now);
                nflag = update("Dict.AuditDictcustomerdiscounted", ht);
                string oldActice = string.Empty;
                if (active == "0")
                    oldActice = "1";
                else if (active == "1")
                    oldActice = "0";
                //添加基础资料修改日志
                string[] arr = ids.Split(new char[] { ',' });
                foreach (string id in arr)
                {
                    Dictcustomerdiscounted dictcustomerNew = GetDictcustomerdiscountedById(Convert.ToDouble(id));
                    Dictcustomerdiscounted dictcustomerOld = GetDictcustomerdiscountedById(Convert.ToDouble(id));
                    dictcustomerOld.Active = oldActice;
                    Dictcustomer dictcustomer = new DictCustomerService().GetDictCustomerById(Convert.ToDouble(dictcustomerid));
                    List<LogInfo> logLst = getLogInfo<Dictcustomerdiscounted>(dictcustomerOld, dictcustomerNew);
                    AddMaintenanceLog("Dictcustomerdiscounted", int.Parse(id), logLst, "审核", dictcustomer.Customername.ToString(), "", modulename); 
                }
                CacheHelper.RemoveAllCache("daan.SelectDictcustomerdiscountedresult");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag>0;
        }
        #endregion
    }
}
