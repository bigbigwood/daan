using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using daan.service.common;
using daan.domain;
using hlis.service.common;
using daan.service.dict;

namespace daan.service.login
{
    public class LoginService : BaseService
    {
        #region >>>> 获取登录用户信息
        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public DataSet GetLoginUserInfo1()
        {
            if (CacheHelper.GetCache("daan.GetLoginUserInfo") != null)
                return (DataSet)CacheHelper.GetCache("daan.GetLoginUserInfo");
            else
            {
                DataSet ds = this.selectDS("daan.GetLoginUserInfo", null);
                CacheHelper.SetCache("daan.GetLoginUserInfo", ds);
                return ds;
            }

        }
        #endregion

        #region >>>> zhouy 获取项目字典【return List〈Dicttestitem〉】
        /// <summary>
        /// 获取项目字典【return List〈Dicttestitem〉】
        /// </summary>
        public List<Dicttestitem> GetLoginDicttestitemList()
        {
            //  return ScriptHelper.datatableToLstT<Dicttestitem>(GetLoginDicttestitemTable());
            if (CacheHelper.GetCache("daan.GetDicttestitem") != null)
                return (List<Dicttestitem>)CacheHelper.GetCache("daan.GetDicttestitem");
            else
            {
                List<Dicttestitem> ds = this.QueryList<Dicttestitem>("Dict.SelectDicttestitem", null).ToList<Dicttestitem>();
                CacheHelper.SetCache("daan.GetDicttestitem", ds);
                return ds;
            }
        }

        public List<Dicttestitem> GetLoginDicttestitemListNoCache()
        {
            List<Dicttestitem> ds = this.QueryList<Dicttestitem>("Dict.SelectDicttestitem", null).ToList<Dicttestitem>();
            return ds;
        }
        #endregion

        #region >>>> 获取项目套餐明细字典

        /// <summary>
        /// 获取项目套餐明细字典
        /// </summary>
        public List<Dictproductdetail> GetLoginDictproductdetail()
        {
            if (CacheHelper.GetCache("daan.GetDictproductdetail") != null)
                return (List<Dictproductdetail>)CacheHelper.GetCache("daan.GetDictproductdetail");
            else
            {
                List<Dictproductdetail> ds = this.QueryList<Dictproductdetail>("Dict.SelectDictproductdetail", null).ToList<Dictproductdetail>();
                CacheHelper.SetCache("daan.GetDictproductdetail", ds);
                return ds;
            }
        }

        public List<Dictproductdetail> GetLoginDictproductdetailNoCache()
        {
            List<Dictproductdetail> ds = this.QueryList<Dictproductdetail>("Dict.SelectDictproductdetail", null).ToList<Dictproductdetail>();
            return ds;
        }

        /// <summary>
        /// 根据套餐ID获取套餐明细
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<Dictproductdetail> GetDictproductdetailByProductID(double? pid)
        {
            List<Dictproductdetail> ds = this.QueryList<Dictproductdetail>("Dict.SelectDictproductdetailByProductID", pid).ToList<Dictproductdetail>();
            return ds;
        }
        #endregion

        #region >>>> zhouy 获取项目组合明细字典

        /// <summary>
        /// 获取项目组合明细字典
        /// </summary>
        public List<Dicttestgroupdetail> GetLoginDicttestgroupdetail()
        {
            if (CacheHelper.GetCache("daan.GetDicttestgroupdetail") != null)
                return (List<Dicttestgroupdetail>)CacheHelper.GetCache("daan.GetDicttestgroupdetail");
            else
            {
                List<Dicttestgroupdetail> ds = this.QueryList<Dicttestgroupdetail>("Dict.SelectDicttestgroupdetail", null).ToList<Dicttestgroupdetail>();
                CacheHelper.SetCache("daan.GetDicttestgroupdetail", ds);
                return ds;
            }
        }

        public List<Dicttestgroupdetail> GetLoginDicttestgroupdetailNoCache()
        {
            List<Dicttestgroupdetail> ds = this.QueryList<Dicttestgroupdetail>("Dict.SelectDicttestgroupdetail", null).ToList<Dicttestgroupdetail>();
            return ds;
        }
        /// <summary>
        /// 根据组合ID获取组合明细
        /// </summary>
        /// <param name="testgroupid"></param>
        /// <returns></returns>
        public List<Dicttestgroupdetail> GetDicttestgroupdetailByGroupID(double? testgroupid)
        {
            return QueryList<Dicttestgroupdetail>("Dict.SetlectDicttestgroupdetailByGroupID", testgroupid).ToList<Dicttestgroupdetail>();
        }

