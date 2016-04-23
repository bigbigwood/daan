using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using System.Collections;
using daan.domain;
namespace daan.service.dict
{
    public class DictfastCommentService : BaseService
    {
        protected const string modulename = "快速录入模板维护";

        #region >>>> 分页获取模板列表 zhangwei
        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<Dictfastcomment> GetDictfastcommentPageLst(Hashtable ht)
        {
            return this.QueryList<Dictfastcomment>("Dict.GetDictfastcommentPageLst", ht);
        }
        #endregion

        #region >>>> 获取模板列表总项数 zhangwei
        /// <summary>
        /// 获取模板列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictfastcommentLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictfastcommentPageLstCount", ht).Tables[0].Rows[0][0]); 
        }
        #endregion

        #region >>>> 根据ID获取详细信息 zhangwei
        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public Dictfastcomment GetDictfastcommentById(double dictlabId)
        {
            //Dictfastcomment obj = null;
            //IList lst = this.selectIList("Dict.GetDictfastcommentInfo", dictlabId);
            //if (lst.Count > 0)
            //    obj = (Dictfastcomment)lst[0];
            //return obj;
            return this.selectObj<Dictfastcomment>("Dict.GetDictfastcommentInfo", dictlabId);
        }
        #endregion

        #region >>>> 获取模板详细信息 zhangwei
        /// <summary>
        /// 获取模块详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Dictfastcomment GetDictfastcommentInfo(Dictfastcomment dictFastComment)
        {
            //Dictfastcomment obj = null;
            //IList lst = this.selectIList("Dict.GetDictfastcommentInfo", dictFastComment);
            //if (lst.Count > 0)
            //    obj = (Dictfastcomment)lst[0];
            //return obj;
            return this.selectObj<Dictfastcomment>("Dict.GetDictfastcommentInfo", dictFastComment);
        }
        #endregion

        #region >>>> 新增编辑后保存 zhangwei
        ///<summary>
         ///新增编辑后保存
         ///</summary>
         ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictfastcomment(Dictfastcomment library)
        {
            int nflag = 0;
            //新增
            if (library.Dictfastcommentid == 0 || library.Dictfastcommentid == null)
            {
                try
                {
                    library.Dictfastcommentid = getSeqID("SEQ_DICTFASTCOMMENT");
                    insert("Dict.InsertDictfastcomment", library);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictfastcomment>(new Dictfastcomment(), library);
                    AddMaintenanceLog("Dictfastcomment", int.Parse(library.Dictfastcommentid.ToString()), logLst, "新增", library.Modulename,library.Fastcode,modulename);
                    CacheHelper.RemoveAllCache("daan.SelectDictfastcommentresult");
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
                    Dictfastcomment olddictfastcomment = GetDictfastcommentInfo(library);
                    nflag = update("Dict.UpdateDictfastcomment", library);
                    List<LogInfo> logLst = getLogInfo<Dictfastcomment>(olddictfastcomment,library);
                    AddMaintenanceLog("Dictfastcomment", int.Parse(library.Dictfastcommentid.ToString()), logLst, "修改", library.Modulename, library.Fastcode,modulename);
                    CacheHelper.RemoveAllCache("daan.SelectDictfastcommentresult");
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
                List<Dictfastcomment> dictLibraryList = new List<Dictfastcomment>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetDictfastcommentById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictfastcomment", strId);
                CacheHelper.RemoveAllCache("daan.SelectDictfastcommentresult");
                foreach (Dictfastcomment item in dictLibraryList)
                {
                    // Dictcustomer dictcustomerDelete = new DictCustomerService().GetDictCustomerById(Convert.ToDouble(item.Dictcustomerid));
                    //增加删除日志对象 fhp
                    List<LogInfo> logLst = getLogInfo<Dictfastcomment>(item, new Dictfastcomment());
                    AddMaintenanceLog("Dictfastcomment", item.Dictfastcommentid, logLst, "删除", item.Commentdesc, item.Modulename.ToString(), modulename);
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
