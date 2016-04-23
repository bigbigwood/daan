using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using System.Collections;
using daan.domain;

namespace daan.service.dict
{
    public class DictSubCompanyService : BaseService
    {

        protected const string modulename = "子公司维护";
        #region >>>> 分页 
        /// <summary>
        /// 获取子公司列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<DictSubCompany> GetDictlabdeptPageLst(Hashtable ht)
        {
            return this.QueryList<DictSubCompany>("Dict.GetDictSubcompanyPageLst", ht);
        }/// <summary>
        /// 获取子公司列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetDictlabdeptPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictSubcompanyLstCount", ht).Tables[0].Rows[0][0]);
        }
        #endregion

        #region >>>> 查询所有子公司信息
        public IList<DictSubCompany> GetDictSubCompanyList(string strKey)
        {
            return QueryList<DictSubCompany>("Dict.SelectDictSubcompany", strKey);
        }
        #endregion

        #region >>>> 根据ID获取详细信息 zhangwei
        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public DictSubCompany GetSubcompanyIdById(double subcompanyId)
        {
            return this.selectObj<DictSubCompany>("Dict.GetDictSubcompanyInfo", subcompanyId);
        }
        #endregion

        #region >>>> 根据ID获取详细信息 zhangwei
        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="dictlabId"></param>
        /// <returns></returns>
        public DictSubCompany GetDictSubcompanyById(DictSubCompany subcompany)
        {
            return this.selectObj<DictSubCompany>("Dict.GetDictSubcompanyInfo", subcompany);
        }
        #endregion


        #region >>>> 新增编辑后保存 zhangwei
        ///<summary>
        ///新增编辑后保存
        ///</summary>
        ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictSubCompany(DictSubCompany library)
        {
            int nflag = 0;
            //新增
            if (library.SubCompanyId == 0 || library.SubCompanyId == null)
            {
                try
                {
                    library.SubCompanyId = getSeqID("SEQ_SUBCOMPANY");
                    insert("Dict.InsertDictSubcompany", library);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<DictSubCompany>(new DictSubCompany(), library);
                    AddMaintenanceLog("SubCompany", int.Parse(library.SubCompanyId.ToString()), logLst, "新增", library.SubCompanyName, library.Addres, modulename);
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
                    DictSubCompany dictlab = GetDictSubcompanyById(library);
                    nflag = update("Dict.UpdateDictSubcompany", library);
                    List<LogInfo> logLst = getLogInfo<DictSubCompany>(dictlab, library);
                    AddMaintenanceLog("SubCompany", int.Parse(library.SubCompanyId.ToString()), logLst, "修改", library.SubCompanyName, library.Addres, modulename);
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
        /// 删除
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int DelSubcompanyById(string strId)
        {
            int nflag = 0;
            try
            {
                var arrayId = strId.Split(',');
                //临时存储待删除对象，备写日志用
                List<DictSubCompany> dictLibraryList = new List<DictSubCompany>();
                foreach (string strid in arrayId)
                {
                    dictLibraryList.Add(GetSubcompanyIdById(Convert.ToDouble(strid)));
                }
                nflag = this.delete("Dict.DeleteDictSubcompany", strId);
                //记录日志
                foreach (DictSubCompany item in dictLibraryList)
                {
                    //增加删除日志对象 fhp
                    List<LogInfo> logLst = getLogInfo<DictSubCompany>(item, new DictSubCompany());
                    AddMaintenanceLog("Dictlab", item.SubCompanyId, logLst, "删除", item.SubCompanyName, item.Addres, modulename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag;
        }

    }
}
