using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using daan.service.common;
using System.Collections;
using daan.domain;

namespace daan.service.dict
{
   public  class DictuserService:BaseService
   {

        protected const string modulename = "用户资料维护";
       #region >>>> 查询所有用户  zhangwei
       public IList<Dictuser> GetDictuser()
       {
           return this.QueryList<Dictuser>("Dict.SelectDictuser", null);
       }
       #endregion

       #region >>>> 分页获取用户列表 zhangwei
       /// <summary>
       /// 获取分点列表
       /// </summary>
       /// <param name="usercode"></param>
       /// <returns></returns>
       public IList<Dictuser> GetDictuserPageLst(Hashtable ht)
       {
           return this.QueryList<Dictuser>("Dict.GetDictuserPageLst", ht);
       }
       #endregion

       #region >>>> 获取用户列表总项数 zhangwei
       /// <summary>
       /// 获取分点列表总项数
       /// </summary>
       /// <param name="usercode"></param>
       /// <returns></returns>
       public int GetDictuserPageLstCount(Hashtable ht)
       {
           return Convert.ToInt32(this.selectDS("Dict.GetDictuserPageLstCount", ht).Tables[0].Rows[0][0]); 
       }
       #endregion

        public Dictuser GetDictuserInfo(Dictuser library)
        {
            //Dictuser obj = null;
            //IList lst = this.selectIList("Dict.GetDictuserInfo", library);
            //if (lst.Count > 0)
            //    obj = (Dictuser)lst[0];
            //return obj;
            return this.selectObj<Dictuser>("Dict.GetDictuserInfo", library);
        }

        /// <summary>
        /// 根据double类型Dictuserid获取用户名称
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        public Dictuser GetDictuserInfoAuto(double? library)
        {
            //Dictuser obj = null;
            //IList lst = this.selectIList("Dict.GetDictuserInfo", library);
            //if (lst.Count > 0)
            //    obj = (Dictuser)lst[0];
            //return obj;
            return this.selectObj<Dictuser>("Dict.GetDictuserInfo", library);
        }


        public Dictuser GetDictuserInfoByUserCode(Dictuser dictuser)
        {
            //Dictuser obj = null;
            //IList lst = this.selectIList("Dict.GetDictuserInfoByCode", dictuser);
            //if (lst.Count > 0)
            //    obj = (Dictuser)lst[0];
            //return obj;

            return this.selectObj<Dictuser>("Dict.GetDictuserInfoByCode", dictuser);
        }

         ///<summary>
         ///新增编辑后保存
         ///</summary>
         ///<param name="library"></param>
        /// <returns></returns>
        public bool SaveDictlab(Dictuser library)
        {  
            int nflag = 0;
            //新增
            if (library.Dictuserid == null || library.Dictuserid == 0)
            {
                try
                {
                    library.Dictuserid = getSeqID("SEQ_DICTUSER");
                    insert("Dict.InsertDictuser", library);
                    CacheHelper.RemoveAllCache("daan.SelectDictuseresult");
                    CacheHelper.RemoveAllCache("daan.GetLoginUserInfo");
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Dictuser>(new Dictuser(), library);
                    AddMaintenanceLog("Dictuser", int.Parse(library.Dictlabid.ToString()), logLst, "新增", library.Username, library.Usercode, modulename);
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
                    Dictuser dictlab = GetDictuserInfo(library);
                    nflag = update("Dict.UpdateDictuser", library);
                    CacheHelper.RemoveAllCache("daan.SelectDictuseresult");
                    CacheHelper.RemoveAllCache("daan.GetLoginUserInfo");
                    List<LogInfo> logLst = getLogInfo<Dictuser>(dictlab, library);
                    AddMaintenanceLog("Dictuser", int.Parse(library.Dictlabid.ToString()), logLst, "修改", library.Username, library.Usercode, modulename);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return nflag > 0 ;
        }
       
   }
}