        #endregion

        #region >>>> zhouy 获取分点项目外包字典

        /// <summary>
        /// 获取分点项目外包字典
        /// </summary>
        public List<Dictlabandtest> GetLoginDictlabandtest()
        {
            if (CacheHelper.GetCache("daan.GetDictlabandtest") != null)
                return (List<Dictlabandtest>)CacheHelper.GetCache("daan.GetDictlabandtest");
            else
            {
                List<Dictlabandtest> ds = this.QueryList<Dictlabandtest>("Dict.SelectDictlabandtest", null).ToList<Dictlabandtest>();
                CacheHelper.SetCache("daan.GetDictlabandtest", ds);
                return ds;
            }
        }

        #endregion

        #region  >>>> 获取项目结果 ylp
        /// <summary>
        /// 获取项目结果【return List〈Dicttestitemresult〉】
        /// </summary>
        public List<Dicttestitemresult> GetLoginDicttestitemresultList()
        {
            if (CacheHelper.GetCache("daan.GetDicttestitemresult") != null)
                return (List<Dicttestitemresult>)CacheHelper.GetCache("daan.GetDicttestitemresult");
            else
            {
                List<Dicttestitemresult> list = this.QueryList<Dicttestitemresult>("Dict.SelectDicttestitemresult", null).ToList<Dicttestitemresult>();
                CacheHelper.SetCache("daan.GetDicttestitemresult", list);
                return list;
            }
        }
        #endregion

        #region >>>> 诊断信息 ylp
        /// <summary>
        /// 诊断信息【return List〈Dictdiagnosis〉】
        /// </summary>
        /// <returns></returns>
        public List<Dictdiagnosis> GetLoginDictdiagnosisresultList()
        {
            if (CacheHelper.GetCache("daan.GetDictdiagnosisresult") != null)
                return (List<Dictdiagnosis>)CacheHelper.GetCache("daan.GetDictdiagnosisresult");
            else
            {
                List<Dictdiagnosis> list = this.QueryList<Dictdiagnosis>("Dict.SelectDictdiagnosis", null).ToList<Dictdiagnosis>();
                CacheHelper.SetCache("daan.GetDictdiagnosisresult", list);
                return list;
            }
        }
        #endregion

        #region >>>> 诊断建议规则公式 ylp
        /// <summary>
        /// 诊断建议规则公式【return List〈Dictruleformular〉】
        /// </summary>
        /// <returns></returns>
        public List<Dictruleformular> GetLoginDictruleformularresultList()
        {
            if (CacheHelper.GetCache("daan.GetDictruleformularresult") != null)
                return (List<Dictruleformular>)CacheHelper.GetCache("daan.GetDictruleformularresult");
            else
            {
                List<Dictruleformular> list = this.QueryList<Dictruleformular>("Dict.SelectDictruleformular", null).ToList<Dictruleformular>();
                CacheHelper.SetCache("daan.GetDictruleformularresult", list);
                return list;
            }
        }
        #endregion

        #region >>>> 获取分点项目价格字典 ylp
        /// <summary>
        /// 获取分点项目价格字典【return List〈Dictlabandtestprice〉】
        /// </summary>
        /// <returns></returns>
        public List<Dictlabandtestprice> GetLoginDictlabandtestpriceList()
        {
            if (CacheHelper.GetCache("daan.GetDictlabandtestpriceresult") != null)
                return (List<Dictlabandtestprice>)CacheHelper.GetCache("daan.GetDictlabandtestpriceresult");
            else
            {
                List<Dictlabandtestprice> list = this.QueryList<Dictlabandtestprice>("Dict.SelectDictlabandtestprice", null).ToList<Dictlabandtestprice>();
                CacheHelper.SetCache("daan.GetDictlabandtestpriceresult", list);
                return list;
            }
        }

        #region >>>> 查询所有用户  zhangwei
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        public List<Dictuser> GetDictuser()
        {
            if (CacheHelper.GetCache("daan.SelectDictuseresult") != null)
                return (List<Dictuser>)CacheHelper.GetCache("daan.SelectDictuseresult");
            else
            {
                List<Dictuser> list = this.QueryList<Dictuser>("Dict.SelectDictuser", null).ToList<Dictuser>();
                CacheHelper.SetCache("daan.SelectDictuseresult", list);
                return list;
            }
            //return this.selectDS("Dict.SelectDictuser", null);
        }
        #endregion

