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
    public class DictLibraryItemService : BaseService
    {
        protected const string modulename = "基础字典明细维护";
        #region >>>基本查询
        /// <summary>
        /// 按条件获取，为空时取全部
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        public IList<Dictlibraryitem> GetDictLibraryItemLst(Dictlibraryitem library)
        {
            return this.QueryList<Dictlibraryitem>("Dict.SelectDictlibraryitemLst", library);
        }

        /// <summary>
        /// 获取基础字典详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Dictlibraryitem GetDictLibraryItemInfo(Dictlibraryitem library)
        {
            return this.selectObj<Dictlibraryitem>("Dict.SelectDictlibraryitemLst", library);        
        }
        #endregion

        #region >>>分页
        /// <summary>
        /// 获取基础字典明细列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<Dictlibraryitem> GetDictLibraryItemPageLst(Hashtable ht)
        {
            return this.QueryList<Dictlibraryitem>("Dict.GetDictLibraryItemPageLst", ht);
        }
        /// <summary>
        /// 获取基础字典明细列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictLibraryItemPageLstCount(Hashtable ht)
        {
            return int.Parse(this.selectDS("Dict.GetDictLibraryItemPageLstCount", ht).Tables[0].Rows[0]["pageCount"].ToString());
        }
        #endregion
        //根据dictlibrary中的CODE查询
        public IList SelectDictlibraryitemByCode(string code)
        {
            return  (this.selectIList("Dict.SelectDictlibraryitemByCode",code) as IList);
        }
        #region >>>新增编辑后保存
        /// <summary>
        /// 新增编辑后保存
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        public bool SaveDictLibraryItem(Dictlibraryitem library)
        {         
            int nflag = 0;
            //新增
            if (library.Dictlibraryitemid == 0)
            {
                try
                {
                    library.Dictlibraryitemid = getSeqID("Seq_DictLibraryItem");
                    insert("Dict.InsertDictlibraryitem", library);
                    nflag = 1;
                    //增加日志 fhp
                    List<LogInfo> logLst = getLogInfo<Dictlibraryitem>(new Dictlibraryitem(), library);
                    AddMaintenanceLog("DICTLIBRARYITEM", int.Parse(library.Dictlibraryitemid.ToString()), logLst, "新增", library.Itemname, library.Createdate.ToString(), modulename);
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
                    //获取旧对象id fhp
                    Dictlibraryitem dictlibraryitemLibrary = new Dictlibraryitem();
                    dictlibraryitemLibrary.Dictlibraryitemid = library.Dictlibraryitemid;
                    dictlibraryitemLibrary.Dictlibraryid = library.Dictlibraryid;
                    //根据id取出对象 fhp
                    IList<Dictlibraryitem> dictlibraryitemList = GetDictLibraryItemLst(dictlibraryitemLibrary);
                    Dictlibraryitem dictlibraryitem = (from Dictlibraryitem in dictlibraryitemList where Dictlibraryitem.Dictlibraryitemid == library.Dictlibraryitemid select Dictlibraryitem).ToList<Dictlibraryitem>()[0];
                    nflag = update("Dict.UpdateDictlibraryitem", library);
                    //写日志 fhp
                    List<LogInfo> logLst = getLogInfo<Dictlibraryitem>(dictlibraryitem, library);
                    AddMaintenanceLog("DICTLIBRARYITEM", int.Parse(library.Dictlibraryitemid.ToString()), logLst, "修改", library.Itemname, library.Createdate.ToString(), modulename);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            CacheHelper.RemoveAllCache("daan.SelectDictlibraryitemLst");
            return nflag > 0 ? true : false;
        }
        #endregion

        #region >>>删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int DelDictLibraryItemByID(string strId)
        {
            int nflag = 0;
            try
            {
                
                var arrayId = strId.Split(',');
                nflag = this.delete("Dict.DeleteDictlibraryitem", strId);
                //记录日志 fhp
                foreach (string id in arrayId)
                {
                    Dictlibraryitem dictlibraryitemLibrary = new Dictlibraryitem();
                    dictlibraryitemLibrary.Dictlibraryitemid = (Convert.ToDouble(id));
                    List<LogInfo> logLst = getLogInfo<Dictlibraryitem>(dictlibraryitemLibrary, new Dictlibraryitem());
                    AddMaintenanceLog("DICTLIBRARYITEM", int.Parse(dictlibraryitemLibrary.Dictlibraryitemid.ToString()), logLst, "删除", dictlibraryitemLibrary.Itemname, dictlibraryitemLibrary.Createdate.ToString(), modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            CacheHelper.RemoveAllCache("daan.SelectDictlibraryitemLst");
            return nflag;
        }
        #endregion
    }
}
