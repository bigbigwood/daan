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
    public class DictlabandtestService : BaseService
    {
        protected const string modulename = "分点试验室检测项目";

        #region >>>> 分页 获取分点测试项目列表 zhangwei
        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<Dictlabandtest> GetDictlabandtestPageLst(Hashtable ht)
        {
            return this.QueryList<Dictlabandtest>("Dict.GetDictlabandtestPageLst", ht);
        }
        #endregion

        public List<Dictlabandtest> GetDictlabandtestByDictlabId(Hashtable ht)
        {
            return this.QueryList<Dictlabandtest>("Dict.GetDictlabandtestByDictlabId", ht).ToList();

        }
        /// <summary>
        /// 根据分点ID查询所有该分点下面可用的测试项
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dictlabandtest> GetTestItem(Hashtable ht)
        {
            return this.QueryList<Dictlabandtest>("Dict.SelectDictlabandtestByDictlabId", ht).ToList();
        }

        #region >>>> 获取分点测试项目列表总项数 zhangwei
        /// <summary>
        /// 获取模板列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictlabandtestPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictlabandtestPageLstCount", ht).Tables[0].Rows[0][0]); 
        }

        public int GetDictlabandtestPageLstCountBywhere(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictlabandtestPageLstCountBywhere", ht).Tables[0].Rows[0][0]); 
        }
        #endregion

        #region >>>> 根据ID获取详细信息 zhangwei
        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public Dictlabandtest GetDictDictlabandtestById(double dictlabId)
        {
            //Dictlabandtest obj = null;
            //IList lst = this.selectIList("Dict.SelectDictlabandtestById", dictlabId);
            //if (lst.Count > 0)
            //    obj = (Dictlabandtest)lst[0];
            //return obj;
            return this.selectObj<Dictlabandtest>("Dict.SelectDictlabandtestById", dictlabId);
        }
        #endregion

        #region >>>> 获取分点测试项目详细信息 zhangwei
        /// <summary>
        /// 获取模块详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Dictlabandtest GetDictlabandtestInfo(Dictlabandtest dictFastComment)
        {
            //Dictlabandtest obj = null;
            //IList lst = this.selectIList("Dict.SelectDictlabandtestById", dictFastComment);
            //if (lst.Count > 0)
            //    obj = (Dictlabandtest)lst[0];
            //return obj;
            return this.selectObj<Dictlabandtest>("Dict.SelectDictlabandtestById", dictFastComment);
        }
        #endregion

        #region >>>> 新增编辑后保存 zhangwei
        ///<summary>
        ///新增编辑后保存
        ///</summary>
        ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictlabandtest(Dictlabandtest dictlabandtest)
        {
            int nflag = 0;
            //新增
            if (dictlabandtest.Dictlabandtestid == 0 || dictlabandtest.Dictlabandtestid == null)
            {
                try
                {
                    dictlabandtest.Dictlabandtestid = getSeqID("SEQ_DICTLABANDTEST");
                    insert("Dict.InsertDictlabandtest", dictlabandtest);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictlabandtest>(new Dictlabandtest(), dictlabandtest);
                    Dictlab dictlab = new DictlabService().GetDictlabById(Convert.ToDouble(dictlabandtest.Dictlabid)); //查询分点 
                    AddMaintenanceLog("Dictlabandtest", int.Parse(dictlabandtest.Dictlabandtestid.ToString()), logLst, "新增", dictlab.Labname, dictlabandtest.Createdate.ToString(), modulename);
                    CacheHelper.RemoveAllCache("daan.GetDictlabandtest");
                    CacheHelper.RemoveAllCache("daan.GetDicttestitemNotDictlabandtest");
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
                    Dictlabandtest oldDictlabandtest = GetDictlabandtestInfo(dictlabandtest);

                    dictlabandtest.Createdate = oldDictlabandtest.Createdate;
                    dictlabandtest.Dictlabid = oldDictlabandtest.Dictlabid;
                    dictlabandtest.Dicttestitemid = oldDictlabandtest.Dicttestitemid;
                    nflag = update("Dict.UpdateDictlabandtest", dictlabandtest);

                    List<LogInfo> logLst = getLogInfo<Dictlabandtest>(oldDictlabandtest, dictlabandtest);
                    Dictlab dictlab = new DictlabService().GetDictlabById(Convert.ToDouble(dictlabandtest.Dictlabid)); //查询分点 
                    AddMaintenanceLog("Dictlabandtest", int.Parse(dictlabandtest.Dictlabandtestid.ToString()), logLst, "修改", dictlabandtest.Labname, dictlabandtest.Createdate.ToString(), modulename);
                    CacheHelper.RemoveAllCache("daan.GetDictlabandtest");
                    CacheHelper.RemoveAllCache("daan.GetDicttestitemNotDictlabandtest");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return nflag > 0;
        }
        #endregion

        #region >>>> 删除 zhangwei
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int DelDictfastcommentByID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictlabandtest> dictLibraryList = new List<Dictlabandtest>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictDictlabandtestById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictlabandtest", strId);
                CacheHelper.RemoveAllCache("daan.GetDictlabandtest");
                CacheHelper.RemoveAllCache("daan.GetDicttestitemNotDictlabandtest");
                //记录日志
                foreach (Dictlabandtest item in dictLibraryList)
                {
                    Dictlab dictlab = new DictlabService().GetDictlabById(Convert.ToDouble(item.Dictlabid)); //查询分点
                    AddMaintenanceLog("Dictlabandtest", item.Dictlabandtestid, null, "删除", dictlab.Labname, item.Createdate.ToString(), modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag;
        }
        #endregion

        /// <summary>
        /// 批量插入测试项
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool BatchInsert(Hashtable ht)
        {
            int nflag = 0;
            try
            {  
                object i = insert("Dict.batchInsertDictlabandtest", ht);
                CacheHelper.RemoveAllCache("daan.GetDictlabandtest");
                CacheHelper.RemoveAllCache("daan.GetDicttestitemNotDictlabandtest");
                nflag = 1;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return nflag > 0;
        }
    }
}