        #region  >>>> 查询获取体检单位总体折扣率列表
        /// <summary>
        /// 查询获取体检单位总体折扣率列表
        /// </summary>
        /// <returns></returns>
        public List<Dictcustomerdiscounted> GetDictcustomerdiscounted()
        {
            if (CacheHelper.GetCache("daan.SelectDictcustomerdiscountedresult") != null)
                return (List<Dictcustomerdiscounted>)CacheHelper.GetCache("daan.SelectDictcustomerdiscountedresult");
            else
            {
                List<Dictcustomerdiscounted> list = this.QueryList<Dictcustomerdiscounted>("Dict.SelectDictcustomerdiscounted", null).ToList<Dictcustomerdiscounted>();
                CacheHelper.SetCache("daan.SelectDictcustomerdiscountedresult", list);
                return list;
            }
        }
        #endregion

        #region  >>>> 查询获取外包单位总体价格列表
        /// <summary>
        /// 查询获取外包单位总体价格列表
        /// </summary>
        /// <returns></returns>
        public List<Dictcustomertestdiscount> GetDictcustomerdiscount()
        {
            if (CacheHelper.GetCache("daan.SelectDictcustomertestdiscountresult") != null)
                return (List<Dictcustomertestdiscount>)CacheHelper.GetCache("daan.SelectDictcustomertestdiscountresult");
            else
            {
                List<Dictcustomertestdiscount> list = this.QueryList<Dictcustomertestdiscount>("Dict.SelectDictcustomertestdiscount", null).ToList<Dictcustomertestdiscount>();
                CacheHelper.SetCache("daan.SelectDictcustomertestdiscountresult", list);
                return list;
            }
        }
        #endregion

        #region >>>> 查询所有模板
        /// <summary>
        /// 查询所有模板
        /// </summary>
        /// <returns></returns>
        public List<Dictfastcomment> GetDictfastcomment()
        {
            if (CacheHelper.GetCache("daan.SelectDictfastcommentresult") != null)
                return (List<Dictfastcomment>)CacheHelper.GetCache("daan.SelectDictfastcommentresult");
            else
            {
                List<Dictfastcomment> list = this.QueryList<Dictfastcomment>("Dict.SelectDictfastcomment", null).ToList<Dictfastcomment>();
                CacheHelper.SetCache("daan.SelectDictfastcommentresult", list);
                return list;
            }
        }
        #endregion

        #endregion

        #region >>>> 客户字典表 yhl
        /// <summary>
        /// 查询所有客户
        /// </summary>
        /// <returns></returns>
        public List<Dictcustomer> GetDictcustomer()
        {
            if (CacheHelper.GetCache("Dict.SelectDictcustomer") != null)
                return (List<Dictcustomer>)CacheHelper.GetCache("Dict.SelectDictcustomer");
            else
            {
                List<Dictcustomer> list = new DictCustomerService().GetDictCustomerList().ToList<Dictcustomer>();
                CacheHelper.SetCache("Dict.SelectDictcustomer", list);
                return list;
            }
        }
       
        #endregion

        #region >>>> 查询分点未选择的测试项
        ///// <summary>
        ///// 查询分点未选择的测试项
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetDicttestitemNotDictlabandtest(Hashtable ht)
        //{
        //    if (CacheHelper.GetCache("daan.GetDicttestitemNotDictlabandtest") != null)
        //        return (DataTable)CacheHelper.GetCache("daan.GetDicttestitemNotDictlabandtest");
        //    else
        //    {
        //        DataTable ds = this.selectDS("Dict.GetDicttestitemNotDictlabandtest", ht).Tables[0];
        //        CacheHelper.SetCache("daan.GetDicttestitemNotDictlabandtest", ds);
        //        return ds;
        //    }
        //}
        #endregion

        #region >>>> 基础字典分类
        /// <summary>
        /// 基础字典分类【return List〈Dictlibrary〉】
        /// </summary>
        /// <returns></returns>
        public List<Dictlibrary> GetLoginDictlibraryList()
        {
            if (CacheHelper.GetCache("daan.GetDictLibraryLst") != null)
                return (List<Dictlibrary>)CacheHelper.GetCache("daan.GetDictLibraryLst");
            else
            {
                List<Dictlibrary> list = this.QueryList<Dictlibrary>("Dict.GetDictLibraryLst", null).ToList<Dictlibrary>();
                CacheHelper.SetCache("daan.GetDictLibraryLst", list);
                return list;
            }
        }
        #endregion

