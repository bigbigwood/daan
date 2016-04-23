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
    public class DictlabService : BaseService
    {
        protected const string modulename = "分点资料维护";

        #region >>>> 分页获取分点列表 zhangwei
        /// <summary>
        /// 获取分点列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<Dictlab> GetDictlabPageLst(Hashtable ht)
        {
            return this.QueryList<Dictlab>("Dict.GetDictlabPageLst", ht);
        }
        #endregion

        #region >>>> 获取分点列表总项数 zhangwei
        /// <summary>
        /// 获取分点列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictlabPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictlabPageLstCount", ht).Tables[0].Rows[0][0]); 
        }
        #endregion



        #region >>>> 分页 未选用户获取分点列表 zhangwei
        /// <summary>
        /// 获取分点列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<Dictlab> GetDictlabPageLstUser(Hashtable ht)
        {
            return this.QueryList<Dictlab>("Dict.GetDictlabPageLstUser", ht);
        }
        #endregion

        #region >>>>  未选用户获取分点列表总项数 zhangwei
        /// <summary>
        /// 获取分点列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictlabPageLstCountUser(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictlabPageLstCountUser", ht).Tables[0].Rows[0][0]);
        }
        #endregion

        #region >>>> 获取全部分点信息 zhangwei
        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<Dictlab> GetDictlabList()
        {
            return this.QueryList<Dictlab>("Dict.SelectDictlab", null).ToList<Dictlab>();
        }
        #endregion

        #region >>>> 根据ID获取详细信息 zhangwei
        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public Dictlab GetDictlabById(double dictlabId)
        {
            //Dictlab obj = null;
            //IList lst = this.selectIList("Dict.GetDictlabInfo", dictlabId);
            //if (lst.Count > 0)
            //    obj = (Dictlab)lst[0];
            //return obj;
            return this.selectObj<Dictlab>("Dict.GetDictlabInfo", dictlabId);
        }
        #endregion

        #region >>>> 获取分点详细信息 zhangwei
        /// <summary>
        /// 获取分点详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Dictlab GetDictlabInfo(Dictlab library)
        {
            //Dictlab obj = null;
            //IList lst = this.selectIList("Dict.GetDictlabInfo", library);
            //if (lst.Count > 0)
            //    obj = (Dictlab)lst[0];
            //return obj;
            return this.selectObj<Dictlab>("Dict.GetDictlabInfo", library);
        }
        #endregion

        #region >>>> 新增编辑后保存 zhangwei
        ///<summary>
         ///新增编辑后保存
         ///</summary>
         ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictlab(Dictlab library)
        {  
            int nflag = 0;
            //新增
            if (library.Dictlabid == 0 || library.Dictlabid == null)
            {
                try
                {
                    library.Dictlabid = getSeqID("SEQ_DICTLAB");
                    insert("Dict.InsertDictlab", library);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictlab>(new Dictlab(), library);
                    AddMaintenanceLog("Dictlab", int.Parse(library.Dictlabid.ToString()), logLst, "新增", library.Labname, library.Addres,modulename);
                    CacheHelper.RemoveAllCache("daan.GetDictlab");
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
                    Dictlab dictlab = GetDictlabInfo(library);
                    nflag = update("Dict.UpdateDictlab", library);
                    List<LogInfo> logLst = getLogInfo<Dictlab>(dictlab, library);
                    AddMaintenanceLog("Dictlab", int.Parse(library.Dictlabid.ToString()), logLst, "修改", library.Labname, library.Addres, modulename);
                    CacheHelper.RemoveAllCache("daan.GetDictlab");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            CacheHelper.RemoveAllCache("daan.GetDictlab");
            return nflag > 0 ;
        }
        #endregion

        #region >>>> 删除 zhangwei
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int DelDictlabByID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictlab> dictLibraryList = new List<Dictlab>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictlabById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictlab", strId);
                CacheHelper.RemoveAllCache("daan.GetDictlab");
                //记录日志
                foreach (Dictlab item in dictLibraryList)
                {
                    //增加删除日志对象 fhp
                    List<LogInfo> logLst = getLogInfo<Dictlab>(item, new Dictlab());
                    AddMaintenanceLog("Dictlab", item.Dictlabid, logLst, "删除", item.Labname, item.Addres, modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            CacheHelper.RemoveAllCache("daan.GetDictlab");
            return nflag;
        }
        #endregion
    }   
}
