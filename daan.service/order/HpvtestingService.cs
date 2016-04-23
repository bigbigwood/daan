using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using daan.domain;
using System.Collections;
using System.Data;

namespace daan.service.order
{
    public class HpvtestingService : BaseService
    {
        protected const string modulename = "HPV耗材初始化";

        /// <summary>获得耗材记录总数
        /// 
        /// </summary>
        /// <param name="ht">参数</param>
        /// <returns>符合条件的耗材记录总数</returns>
        public int GetHpvinstrumentsPageLstCountNew(Hashtable ht)
        {
            return int.Parse(this.selectDS("Order.SelectHpvinstrumentsCount", ht).Tables[0].Rows[0]["counts"].ToString());
        }

        /// <summary>根据ID获得耗材对象
        /// 
        /// </summary>
        /// <param name="hvpinstrumentsid">记录ID</param>
        /// <returns>耗材对象</returns>
        public Hpvinstruments GetHpvinstruments(double hvpinstrumentsid)
        {
            return this.selectObj<Hpvinstruments>("Order.GetHpvinstruments", hvpinstrumentsid);
        }

        /// <summary>根据条件精确查找 耗材接收条码扫描
        /// 
        /// </summary>
        /// <param name="ht">参数</param>
        /// <returns>获得耗材数据集</returns>
        public List<Hpvinstruments> GetHpvinstrumentsByWhere(Hashtable ht)
        {
            return this.QueryList<Hpvinstruments>("Order.SelectHpvinstrumentsByWhere", ht).ToList<Hpvinstruments>();
        }

        /// <summary>
        /// 根据条件模糊查找 耗材管理中心
        /// </summary>
        /// <param name="ht">参数</param>
        /// <returns>获得耗材数据集</returns>
        public List<Hpvinstruments> GetHpvinstrumentsByWhereNew(Hashtable ht)
        {
            return this.QueryList<Hpvinstruments>("Order.SelectHpvinstrumentsByWhereNew", ht).ToList<Hpvinstruments>();
        }

        /// <summary>
        /// 根据条件查找护美类产品返回公司体检量统计  zhangwei
        /// </summary>
        /// <param name="ht">参数</param>
        /// <returns>获得耗材数据集</returns>
        public DataTable GetListHpvinstrumentsByWhereTime(Hashtable ht)
        {
            return selectDS("Order.SelectHpvinstrumentsByWhereTime", ht).Tables[0];       
        }