        #region >>>> 基础字典明细
        /// <summary>
        /// 基础字典明细【return List〈Dictlibraryitem〉】
        /// </summary>
        /// <returns></returns>
        public List<Dictlibraryitem> GetLoginDictlibraryitemList()
        {
            if (CacheHelper.GetCache("daan.SelectDictlibraryitemLst") != null)
                return (List<Dictlibraryitem>)CacheHelper.GetCache("daan.SelectDictlibraryitemLst");
            else
            {
                List<Dictlibraryitem> list = this.QueryList<Dictlibraryitem>("Dict.SelectDictlibraryitemLst", null).ToList<Dictlibraryitem>();
                CacheHelper.SetCache("daan.SelectDictlibraryitemLst", list);
                return list;
            }
        }
        #endregion

        #region >>>> 基本资料
        /// <summary>
        /// 基本资料表【return List〈Initbasic〉】
        /// </summary>
        /// <returns></returns>
        public List<Initbasic> GetLoginInitbasicList()
        {
            if (CacheHelper.GetCache("daan.SelectInitbasicLst") != null)
                return (List<Initbasic>)CacheHelper.GetCache("daan.SelectInitbasicLst");
            else
            {
                List<Initbasic> list = new InitBasicService().GetInitbasicLst(null);
                CacheHelper.SetCache("daan.SelectInitbasicLst", list);
                return list;
            }
        }
        #endregion

        #region >>>>  获取分点字典

        /// <summary>
        /// 获取分点字典
        /// </summary>
        public List<Dictlab> GetLoginDictlab()
        {
            if (CacheHelper.GetCache("daan.GetDictlab") != null)
                return (List<Dictlab>)CacheHelper.GetCache("daan.GetDictlab");
            else
            {
                List<Dictlab> ds = new DictlabService().GetDictlabList();
                return ds;
            }
        }

        /// <summary>
        /// 获取用户下分点
        /// </summary>
        public List<Dictlab> GetPermissionDictlab(UserInfo user)
        {
            string[] labidstr = user.joinLabidstr.Split(',');
            List<Dictlab> df = GetLoginDictlab().Where<Dictlab>(c => labidstr.Contains(c.Dictlabid.ToString())).ToList<Dictlab>();            
            return df;
        }

        #endregion

        #region >>>> 科室分类表
        /// <summary>
        /// 科室分类表【return List〈Dictlibraryitem〉】
        /// </summary>
        /// <returns></returns>
        public List<Dictlabdept> GetLoginDictlabdeptList()
        {
            if (CacheHelper.GetCache("daan.SelectDictlabdeptLst") != null)
                return (List<Dictlabdept>)CacheHelper.GetCache("daan.SelectDictlabdeptLst");
            else
            {
                List<Dictlabdept> list = this.QueryList<Dictlabdept>("Dict.SelectDictlabdept", null).ToList<Dictlabdept>();
                CacheHelper.SetCache("daan.SelectDictlabdeptLst", list);
                return list;
            }
        }
        #endregion

        #region >>>> 短信模板
        /// <summary>
        /// 短信模板【return List〈DictSmsModule〉】
        /// </summary>
        /// <returns></returns>
        public List<DictSmsModule> GetLoginDictSmsModuleList()
        {
            if (CacheHelper.GetCache("daan.GetDictSmsModuleLst") != null)
                return (List<DictSmsModule>)CacheHelper.GetCache("daan.GetDictSmsModuleLst");
            else
            {
                List<DictSmsModule> list = this.QueryList<DictSmsModule>("Dict.GetDictSmsModuleLst", null).ToList<DictSmsModule>();
                CacheHelper.SetCache("daan.GetDictSmsModuleLst", list);
                return list;
            }
        }
        #endregion

        #region >>>> 诊断建议互斥表
        /// <summary>
        /// 诊断建议互斥表【return List〈DictSmsModule〉】
        /// </summary>
        /// <returns></returns>
        public List<Dictdiagnosesmutex> GetLoginDictdiagnosesmutexList()
        {
            if (CacheHelper.GetCache("daan.GetDictdiagnosesmutexLst") != null)
                return (List<Dictdiagnosesmutex>)CacheHelper.GetCache("daan.GetDictdiagnosesmutexLst");
            else
            {
                List<Dictdiagnosesmutex> list = this.QueryList<Dictdiagnosesmutex>("Dict.SelectDictdiagnosesmutex", null).ToList<Dictdiagnosesmutex>();
                CacheHelper.SetCache("daan.GetDictdiagnosesmutexLst", list);
                return list;
            }
        }
        #endregion
    }
}
