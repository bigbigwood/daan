using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using System.Collections;
using daan.domain;
using System.Data;

namespace daan.service.dict
{
    public class DictCustomerService : BaseService
    {
        protected const string modulename = "体检单位资料维护";

        #region >>>> 分页 获取体检单位列表  zhangwei
        /// <summary>
        /// 获取体检单位列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<Dictcustomer> GetDictCustomerPageLst(Hashtable ht)
        {
            return this.QueryList<Dictcustomer>("Dict.GetDictcustomerPageLst", ht);
        }
        #endregion

        #region   >>>> 获取体检单位列表总项数 zhangwei
        /// <summary>
        /// 获取体检单位列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictCustomerPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictcustomerPageLstCount", ht).Tables[0].Rows[0][0]); 
        }
        #endregion

        #region >>>> 获取所有体检单位列表 zhangwei
        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <returns></returns>
        public IList<Dictcustomer> GetDictCustomerList()
        {
            return this.QueryList<Dictcustomer>("Dict.SelectDictcustomer", null);
        }

        public IList<Dictcustomer> GetDictcustomerByLabid(string labid)
        {
            return this.QueryList<Dictcustomer>("Dict.SelectCustomerByLabID", labid);
        }
        #endregion

        #region >>>> 根据ID获取详细信息 zhangwei
        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public Dictcustomer GetDictCustomerById(double? dictlabId)
        {
            return this.selectObj<Dictcustomer>("Dict.GetDictcustomerInfo", dictlabId);
        }
        public Dictcustomer GetDictCustomerInfo(string str)
        {
            return this.selectObj<Dictcustomer>("Dict.GetDictcustomerInfo", str);
        }
        #endregion
        