        public DataTable GetHuMeiList(Hashtable ht)
        {
            DataTable dt = selectDS("Order.SelectHuMeiList", ht).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["age"] = System.Web.WebUI.GetAge(dt.Rows[i]["age"]);
            }
            return dt;
        }
        public int GetHuMeiListCount(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Order.SelectHuMeiListCount",ht).Tables[0].Rows[0][0].ToString());
        }
        /// <summary>
        /// 根据条件查询TM15检测数据  zhangwei
        /// </summary>
        /// <param name="ht">参数</param>
        /// <returns>获得耗材数据集</returns>
        public DataTable GetListTM15ByWhereTime(Hashtable ht)
        {
            DataTable dt = selectDS("Order.SelectTM15ByWhereTime", ht).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["年龄"] = System.Web.WebUI.GetAge(dt.Rows[i]["年龄"]);
                if (Convert.ToDouble(dt.Rows[i]["报告状态"]) >= 25)
                    dt.Rows[i]["报告状态"] = "报告已出";
                else
                    dt.Rows[i]["报告状态"] = "报告未出";
            }
            return dt;
        }
        //TM15检测数据列表查询
        public DataTable GetTM15List(Hashtable ht)
        {
            DataTable dt = selectDS("Order.SelectTM15List", ht).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["age"] = System.Web.WebUI.GetAge(dt.Rows[i]["age"]);
            }
            return dt;
        }
        public int GetTM15ListCount(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Order.SelectTm15ListCount", ht).Tables[0].Rows[0][0].ToString());
        }

        public DataTable SelectHPVTMAccondingInfos(Hashtable ht)
        {
            DataTable dt = selectDS("Order.SelectHPVTMAccondingInfos", ht).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["年龄"] = System.Web.WebUI.GetAge(dt.Rows[i]["年龄"]);
            }
            return dt;
        }

        /// <summary>
        /// 按分点查询所有检测项目，并组合项目及价格求和  zhangwei
        /// </summary>
        /// <param name="ht">参数</param>
        /// <returns>获得耗材数据集</returns>
        public DataTable GetListTestNameWhereTime(Hashtable ht)
        {
            return selectDS("Order.SelectTestnameWhereTime", ht).Tables[0];
        }

        /// <summary>新增  修改    Hpvinstrumentsid为0或者null时表示新增数据，反之为编辑数据
        /// 
        /// </summary>
        /// <param name="hpvs">产品对象</param>
        /// <returns>是否操作成功 True 成功 false 失败</returns>
        public bool InsertHpvinstruments(Hpvinstruments hpvs)
        {
            int nflag = 0;
            if (hpvs.Hpvinstrumentsid == 0 || hpvs.Hpvinstrumentsid == null)
            {
                try
                {
                    hpvs.Hpvinstrumentsid = getSeqID("SEQ_HPVINSTRUMENTS");
                    insert("Order.InsertHpvinstruments", hpvs);
                    nflag = 1;
                    List<LogInfo> logLst = getLogInfo<Hpvinstruments>(new Hpvinstruments(), hpvs);
                    AddMaintenanceLog("Hpvinstruments", int.Parse(hpvs.Hpvinstrumentsid.ToString()), logLst, "新增", hpvs.Instrumentsbarcode, hpvs.Dictcustomerid.ToString(), modulename);
                }
                catch (Exception ex)
                {
                    nflag = 0;
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                try
                {
                    Hpvinstruments dictlab = GetHpvinstruments(Convert.ToDouble(hpvs.Hpvinstrumentsid));
                    nflag = update("Order.UpdateHpvinstruments", hpvs);
                    List<LogInfo> logLst = getLogInfo<Hpvinstruments>(dictlab, hpvs);
                    AddMaintenanceLog("Hpvinstruments", int.Parse(hpvs.Hpvinstrumentsid.ToString()), logLst, "修改", hpvs.Instrumentsbarcode, hpvs.Dictcustomerid.ToString(), modulename);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return nflag > 0;
        }

        /// <summary>删除
        /// 
        /// </summary>
        /// <param name="strId">记录ID</param>
        /// <returns>是否删除成功  True False</returns>
        public bool DeleteHpvinstruments(string strId)
        {
            int nflag = 0;
            try
            {
                //增加删除日志对象 fhp
                Hpvinstruments hpvinstruments = GetHpvinstruments(Convert.ToDouble(strId));
                //临时存储待删除对象，备写日志用                
                nflag = this.delete("Order.DeleteHpvinstruments", strId);

                List<LogInfo> logLst = getLogInfo<Hpvinstruments>(hpvinstruments, new Hpvinstruments());
                AddMaintenanceLog("Hpvinstruments", hpvinstruments.Hpvinstrumentsid, logLst, "删除", hpvinstruments.Instrumentsbarcode, hpvinstruments.Instenterby, modulename);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag > 0;
        }

        /// <summary>
        /// 取消耗材与条码号的关联，即将条码号更新为空
        /// </summary>
        /// <param name="strId">记录ID</param>
        /// <returns>是否操作成功  True False</returns>
        public bool UpdateHpvinstruments(string strId)
        {
            int nflag = 0;
            try
            {
                //增加删除日志对象 fhp
                Hpvinstruments hpvinstruments = GetHpvinstruments(Convert.ToDouble(strId));
                //临时存储待删除对象，备写日志用      
                Hpvinstruments newhpvinstruments = hpvinstruments;
                newhpvinstruments.Barcodecreatedate = null;
                newhpvinstruments.Barcode = null;
                newhpvinstruments.Barcodeenterby = null;
                nflag = this.update("Order.UpdateHpvinstruments", newhpvinstruments);

                List<LogInfo> logLst = getLogInfo<Hpvinstruments>(hpvinstruments, new Hpvinstruments());
                AddMaintenanceLog("Hpvinstruments", hpvinstruments.Hpvinstrumentsid, logLst, "修改", hpvinstruments.Instrumentsbarcode, hpvinstruments.Instenterby, modulename);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return nflag > 0;
        }

        /// <summary>
        /// 获取产品类套餐集合
        /// </summary>
        /// <returns>返回产品类套餐集合</returns>
        public IList<Dicttestitem> GetDicttestitemWithIsProject()
        {
            IList<Dicttestitem> list = this.QueryList<Dicttestitem>("Dict.GetDictTestProductWithIsProject", null);

            return list;
        }
    }
}
