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
    public class DictMEDHistoryService : BaseService
    {
        /// <summary>
        /// 获取健康档案列表
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public DataTable GetHealthRecordsDataList(string MemberID)
        {
            return selectDS("Dict.SelectDicthealthrecords", MemberID).Tables[0];
        }

        public bool InsertOrUpdateHealthRecord(Dicthealthrecords model,bool isAdd)
        {
            int cnt=0;
            bool b = true;
            try
            {
                if (isAdd)
                {
                    model.Dicthealthrecordsid = getSeqID("SEQ_DICTHEALTHRECORDS");
                    cnt = Convert.ToInt32(insert("Dict.InsertDicthealthrecords", model));
                }
                else
                {
                    cnt = Convert.ToInt32(insert("Dict.UpdateDicthealthrecords", model));
                }
                if (cnt < 0) 
                    b = false;
            }
            catch(Exception)
            {
                b = false;
            }
            return b;
        }

        public bool DeleteHealthRecords(string hid)
        {
            return Convert.ToInt32(delete("Dict.DeleteDicthealthrecords", hid)) > 0;
        }

        /// <summary>
        /// 获取既往疾病列表
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public IList<Dictmedhistory> GetDictmedHistoryDataList(string MemberID)
        {
            return this.QueryList<Dictmedhistory>("Dict.SelectDictmedhistory", MemberID);
        }

        /// <summary>
        /// 获取家族疾病列表
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public IList<Dictfamilymedhistory> GetDictFamilymedHistoryDataList(string MemberID)
        {
            return this.QueryList<Dictfamilymedhistory>("Dict.SelectDictfamilymedhistory", MemberID);
        }

        /// <summary>
        /// 获取其他病史-药物过敏史
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public IList<Dictothermedhistory> GetDictOrthermedHistoryDataList(string MemberID)
        {
            return this.QueryList<Dictothermedhistory>("Dict.SelectDictothermedhistory", MemberID);
        }

        #region 既往体检存档
        /// <summary>
        /// 查询既往体检存档记录数
        /// </summary>
        /// <param name="Mid"></param>
        /// <returns></returns>
        public int GetDictPastOrdersCount(string Mid)
        {
            return Convert.ToInt32(selectDS("Dict.SelectPastOrdersCount", Mid).Tables[0].Rows[0][0]);
        }
        /// <summary>
        /// 获取既往体检存档记录集合
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetDictPastOrders(Hashtable ht)
        {
            return selectDS("Dict.SelectPastOrders", ht).Tables[0];
        }

        public DataTable GetDictPastOrdersNoPages(string Mid)
        {
            return selectDS("Dict.SelectPastOrdersNoPages", Mid).Tables[0];
        }
        #endregion
        /// <summary>
        /// 添加既往疾病记录
        /// </summary>
        /// <param name="medhistory"></param>
        /// <returns></returns>
        public bool InsertmedHistory(Dictmedhistory medhistory)
        {
            bool b = true;
            try
            {
                medhistory.Dictmedhistoryid = this.getSeqID("SEQ_DICTMEDHISTORY");
                int i = Convert.ToInt32(this.insert("Dict.InsertDictmedhistory", medhistory));
                if (i < 0) { b = false; }
            }
            catch (Exception)
            {
                b = false;
            }
            return b;
        }

        /// <summary>
        /// 添加家族疾病记录
        /// </summary>
        /// <param name="medhistory"></param>
        /// <returns></returns>
        public bool InsertFamilymedHistory(Dictfamilymedhistory familymedhistory)
        {
            bool b = true;
            try
            {
                familymedhistory.Dictfamilymedhistoryid = this.getSeqID("SEQ_DICTFAMILYMEDHISTORY");
                int i = Convert.ToInt32(this.insert("Dict.InsertDictfamilymedhistory", familymedhistory));
                if (i < 0) { b = false; }
                
            }
            catch (Exception)
            {
                b = false;
            }

            return b;
        }

        /// <summary>
        /// 添加其他病史-药物过敏史
        /// </summary>
        /// <param name="medhistory"></param>
        /// <returns></returns>
        public bool InsertOthermedHistory(Dictothermedhistory othermedhistory)
        {
            bool b = true;
            try
            {
                othermedhistory.Dictothermedhistoryid = this.getSeqID("SEQ_DICTORTHERMEDHISTORY");
                int i = Convert.ToInt32(this.insert("Dict.InsertDictothermedhistory", othermedhistory));
                if (i < 0) { b = false; }
            }
            catch (Exception)
            {
                b = false;
            }

            return b;
        }

        /// <summary>
        /// 修改其他病史-药物过敏史
        /// </summary>
        /// <param name="medhistory"></param>
        /// <returns></returns>
        public bool UpdateOthermedHistory(Dictothermedhistory othermedhistory)
        {
            bool b = true;
            try
            {
                int i = Convert.ToInt32(this.update("Dict.UpdateDictothermedhistory", othermedhistory));
                if (i < 0) { b = false; }
            }
            catch (Exception)
            {
                b = false;
            }

            return b;
        }

        /// <summary>
        /// 删除既往病史一条记录
        /// </summary>
        /// <param name="medhistory"></param>
        /// <returns></returns>
        public bool DeletemedHistory(string ID)
        {
            bool b = true;
            try
            {
                int i = Convert.ToInt32(this.delete("Dict.DeleteDictmedhistory", ID));
                if (i < 0) { b = false; }
            }
            catch (Exception)
            {
                b = false;
            }

            return b;
        }

        /// <summary>
        /// 删除家族病史一条记录
        /// </summary>
        /// <param name="medhistory"></param>
        /// <returns></returns>
        public bool DeleteFamilymedHistory(string ID)
        {
            bool b = true;
            try
            {
                int i = Convert.ToInt32(this.delete("Dict.DeleteDictfamilymedhistory", ID));
                if (i < 0) { b = false; }
            }
            catch (Exception)
            {
                b = false;
            }

            return b;
        }


    }
}
