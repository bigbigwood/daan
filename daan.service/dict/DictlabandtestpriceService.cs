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
    public class DictlabandtestpriceService : BaseService
    {
        protected const string modulename = "分点测试项目价格维护";

        #region  >>>> 分页  获取分点测试项目价格列表 zhangwei

        /// <summary>
        /// 获取体检单位总体价格列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<Dictlabandtestprice> GetGetDictlabandtestpricePageLstPageLst(Hashtable ht)
        {
            return this.QueryList<Dictlabandtestprice>("Dict.GetDictlabandtestpricePageLst", ht);
        }

        #endregion

        #region >>>> 获取  分点测试项目价格列表总项数 zhangwei
        /// <summary>
        /// 获取体检单位总体价格列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetGetDictlabandtestpricePageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictlabandtestpricePageLstCount", ht).Tables[0].Rows[0][0]); 
        }
        #endregion

        #region >>>> 根据时间判断是否存在记录 zhangwei
        /// <summary>
        /// 根据时间判断是否存在记录
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Dictlabandtestprice> GetDictlabandtestpriceByTime(Hashtable ht)
        {
            return this.QueryList<Dictlabandtestprice>("Dict.GetDictlabandtestpriceByTime", ht);
        }
        public IList<Dictlabandtestprice> GetDictlabandtestpriceByTimeTo(Hashtable ht)
        {
            return this.QueryList<Dictlabandtestprice>("Dict.GetDictlabandtestpriceByTimeTo", ht);
        }
        #endregion

        #region >>>> 获取详细信息 zhangwei
        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <returns></returns>
        public IList<Dictlabandtestprice> GetDictlabandtestpriceList()
        {
            return this.QueryList<Dictlabandtestprice>("Dict.SelectDictlabandtestprice", null);
        }

        public IList<Dictlabandtestprice> GetDictlabandtestpriceByWhere(Hashtable ht)
        {
            return this.QueryList<Dictlabandtestprice>("Dict.SelectDictlabandtestpriceByWhere", ht);
        }

        #endregion

        #region >>>> 根据ID获取详细信息 zhangwei
        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public Dictlabandtestprice GetDictlabandtestpriceById(double dictlabId)
        {
            //Dictlabandtestprice obj = null;
            //IList lst = this.selectIList("Dict.GetDictlabandtestpriceInfo", dictlabId);
            //if (lst.Count > 0)
            //    obj = (Dictlabandtestprice)lst[0];
            //return obj;
            return this.selectObj<Dictlabandtestprice>("Dict.GetDictlabandtestpriceInfo", dictlabId);
        }
        #endregion

        #region >>>> 获取体检单位总体价格详细信息 zhangwei
        /// <summary>
        /// 获取体检单位总体价格详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Dictlabandtestprice GetDictlabandtestpriceInfo(Dictlabandtestprice library)
        {
            //Dictlabandtestprice obj = null;
            //IList lst = this.selectIList("Dict.SelectDictlabandtestprice", library);
            //if (lst.Count > 0)
            //    obj = (Dictlabandtestprice)lst[0];
            //return obj;
            return this.selectObj<Dictlabandtestprice>("Dict.SelectDictlabandtestprice", library);
        }
        #endregion

        #region >>>> 新增编辑后保存 zhangwei
        ///<summary>
        ///新增编辑后保存
        ///</summary>
        ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictlabandtestprice(Dictlabandtestprice library)
        { 
            int nflag = 0;
            //新增
            if (library.Dictlabandtestpriceid == 0 || library.Dictlabandtestpriceid == null)
            {
                try
                {
                    library.Dictlabandtestpriceid = getSeqID("SEQ_DICTLABANDTESTPRICE");
                    insert("Dict.InsertDictlabandtestprice", library);
                    CacheHelper.RemoveAllCache("daan.GetDictlabandtestpriceresult");
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictlabandtestprice>(new Dictlabandtestprice(), library);
                    Dictlab dictlab = new DictlabService().GetDictlabById(Convert.ToDouble(library.Dictlabid)); //查询分点                 
                    AddMaintenanceLog("Dictlabandtestprice", int.Parse(library.Dictlabandtestpriceid.ToString()), logLst, "新增", dictlab.Labname.ToString(), library.Price.ToString(), modulename);
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
                    Dictlabandtestprice dictlabandtestprice = GetDictlabandtestpriceInfo(library);
                    nflag = update("Dict.UpdateDictlabandtestprice", library);
                    CacheHelper.RemoveAllCache("daan.GetDictlabandtestpriceresult");
                    List<LogInfo> logLst = getLogInfo<Dictlabandtestprice>(dictlabandtestprice, library);
                    Dictlab dictlab = new DictlabService().GetDictlabById(Convert.ToDouble(library.Dictlabid)); //查询分点      
                    AddMaintenanceLog("Dictlabandtestprice", int.Parse(library.Dictlabandtestpriceid.ToString()), logLst, "修改", dictlab.Labname.ToString(), library.Price.ToString(), modulename);
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
        public int DelDictlabandtestpriceByID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictlabandtestprice> dictLibraryList = new List<Dictlabandtestprice>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictlabandtestpriceById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictlabandtestprice", strId);
                CacheHelper.RemoveAllCache("daan.GetDictlabandtestpriceresult");
                //记录日志
                foreach (Dictlabandtestprice item in dictLibraryList)
                {
                     //增加删除日志对象 fhp
                     List<LogInfo> logLst = getLogInfo<Dictlabandtestprice>(item, new Dictlabandtestprice());
                     Dictlab dictlab = new DictlabService().GetDictlabById(Convert.ToDouble(item.Dictlabid)); //查询分点
                     AddMaintenanceLog("Dictlabandtestprice", item.Dictlabandtestpriceid, logLst, "删除", dictlab.Labname, item.Price.ToString(), modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag;
        }


        public int DelDictlabandtestpriceByWhere(Hashtable ht)
        {
            int nflag = 0;
            try
            {
                nflag = this.delete("Dict.DeleteDictlabandtestpriceByWhere", ht);
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
