using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using daan.domain;
using System.Data;
using System.Collections;
using daan.util.Common;

namespace daan.service.dict
{
    public class DicttestitemresultService : BaseService
    {
        protected const string modulename = "项目结果";

        #region >>>>删除项目结果 ylp
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="idstr"></param>
        /// <returns></returns>
        public bool DelDicttestitemResultByIdStr(List<Dicttestitemresult> itemresult)
        {
            SortedList sortedlist = new SortedList(new MySort());
            bool b = false;
            try
            {
                string idstr = "";
                foreach (Dicttestitemresult item in itemresult)
                {
                    idstr = idstr + item.Dicttestitemresultid + ",";
                }
                sortedlist.Add(new Hashtable { { "DELETE", "Dict.DeleteDicttestitemresultByIdStr" } }, idstr.TrimEnd(','));
                CacheHelper.RemoveAllCache("daan.GetDicttestitemresult");
                b = this.ExecuteSqlTran(sortedlist);
                if (b)
                {
                    foreach (Dicttestitemresult testitemresult in itemresult)
                    {
                        //日志 fhp
                        List<LogInfo> logLst = getLogInfo<Dicttestitemresult>(testitemresult, new Dicttestitemresult());
                        AddMaintenanceLog("Dicttestitemresult", testitemresult.Dicttestitemresultid, logLst, "删除", testitemresult.Result, null, modulename);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return b;
        }
        #endregion

        #region >>>>新增后保存 ylp
        /// <summary>
        /// 新增编辑后保存
        /// </summary>
        /// <param name="testitem"></param>
        /// <returns></returns>
        public bool SaveDictTestItemResult(Dicttestitemresult testitemresult)
        {
            int nflag = 0;

            //新增
            if (testitemresult.Dicttestitemid != null)
            {
                try
                {
                    testitemresult.Dicttestitemresultid = getSeqID("SEQ_DICTTESTITEMRESULT");
                    this.insert("Dict.InsertDicttestitemresult", testitemresult);
                    CacheHelper.RemoveAllCache("daan.GetDicttestitemresult");
                    nflag = 1;
                    //日志 fhp
                    List<LogInfo> logLst = getLogInfo < Dicttestitemresult >(new Dicttestitemresult(),testitemresult);
                    this.AddMaintenanceLog("Dicttestitemresult", testitemresult.Dicttestitemresultid, logLst, "添加", testitemresult.Result, null, modulename);
                }
                catch (Exception ex)
                {
                   
                    throw new Exception(ex.Message);
                }
            }

            return nflag > 0;
        }

        #endregion

        /// <summary>
        /// 根据项目得到项目结果
        /// </summary>
        /// <param name="dicttestitemid"></param>
        /// <returns></returns>
        public List<Dicttestitemresult> GetDicttestitemResult(double? dicttestitemid)
        {
            return this.QueryList<Dicttestitemresult>("Dict.SelectResultByDicttestitemid", dicttestitemid).ToList<Dicttestitemresult>();
        }
    }
}