        #region >>>> 新增编辑后保存 zhangwei
        ///<summary>
        ///新增编辑后保存
        ///</summary>
        ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictcustomer(Dictcustomer dictCustomer)
        {
            int nflag = 0;
            //新增
            if (dictCustomer.Dictcustomerid == 0 || dictCustomer.Dictcustomerid == null)
            {
                try
                {
                    dictCustomer.Dictcustomerid = getSeqID("SEQ_DICTCUSTOMER");
                    insert("Dict.InsertDictcustomer", dictCustomer);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictcustomer>(new Dictcustomer(), dictCustomer);
                    AddMaintenanceLog("Dictcustomer", int.Parse(dictCustomer.Dictcustomerid.ToString()), logLst, "新增", dictCustomer.Customername.ToString(), dictCustomer.Customercode.ToString(), modulename);                    
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
                    Dictcustomer dictCustomerOld = GetDictCustomerById(dictCustomer.Dictcustomerid);
                    nflag = update("Dict.UpdateDictcustomer", dictCustomer);
                    List<LogInfo> logLst = getLogInfo<Dictcustomer>(dictCustomerOld, dictCustomer);
                    AddMaintenanceLog("Dictcustomer", int.Parse(dictCustomer.Dictcustomerid.ToString()), logLst, "修改", dictCustomer.Customername.ToString(), dictCustomer.Customercode.ToString(), modulename);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            CacheHelper.RemoveAllCache("Dict.SelectDictcustomer");
            return nflag > 0 ;
        }
        #endregion

        #region >>>> 删除 zhangwei
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int DelDictcustomerByID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictcustomer> dictLibraryList = new List<Dictcustomer>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictCustomerById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictcustomer", strId);
                foreach (Dictcustomer item in dictLibraryList)
                {
                    List<LogInfo> logLst = getLogInfo<Dictcustomer>(item, new Dictcustomer());
                   // Dictcustomer dictcustomerDelete = new DictCustomerService().GetDictCustomerById(Convert.ToDouble(item.Dictcustomerid));
                    AddMaintenanceLog("Dictcustomer", item.Dictcustomerid, logLst, "删除", item.Customername, item.Customercode.ToString(), modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            CacheHelper.RemoveAllCache("Dict.SelectDictcustomer");
            return nflag;
        }
        #endregion

        #region >>>> 根据类型获取客户列表 ylp
        /// <summary>
        /// 根据类型获取客户列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<Dictcustomer> GetDictCustomerListByType(string type)
        {
            return this.QueryList<Dictcustomer>("Dict.SelectDictcustomerByType", type);
        }

        public IList<Dictcustomer> GetDictCustomerListByType(Hashtable ht)
        {
            return this.QueryList<Dictcustomer>("Dict.SelectDictcustomerByDictlabId", ht);
        }

        public List<Dictcustomer> GetDictCustomerByCode(Hashtable ht)
        {
            return this.QueryList<Dictcustomer>("Dict.SelectDictcustomerByCode", ht).ToList();
        }
        #endregion

        
        #region >>>> 体检单位审核
        /// <summary>
        /// 查询体检单位审核列表
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetDictcustomerAuditList(Hashtable ht)
        {
            return selectDS("Dict.GetDictcustomerInfoAuditList", ht).Tables[0];
        }
        public int GetDictcustomerAuditListCount(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Dict.GetDictcustomerInfoAuditListCount", ht).Tables[0].Rows[0][0].ToString());
        }

        public DataTable GetDictcustomerdiscountedAuditList(Hashtable ht)
        {
            return selectDS("Dict.DictcustomerdiscountedAuditList", ht).Tables[0];
        }
        public int GetDictcustomerdiscountedAuditListCount(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Dict.DictcustomerdiscountedAuditListCount", ht).Tables[0].Rows[0][0].ToString());
        }

        public DataTable GetDictcustomerExportList(Hashtable ht)
        {
            return selectDS("Dict.GetDictcustomerInfoExportList", ht).Tables[0];
        }

        public DataTable GetDictcustomerdiscountedExportList(Hashtable ht)
        {
            return selectDS("Dict.DictcustomerdiscountedExportList", ht).Tables[0];
        }
        /// <summary>
        /// 审核单位信息
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public int AuditCustomerInfo(string ids,string active)
        {
            int nflag = 0;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("dictcustomerid",ids);
                ht.Add("active",active);
                ht.Add("lastupdatetime",DateTime.Now);
                nflag = update("Dict.AuditCustomerInfo",ht);
                
                string oldActice = string.Empty;
                if (active == "0")
                    oldActice = "1";
                else if (active == "1")
                    oldActice = "0";
                //添加基础资料修改日志
                string[] arr = ids.Split(new char[] { ',' });
                foreach (string id in arr)
                {
                    Dictcustomer dictCustomerNew = GetDictCustomerById(Convert.ToDouble(id));
                    Dictcustomer dictCustomer = GetDictCustomerById(Convert.ToDouble(id));
                    dictCustomer.Active = oldActice;
                    List<LogInfo> logLst = getLogInfo<Dictcustomer>(dictCustomer, dictCustomerNew);
                    AddMaintenanceLog("Dictcustomer", int.Parse(id), logLst, "审核", dictCustomer.Customername, dictCustomer.Customercode, modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            CacheHelper.RemoveAllCache("Dict.SelectDictcustomer");
            return nflag;
        }
        #endregion

        #region >>>> 对接大众健康系统验证 lisp

        public DataTable CheckHasCustomer(string dictcustomercode)
        {
            return selectDS("Select.CheckHasCustomer", dictcustomercode).Tables[0];
        }
        /// <summary>
        /// 查询未同步到易感和大众的体检单位信息
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetNotSynchronizedInfoList(Hashtable ht)
        {
            return selectDS("Dict.GetNotSynchronizedInfoList", ht).Tables[0];
        }
        /// <summary>
        /// 设置单位信息同步状态
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public string setSyncStatus(Hashtable ht)
        {
            string res = string.Empty;
            try
            {
                update("Dict.UpdateSyncStatus", ht);
            }
            catch(Exception e)
            {
                res = e.Message;
            }
            return res;
        }
        #endregion
    }

}
