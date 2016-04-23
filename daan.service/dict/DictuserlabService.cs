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
    public class DictuserlabService :BaseService
    {
        protected const string modulename = "用户分点维护";

        #region >>>> 分页 用户分点列表  zhangwei
        /// <summary>
        /// 获取体检单位列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<Dictuserandlab> GetDictuserandlabPageLst(Hashtable ht)
        {
            return this.QueryList<Dictuserandlab>("Dict.GetDictuserandlabPageLst", ht);
        }
        #endregion

        #region   >>>> 获取用户分点列表总项数 zhangwei
        /// <summary>
        /// 获取体检单位列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictuserandlabPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictuserandlabPageLstCount", ht).Tables[0].Rows[0][0]); 
        }
        #endregion

        #region >>>> 获取所有用户分点列表 zhangwei
        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <returns></returns>
        public IList<Dictuserandlab> GetDictCustomerList()
        {
            return this.QueryList<Dictuserandlab>("Dict.SelectDictuserandlab", null);
        }
        #endregion

        #region >>>> 根据ID获取详细信息 zhangwei
        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public Dictuserandlab GetDictuserandlabById(double dictlabId)
        {
            //Dictuserandlab obj = null;
            //IList lst = this.selectIList("Dict.GetDictuserandlabInfo", dictlabId);
            //if (lst.Count > 0)
            //    obj = (Dictuserandlab)lst[0];
            //return obj;
            return this.selectObj<Dictuserandlab>("Dict.GetDictuserandlabInfo", dictlabId);
        }
        #endregion

        #region >>>> 获取用户分点详细信息 zhangwei
        /// <summary>
        /// 获取体检单位详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Dictuserandlab GetDictcustomeridInfo(Dictuserandlab library)
        {
            //Dictuserandlab obj = null;
            //IList lst = this.selectIList("Dict.GetDictuserandlabInfo", library);
            //if (lst.Count > 0)
            //    obj = (Dictuserandlab)lst[0];
            //return obj;
            return this.selectObj<Dictuserandlab>("Dict.GetDictuserandlabInfo", library);
        }
        #endregion

        #region >>>> 新增编辑后保存 zhangwei
        ///<summary>
        ///新增编辑后保存
        ///</summary>
        ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictuserandlab(Dictuserandlab library)
        {
            int nflag = 0;
            //新增
            if (library.Dictuserandlabid == 0 || library.Dictuserandlabid ==null)
            {
                try
                {
                    library.Dictuserandlabid = getSeqID("SEQ_DICTUSERANDLAB");
                    insert("Dict.InsertDictuserandlab", library);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictuserandlab>(new Dictuserandlab(), library);
                    Dictlab dictlab = new Dictlab();
                    dictlab.Dictlabid = library.Dictlabid;
                    dictlab = new DictlabService().GetDictlabInfo(dictlab);
                    AddMaintenanceLog("Dictuserandlab", int.Parse(library.Dictuserandlabid.ToString()), logLst, "新增", dictlab.Labname, library.Createdate.ToString(), modulename);
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
                    Dictuserandlab dictcustomer = GetDictcustomeridInfo(library);
                    nflag = update("Dict.UpdateDictuserandlab", library);
                    List<LogInfo> logLst = getLogInfo<Dictuserandlab>(dictcustomer, library);
                    Dictlab dictlab = new Dictlab();
                    dictlab.Dictlabid = library.Dictlabid;
                    dictlab = new DictlabService().GetDictlabInfo(dictlab);
                    AddMaintenanceLog("Dictuserandlab", int.Parse(library.Dictuserandlabid.ToString()), logLst, "修改", dictlab.Labname, library.Createdate.ToString(), modulename);
              
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
        /// 删除根据主键ID
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int DelDictuserandlabByID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictuserandlab> dictLibraryList = new List<Dictuserandlab>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictuserandlabById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictuserandlab", strId);
                foreach (Dictuserandlab item in dictLibraryList)
                {
                    Dictlab dictlab = new Dictlab();
                    dictlab.Dictlabid = item.Dictlabid;
                    dictlab = new DictlabService().GetDictlabInfo(dictlab);
                    AddMaintenanceLog("Dictuserandlab", item.Dictuserandlabid, null, "删除", dictlab.Labname, item.Createdate.ToString(), modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag;
        }


        /// <summary>
        /// 根据UserId
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public int DelDictuserandlabByUserID(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<Dictuserandlab> dictLibraryList = new List<Dictuserandlab>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictuserandlabById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictuserandlabByUserId", strId);
                foreach (Dictuserandlab item in dictLibraryList)
                {
                    Dictlab dictlab = new Dictlab();
                    dictlab.Dictlabid = item.Dictlabid;
                    dictlab = new DictlabService().GetDictlabInfo(dictlab);
                    AddMaintenanceLog("Dictuserandlab", item.Dictuserandlabid, null, "删除", dictlab.Labname, item.Createdate.ToString(), modulename);
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
