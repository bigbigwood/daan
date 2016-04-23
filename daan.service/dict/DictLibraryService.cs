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
    public class DictLibraryService : BaseService
    {
        protected const string modulename = "基础字典维护";

        #region >>>基本查询

        /// <summary>
        /// 获取基础字典详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Dictlibrary GetDictLibraryInfo(Dictlibrary library)
        {
            return this.selectObj<Dictlibrary>("Dict.GetDictLibraryLst", library);
        }     


        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public Dictlibrary GetDictLibraryInfoById(string libraryId)
        {
            Dictlibrary dictLibrary = new Dictlibrary();
            dictLibrary.Dictlibraryid =double.Parse(libraryId);
            return GetDictLibraryInfo(dictLibrary); 
        }
        /// <summary>
        /// 按条件获取，为空时取全部
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        public IList<Dictlibrary> GetDictLibraryLst(Dictlibrary library)
        {
            return this.QueryList<Dictlibrary>("Dict.GetDictLibraryLst", library);
        }           
        #endregion

        #region >>>分页
        /// <summary>
        /// 获取基础字典列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public List<Dictlibrary> GetDictLibraryPageLst(Hashtable ht)
        {
            return this.QueryList<Dictlibrary>("Dict.GetDictLibraryPageLst", ht).ToList<Dictlibrary>();
        }
        /// <summary>
        /// 获取基础字典列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictLibraryPageLstCount(Hashtable ht)
        {
            return int.Parse(this.selectDS("Dict.GetDictLibraryPageLstCount", ht).Tables[0].Rows[0]["pageCount"].ToString());            
        }
        #endregion

        #region >>>新增编辑后保存
        /// <summary>
        /// 新增编辑后保存
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        public bool SaveDictLibrary(Dictlibrary library)
        {
            int nflag = 0;
            //新增
            if (library.Dictlibraryid == 0)
            {
                try
                {
                    library.Dictlibraryid = getSeqID("Seq_DictLibrary");
                    insert("Dict.InsertDictLibrary", library);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictlibrary>(new Dictlibrary(), library);
                    AddMaintenanceLog("Dictlibrary", int.Parse(library.Dictlibraryid.ToString()), logLst, "新增", library.Libraryname, library.Librarycode, modulename);
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
                    Dictlibrary tempLibrary = new Dictlibrary();
                    tempLibrary.Dictlibraryid = library.Dictlibraryid;
                    Dictlibrary olddictlibrary = GetDictLibraryInfo(tempLibrary);

                    nflag = update("Dict.UpdateDictLibrary", library);
                    List<LogInfo> logLst = getLogInfo<Dictlibrary>(olddictlibrary, library);
                    AddMaintenanceLog("Dictlibrary", int.Parse(library.Dictlibraryid.ToString()), logLst, "修改", library.Libraryname, library.Librarycode, modulename);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            CacheHelper.RemoveAllCache("daan.GetDictLibraryLst");
            return nflag > 0;
        }
        #endregion

        #region >>>删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="usercode">由逗号组成的ID字符串</param>
        /// <returns></returns>
        public int DelDictLibraryByID(string usercode)
        {
            int nflag = 0;
            try
            {      
                var arrayId = usercode.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictlibrary> dictLibraryList = new List<Dictlibrary>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictLibraryInfoById(strid));                    
                }
                //删除
                nflag = this.delete("Dict.DelDictLibraryByID", usercode);
                //记录日志
                foreach (Dictlibrary item in dictLibraryList)
                {
                    AddMaintenanceLog("Dictlibrary", item.Dictlibraryid, null, "删除", item.Libraryname, item.Librarycode, modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            CacheHelper.RemoveAllCache("daan.GetDictLibraryLst");
            return nflag;
        }
        #endregion
    }
}
