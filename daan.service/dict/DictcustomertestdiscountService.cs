using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using System.Data;
using System.Collections;
using daan.domain;

namespace daan.service.dict
{
  public  class DictcustomertestdiscountService :BaseService
  {
      protected const string modulename = "外包单位价格维护";

        #region  >>>> 分页获取外包单位总体价格列表 zhangwei
        /// <summary>
      /// 获取外包单位总体价格列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
      public IList<Dictcustomertestdiscount> GetDictcustomerdiscountPageLst(Hashtable ht)
        {
            return this.QueryList<Dictcustomertestdiscount>("Dict.GetDictcustomerdiscounPageLst", ht);
        }

      #endregion

        #region >>>> 获取外包单位总体价格列表总项数 zhangwei
        /// <summary>
        /// 获取外包单位总体价格列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictcustomerdiscountPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictcustomerdiscounPageLstCount", ht).Tables[0].Rows[0][0]); 
        }
        #endregion

        #region >>>> 根据时间判断是否存在记录 zhangwei
        /// <summary>
        /// 根据时间判断是否存在记录
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Dictcustomertestdiscount> GetDictcustomerByTime(Hashtable ht)
        {
            return this.QueryList<Dictcustomertestdiscount>("Dict.GetDictcustomerdiscounByTime", ht);
        }
        public IList<Dictcustomertestdiscount> GetDictcustomerByTimeTo(Hashtable ht)
        {
            return this.QueryList<Dictcustomertestdiscount>("Dict.GetDictcustomerdiscounByTimeTo", ht);
        }
        #endregion

        #region >>>> 获取详细信息 zhangwei
        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <returns></returns>
        public IList<Dictcustomertestdiscount> GetDictcustomerdiscountList()
        {
            return this.QueryList<Dictcustomertestdiscount>("Dict.SelectDictcustomertestdiscount", null);
        }
        #endregion 
       
        #region >>>> 根据ID获取详细信息 zhangwei
        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public Dictcustomertestdiscount GetDictcustomerdiscountById(double dictlabId)
        {
            //Dictcustomertestdiscount obj = null;
            //IList lst = this.selectIList("Dict.GetDictcustomerdiscounInfo", dictlabId);
            //if (lst.Count > 0)
            //    obj = (Dictcustomertestdiscount)lst[0];
            //return obj;
            return this.selectObj<Dictcustomertestdiscount>("Dict.GetDictcustomerdiscounInfo", dictlabId);
        }
        #endregion

        #region >>>> 获取体检单位总体价格详细信息 zhangwei
        /// <summary>
        /// 获取体检单位总体价格详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Dictcustomertestdiscount GetDictcustomerdiscountInfo(Dictcustomertestdiscount library)
        {
            //Dictcustomertestdiscount obj = null;
            //IList lst = this.selectIList("Dict.GetDictcustomerdiscounInfo", library);
            //if (lst.Count > 0)
            //    obj = (Dictcustomertestdiscount)lst[0];
            //return obj;
            return this.selectObj<Dictcustomertestdiscount>("Dict.GetDictcustomerdiscounInfo", library);
        }
        #endregion

        #region >>>> 新增编辑后保存 zhangwei
        ///<summary>
        ///新增编辑后保存
        ///</summary>
        ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictcustomerdiscount(Dictcustomertestdiscount library)
        {
            int nflag = 0;
            //新增
            if (library.Dictcustomerdiscountid == 0 || library.Dictcustomerdiscountid == null)
            {
                try
                {
                    library.Dictcustomerdiscountid = getSeqID("SEQ_DICTCUSTOMERTESTDISCOUNT");
                    insert("Dict.InsertDictcustomertestdiscount", library);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictcustomertestdiscount>(new Dictcustomertestdiscount(), library);
                    Dictcustomer dictcustomer = new DictCustomerService().GetDictCustomerById(Convert.ToDouble(library.Dictcustomerid));
                    AddMaintenanceLog("Dictcustomertestdiscount", int.Parse(library.Dictcustomerdiscountid.ToString()), logLst, "新增", dictcustomer.Customername.ToString(), library.Finalprice.ToString(), modulename);
                    CacheHelper.RemoveAllCache("daan.SelectDictcustomertestdiscountresult");
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
                    Dictcustomertestdiscount dictcustomer = GetDictcustomerdiscountById(Convert.ToDouble(library.Dictcustomerdiscountid));
                    nflag = update("Dict.UpdateDictcustomertestdiscount", library);
                    List<LogInfo> logLst = getLogInfo<Dictcustomertestdiscount>(dictcustomer, library);
                    Dictcustomer dictcustomerUpdate = new DictCustomerService().GetDictCustomerById(Convert.ToDouble(library.Dictcustomerid));
                    AddMaintenanceLog("Dictcustomertestdiscount", int.Parse(library.Dictcustomerdiscountid.ToString()), logLst, "修改", dictcustomerUpdate.Customername.ToString(), library.Finalprice.ToString(), modulename);
                    CacheHelper.RemoveAllCache("daan.SelectDictcustomertestdiscountresult");
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
        public int DelDictcustomerdiscountByID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictcustomertestdiscount> dictLibraryList = new List<Dictcustomertestdiscount>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictcustomerdiscountById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictcustomertestdiscount", strId);
                CacheHelper.RemoveAllCache("daan.SelectDictcustomertestdiscountresult");
                foreach (Dictcustomertestdiscount item in dictLibraryList)
                {
                    List<LogInfo> logLst = getLogInfo<Dictcustomertestdiscount>(item, new Dictcustomertestdiscount());
                    Dictcustomer dictcustomerDelete = new DictCustomerService().GetDictCustomerById(Convert.ToDouble(item.Dictcustomerid));
                    AddMaintenanceLog("Dictcustomertestdiscount", item.Dictcustomerdiscountid, logLst, "删除", dictcustomerDelete.Customername.ToString(), item.Finalprice.ToString(), modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag;
        }
        #endregion
  }
}
