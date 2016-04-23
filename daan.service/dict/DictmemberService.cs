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
    public class DictmemberService : BaseService
    {
        #region >>>> zhouy 获取会员列表
        /// <summary>
        /// 获取会员列表
        /// </summary>
        public List<Dictmember> GetDictmemberList(Dictmember m)
        {
             
            List<Dictmember> ds = this.QueryList<Dictmember>("Dict.SelectDictmember", m).ToList<Dictmember>();
            return ds;
        }

        #endregion

        #region >>>> zhouy 获取单个会员信息
        /// <summary>
        /// 获取单个会员信息
        /// </summary>
        public Dictmember GetMemberById(double? memberid)
        {
            Dictmember member = new Dictmember();
            member.Dictmemberid = memberid;
            List<Dictmember> memberList = GetDictmemberList(member);
            if (memberList.Count > 0)
            {
                member = memberList[0];
            }
            else { member = null; }
            return member;
        }
        #endregion

        #region >>>> lee 按体检号获取会员信息
        /// <summary>
        /// 按体检号获取会员信息
        /// </summary>
        public Dictmember SelectDictmemberByOrderNum(string strOrdernum)
        {
            return this.selectObj<Dictmember>("Dict.SelectDictmemberByOrderNum", strOrdernum);  
        }
        #endregion

        #region >>>> 销售员导出会员信息
        public DataTable GetCustomerInfosExportList(Hashtable ht)
        {
            return selectDS("Dict.SelectCustomerInfosExportList", ht).Tables[0];
        }

        public int GetCustomerInfosExportCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.SelectCustomerInfosExportCount", ht).Tables[0].Rows[0][0]);
        }
        #endregion


        #region >>>> 获取省市区数据
        public DataTable GetProveice()
        {
            return selectDS("Dict.Province", null).Tables[0];
        }
        public DataTable GetCity(string pid)
        {
            return selectDS("Dict.City", pid).Tables[0];
        }
        public DataTable GetCounty(string cid)
        {
            return selectDS("Dict.County", cid).Tables[0];
        }
        #endregion
    }
}
