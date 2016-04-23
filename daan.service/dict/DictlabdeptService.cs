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
    public class DictlabdeptService : BaseService
    {
        protected const string modulename = "物理实验室";

        #region >>>> 查询物理组(科室名称) ylp
        /// <summary>
        /// 查询物理组(科室名称)
        /// </summary>
        /// <returns></returns>
        public List<Dictlabdept> GetDictlabdept()
        {
            return this.QueryList<Dictlabdept>("Dict.SelectDictlabdept", null).ToList<Dictlabdept>();
        }
        #endregion

        #region >>>> 分页 获取实验室列表 zhangwei
        /// <summary>
        /// 获取实验室列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<Dictlabdept> GetDictlabdeptPageLst(Hashtable ht)
        {
            return this.QueryList<Dictlabdept>("Dict.GetDictlabdeptPageLst", ht);
        }
        #endregion

        #region >>>> 获取实验室列表总项数 zhangwei
        /// <summary>
        /// 获取实验室列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictlabdeptPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictlabdeptPageLstCount", ht).Tables[0].Rows[0][0]);
        }
        #endregion

        #region >>>> 分页 已选用户获取实验室列表 zhangwei
        /// <summary>
        /// 获取实验室列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<Dictlabdept> GetDictlabdeptPageLstUser(Hashtable ht)
        {
            return this.QueryList<Dictlabdept>("Dict.GetDictlabdeptPageLstUser", ht);
        }
        #endregion

        #region >>>> 获取 已选用户实验室列表总项数 zhangwei
        /// <summary>
        /// 获取实验室列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictlabdeptPageLstCountUser(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictlabdeptPageLstCountUser", ht).Tables[0].Rows[0][0]);
        }
        #endregion

        #region >>>> 获取实验室详细信息 zhangwei
        /// <summary>
        /// 获取实验室详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Dictlabdept GetDictlabdeptInfo(Dictlabdept library)
        {
            //Dictlabdept obj = null;
            //IList lst = this.selectIList("Dict.GetDictlabdeptInfo", library);
            //if (lst.Count > 0)
            //    obj = (Dictlabdept)lst[0];
            //return obj;
            return this.selectObj<Dictlabdept>("Dict.GetDictlabdeptInfo", library);
        }
        #endregion

        #region >>>> 新增编辑后保存 zhangwei
        ///<summary>
         ///新增编辑后保存
         ///</summary>
         ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictlabdept(Dictlabdept library)
        {
            int nflag = 0;
            //新增
            if (library.Dictlabdeptid == 0 || library.Dictlabdeptid == null)
            {
                try
                {
                    library.Dictlabdeptid = getSeqID("SEQ_DICTLABDEPT");
                    insert("Dict.InsertDictlabdept", library);
                    CacheHelper.RemoveAllCache("daan.GetDictlabandtest");
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictlabdept>(new Dictlabdept(), library);
                    AddMaintenanceLog("Dictlabdept", int.Parse(library.Dictlabdeptid.ToString()), logLst, "新增", library.Labdeptname, library.Createdate.ToString(), modulename);
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
                    Dictlabdept olddictfastcomment = GetDictlabdeptInfo(library);
                    nflag = update("Dict.UpdateDictlabdept", library);
                  
                    List<LogInfo> logLst = getLogInfo<Dictlabdept>(olddictfastcomment, library);
                    AddMaintenanceLog("Dictlabdept", int.Parse(library.Dictlabdeptid.ToString()), logLst, "修改", library.Labdeptname, library.Createdate.ToString(), modulename);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            CacheHelper.RemoveAllCache("daan.SelectDictlabdeptLst");
            return nflag > 0 ;
        }
        #endregion

        #region >>>> 删除 zhangwei
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int DelDictlabdeptByID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictlabdept> dictLibraryList = new List<Dictlabdept>();
                foreach (string strid in arrayId)
                {
                    Dictlabdept dictlabdept = new Dictlabdept();
                    dictlabdept.Dictlabdeptid = Convert.ToDouble(strid);
                    dictLibraryList.Add(GetDictlabdeptInfo(dictlabdept));
                }
                nflag = this.delete("Dict.DeleteDictlabdept", strId);
             
                //记录日志
                foreach (Dictlabdept item in dictLibraryList)
                {
                    //增加删除日志对象 fhp
                    List<LogInfo> logLst = getLogInfo<Dictlabdept>(item, new Dictlabdept());
                    AddMaintenanceLog("Dictlabdept", item.Dictlabdeptid, logLst, "删除", item.Labdeptname, item.Createdate.ToString(), modulename);
                }  
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            CacheHelper.RemoveAllCache("daan.SelectDictlabdeptLst");
            return nflag;
        }
        
        #endregion
    }
}
