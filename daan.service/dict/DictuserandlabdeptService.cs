using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using daan.service.common;
using daan.domain;

namespace daan.service.dict
{
  public  class DictuserandlabdeptService : BaseService
    {

      protected const string modulename = "用户物理组维护";

        #region >>>> 分页 用户物理组列表  zhangwei
        /// <summary>
        /// 获取体检单位列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
      public IList<Dictuserandlabdept> GetDictuserandlabdeptPageLst(Hashtable ht)
        {
            return this.QueryList<Dictuserandlabdept>("Dict.GetDictuserandlabdeptPageLst", ht);
        }
        #endregion

        #region   >>>> 获取用户物理组列表总项数 zhangwei
        /// <summary>
        /// 获取体检单位列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictuserandlabdeptPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictuserandlabdeptPageLstCount", ht).Tables[0].Rows[0][0]); 
        }
        #endregion

        #region >>>> 获取所有用户物理组列表 zhangwei
        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <returns></returns>
        public IList<Dictuserandlabdept> GetDictuserandlabdeptList()
        {
            return this.QueryList<Dictuserandlabdept>("Dict.SelectDictuserandlabdept", null);
        }
        #endregion

        #region >>>> 根据ID获取详细信息 zhangwei
        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public Dictuserandlabdept GetDictuserandlabdeptById(double dictlabId)
        {
            //Dictuserandlabdept obj = null;
            //IList lst = this.selectIList("Dict.GetDictuserandlabdeptInfo", dictlabId);
            //if (lst.Count > 0)
            //    obj = (Dictuserandlabdept)lst[0];
            //return obj;
            return this.selectObj<Dictuserandlabdept>("Dict.GetDictuserandlabdeptInfo", dictlabId);
        }
        #endregion

        #region >>>> 获取用户物理组详细信息 zhangwei
        /// <summary>
        /// 获取体检单位详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Dictuserandlabdept GetDictuserandlabdeptInfo(Dictuserandlabdept library)
        {
            //Dictuserandlabdept obj = null;
            //IList lst = this.selectIList("Dict.GetDictuserandlabdeptInfo", library);
            //if (lst.Count > 0)
            //    obj = (Dictuserandlabdept)lst[0];
            //return obj;
            return this.selectObj<Dictuserandlabdept>("Dict.GetDictuserandlabdeptInfo", library);
        }
        #endregion

        #region >>>> 新增编辑后保存 zhangwei
        ///<summary>
        ///新增编辑后保存
        ///</summary>
        ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictuserandlabdept(Dictuserandlabdept library)
        {
            int nflag = 0;
            //新增
            if (library.Dictuserandlabdeptid == 0 || library.Dictuserandlabdeptid == null)
            {
                try
                {
                    library.Dictuserandlabdeptid = getSeqID("SEQ_DICTUSERANDLABDEPT");
                    insert("Dict.InsertDictuserandlabdept", library);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictuserandlabdept>(new Dictuserandlabdept(), library);
                    Dictlabdept dictlabdep = new Dictlabdept();
                    dictlabdep.Dictlabdeptid = library.Dictlabdeptid;
                    dictlabdep = new DictlabdeptService().GetDictlabdeptInfo(dictlabdep);
                    AddMaintenanceLog("Dictuserandlabdept", int.Parse(library.Dictuserandlabdeptid.ToString()), logLst, "新增", dictlabdep.Labdeptname, library.Createdate.ToString(), modulename);
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
                    Dictuserandlabdept dictcustomer = GetDictuserandlabdeptInfo(library);
                    nflag = update("Dict.UpdateDictuserandlabdept", library);
                    List<LogInfo> logLst = getLogInfo<Dictuserandlabdept>(dictcustomer, library);
                    Dictlabdept dictlabdep = new Dictlabdept();
                    dictlabdep.Dictlabdeptid = library.Dictlabdeptid;
                    dictlabdep = new DictlabdeptService().GetDictlabdeptInfo(dictlabdep);
                    AddMaintenanceLog("Dictuserandlabdept", int.Parse(library.Dictuserandlabdeptid.ToString()), logLst, "修改", dictlabdep.Labdeptname, library.Createdate.ToString(), modulename);
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
        public int DelDictuserandlabdeptByID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictuserandlabdept> dictLibraryList = new List<Dictuserandlabdept>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictuserandlabdeptById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictuserandlabdept", strId);
                foreach (Dictuserandlabdept item in dictLibraryList)
                {
                    Dictlabdept dictlabdep = new Dictlabdept();
                    dictlabdep.Dictlabdeptid = item.Dictlabdeptid;
                    dictlabdep = new DictlabdeptService().GetDictlabdeptInfo(dictlabdep);
                    AddMaintenanceLog("Dictuserandlabdept", item.Dictuserandlabdeptid, null, "删除", dictlabdep.Labdeptname, item.Createdate.ToString(), modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag;
        }


        public int DelDictuserandlabdeptByUserID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictuserandlabdept> dictLibraryList = new List<Dictuserandlabdept>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictuserandlabdeptById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictuserandlabdeptByUserId", strId);
                foreach (Dictuserandlabdept item in dictLibraryList)
                {
                    Dictlabdept dictlabdep = new Dictlabdept();
                    dictlabdep.Dictlabdeptid = item.Dictlabdeptid;
                    dictlabdep = new DictlabdeptService().GetDictlabdeptInfo(dictlabdep);
                    AddMaintenanceLog("Dictuserandlabdept", item.Dictuserandlabdeptid, null, "删除", dictlabdep.Labdeptname, item.Createdate.ToString(), modulename);
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
