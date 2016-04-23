using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using daan.service.common;
using daan.domain;
using System.Data;
using System.Web;

namespace daan.service.order
{
    public class OrdersService : BaseService
    {

        /// <summary>
        /// 查询已总检的体检系统上传到社区网站
        /// </summary>
        /// <returns></returns>
        public DataTable GetSelectOrdersByStatus()
        {
            return selectDS("Order.SelectOrdersByStatus", null).Tables[0];
        }

        /// <summary>
        /// 查询已总检的体检系统上传到社区网站
        /// </summary>
        /// <returns></returns>
        public DataTable GetdtTitle(Hashtable ht)
        {
            return selectDS("Order.GetdtTitle", ht).Tables[0];
        }

        /// <summary>
        ///根据查询ordernum查找体检报告
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public DataTable GetSelectOrdersResultByOrdernum(Hashtable ht)
        {
            return selectDS("Order.SelectOrdersResultByOrdernum", ht).Tables[0];
        }





        /// <summary>
        ///平均年龄，团检报告
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetAvgAge(Hashtable ht)
        {
            return selectDS("Order.GetAvgAge", ht).Tables[0];

        }

        /// <summary>
        ///已预约未体检人员，团检报告
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetDonotJoinTest(Hashtable ht)
        {
            return selectDS("Order.GetDonotJoinTest", ht).Tables[0];

        }
        /// <summary>
        ///平均年龄以下人数，团检报告
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetAvgAgeLow(Hashtable ht)
        {
            return selectDS("Order.GetAvgAgeLow", ht).Tables[0];

        }
        /// <summary>
        ///平均年龄以上人数，团检报告
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetAvgAgeHight(Hashtable ht)
        {
            try
            {
                return selectDS("Order.GetAvgAgeHight", ht).Tables[0];
            }
            catch
            {
                return null;
            }


        }
        /// <summary>
        ///参检人员年龄及性别，团检报告
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetAgeRegion(Hashtable ht)
        {
            return selectDS("Order.GetAgeRegion", ht).Tables[0];

        }
        /// <summary>
        ///男女实际人数，团检报告
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetFMRitol(Hashtable ht)
        {
            try
            {
                DataTable dt = selectDS("Order.GetFMRitol", ht).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ee)
            {

                return null;
            }


        }
        /// <summary>
        ///总检查询,分页查询
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable DataForFinalCheck(Hashtable ht)
        {
            return selectDS("Order.DataForFinalCheck", ht).Tables[0];

        }
        /// <summary>
        ///体检日期，团检报告
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetEnterdate(Hashtable ht)
        {
            return selectDS("Order.GetEnterdate", ht).Tables[0];

        }
        /// <summary>
        /// 总检查询,页数查询
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int CountForFinalCheck(Hashtable ht)
        {
            return int.Parse(selectDS("Order.CountForFinalCheck", ht).Tables[0].Rows[0][0].ToString());
        }
        /// <summary>
        ///体检报告单查询打印,分页查询
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable DataForFocusPrintPageLst(Hashtable htPara)
        {
            DataTable dt = selectDS("Order.DataForFocusPrintPageLst", htPara).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["AGE"] = WebUI.GetAge(dt.Rows[i]["AGE"]);
            }
            return dt;
        }
        /// <summary>
        ///体检报告单查询打印总数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int DataForFocusPrintPageTotal(Hashtable htPara)
        {
            return int.Parse(this.selectDS("Order.DataForFocusPrintPageTotal", htPara).Tables[0].Rows[0]["total"].ToString());      
        }

        public DataTable GetReportDataForWinform(Hashtable ht)
        {
            DataTable dt = selectDS("Order.DataForReport", ht).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["AGE"] = WebUI.GetAge(dt.Rows[i]["AGE"]);
            }
            return dt;
        }

        /// <summary>
        /// 查询订单异常报告记录数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int GetOrderExceptionReport(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Order.GetOrderExceptionReport", ht).Tables[0].Rows[0][0].ToString());
        }

        public DataTable GetOrderExceptionInfo(string ordernum)
        {
            return selectDS("Order.GetOrderExceptionInfo", ordernum).Tables[0];
        }
        public DataTable GetOrderException(Hashtable ht)
        {
            return selectDS("Order.GetOrderException", ht).Tables[0];
        }
        /// <summary>
        /// 【集中打印报告】导出数据
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable DataForFocusPrintExport(Hashtable ht)
        {
            DataTable dt = selectDS("Order.DataForFocusPrintExport", ht).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["年龄"] = WebUI.GetAge(dt.Rows[i]["年龄"]);
            }
            return dt;
        }
        /// <summary>
        ///客户追踪处理查询,分页查询
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable CountForAnaCustomTraceHandle(Hashtable ht)
        {
            return selectDS("Order.CountForAnaCustomTraceHandle", ht).Tables[0];
        }
        /// <summary>
        ///客户追踪处理查询,分页查询
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable DataForAnaCustomTraceHandle(Hashtable ht)
        {
            return selectDS("Order.DataForAnaCustomTraceHandle", ht).Tables[0];

        }
        /// <summary>
        /// 根据ordernum查询
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public Orders SelectOrdersByOrdernum(string ordernum)
        {
            try
            {
                return this.selectObj<Orders>("Order.SelectOrders", ordernum);
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// 增加预约复查时间
        /// </summary>
        /// <param name="rerundate"></param>
        /// <returns></returns>
        public bool EditRerundate(Hashtable ht)
        {
            return update("Order.EditRerundate", ht) > 0;
        }
        /// <summary>
        /// 订单号完成跟进
        /// </summary>
        /// <returns></returns>
        public bool EditReviewstate(string ordernum)
        {
            return update("Order.EditReviewstate", ordernum) > 0;
        }
        /// <summary>
        /// 订单号状态修改，
        /// 输入参数，订单号ordernum,订单状态status
        /// 输出参数，受影响的行数是否大于0
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool EditStatus(Hashtable ht)
        {
            return update("Order.EditStatus", ht) > 0;
        }

        /// <summary>>>>> zhouy 根据已有的状态，批量修改订单号
        /// 根据已有的状态，批量修改订单号
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool EditStatusByOldStatus(Hashtable ht)
        {
            return update("Order.EditStatusByOldStatus", ht) > 0;
        }

        /// <summary>
        /// 查询订单 上传到LIS ylp
        /// </summary>
        /// <returns></returns>
        public DataTable GetOrderToLis(double? dictlabid)
        {
            DataSet ds = selectDS("Order.SelectOrdersToLis", dictlabid);
            return ds.Tables[0];
        }

        /// <summary>
        /// 社区网站根据订单号修改是否上传状态
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool EditTransed(Hashtable ht)
        {
            return update("Order.EditTransed", ht) > 0;
        }

        /// <summary>
        /// 当同一订单号ORDERTEST表的所有检验记录取到结果，更新ORDERS表IOLIS状态为1 yhl
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public int UpdateOrderIOLIS(Hashtable ht)
        {
            try
            {
                return update("Order.UpdateOrderIOLIS", ht);
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// 数据接收:全选时获取符合条件的所有ordernum
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public string SelectOrdernumString(Hashtable ht)
        {
            try
            {
                return this.selectIList("Order.SelectOrdernumString", ht)[0].ToString();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 退回重做
        /// </summary>
        /// <param name="strid">要退回的科室小结ID字符串</param>
        /// <param name="ordernum">订单号</param>
        /// <returns></returns>
        public bool CancelAudit(string strid, string ordernum)
        {
            return true;
        }
        /// <summary>
        /// 根据相关条件查询订单上传社区标志失败状态的记录
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable SelectUploadToSheQu(Hashtable ht)
        {
            return selectDS("Order.UploadToSheQu", ht).Tables[0];
        }
        public int SelectUploadToSheQuCount(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Order.UploadToSheQuCount", ht).Tables[0].Rows[0][0].ToString());
        }
        /// <summary>
        /// 根据选择的订单号修改上传社区标志状态
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool EditSelectTransed(Hashtable ht)
        {
            return update("Order.EditSelectTransed", ht) > 0;
        }

        #region >>>> zhouy 获取需要自动取结果的订单号
        /// <summary>
        /// 获取需要自动取结果的订单号
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public string[] SelectGetResultOrderNum(double? labid)
        {
            IList<string> ordernumlist = QueryList<string>("Order.SelectGetResultOrderNum", labid);
            string[] arryOrdernum = ordernumlist.ToArray<string>();
            return arryOrdernum;
        }
        /// <summary>
        /// 获取需要自动取结果的订单号
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public string[] SelectGetResultOrderNumFromWebLis(double? labid)
        {
            IList<string> ordernumlist = QueryList<string>("Order.SelectGetResultOrderNumFromWebLis", labid);
            string[] arryOrdernum = ordernumlist.ToArray<string>();
            return arryOrdernum;
        }
        #endregion

        #region >>>短信
        /// <summary>
        /// 获取待发送短信人员名单
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable WaitSendSmsUsers(Hashtable ht)
        {
            return selectDS("Order.WaitSendSmsUsers", ht).Tables[0];
        }

        /// <summary>
        /// 获取待发送短信人员名单行数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int WaitSendSmsUsersCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Order.WaitSendSmsUsersCount", ht).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 获取要下载的人员名单
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Orders> WaitDownLoadUsers(Hashtable ht)
        {
            return this.QueryList<Orders>("Order.WaitExcelUsers", ht).ToList();
        }
        /// <summary>
        /// 短信发送成功后保存发送的内容标题
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool UpdateOrdersSms(Hashtable ht)
        {
            return update("Order.UpdateOrdersSms", ht) > 0;
        }
        #endregion

        /// <summary>
        /// 查询逾期未检的订单信息列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetOverdueOrders(Hashtable ht)
        {
            DataTable dt = this.selectDS("Order.GetOverdueOrders", ht).Tables[0];
            
            return dt;
        }

        /// <summary>
        /// 查询逾期未检的订单总数
        /// </summary>
        /// <returns></returns>
        public int GetOverdueOrdersCount()
        {
            return Convert.ToInt32(this.selectDS("Order.GetOverdueOrdersCount", null).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 查询统计HPV、TM检查人员信息
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetHPVTMAccondingInfos(Hashtable ht)
        {
            DataTable dt = selectDS("Order.GetHPVTMAccondingInfos", ht).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["AGE"] = WebUI.GetAge(dt.Rows[i]["AGE"]);
            }
            return dt;
        }
        public int GetHPVTMAccondingInfosCount(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Order.GetHPVTMAccondingInfosCount", ht).Tables[0].Rows[0][0]);
        }
        /// <summary>
        /// 根据体检流水号查询备注
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public string GetRemarksByordernum(string ordernum)
        {
            return selectDS("Order.GetRemarksByordernum", ordernum).Tables[0].Rows[0][0].ToString();
        }
        /// <summary>
        /// 添加、修改备注
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool UpdateOrdersRemarks(Hashtable ht)
        {
            return update("Order.UpdateOrdersRemarks", ht) > 0;
        }
        /// <summary>
        /// 根据姓名+性别查询体检记录
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetMemeberPastOrders(Hashtable ht)
        {
            DataTable dt = selectDS("Order.GetMemberPastOrders", ht).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["AGE"] = WebUI.GetAge(dt.Rows[i]["AGE"]);
            }
            return dt;
        }
        /// <summary>
        /// 手动关联会员历史体检记录
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool UpdateOrdersMemberInfoByName(Hashtable ht)
        {
            return update("Order.UpdateOrdersMemberInfoByName", ht) > 0;
        }

        /// <summary>
        /// 总检医生查询订单
        /// </summary>
        /// <param name="ht">查询参数</param>
        /// <returns></returns>
        public DataTable GetFinishOrdersList(Hashtable ht,string t)
        {
            string statementName = t == "1" ? "Order.GetAuthorizedOrdersList" : "Order.GetFinishOrdersList"; 
            DataTable dt=selectDS(statementName, ht).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["AGE"] = WebUI.GetAge(dt.Rows[i]["AGE"]);
            }
            return dt;
        }
        public int GetFinishOrdersListCount(Hashtable ht,string t)
        {
            string statementName = t == "1" ? "Order.GetAuthorizedOrdersListCount" : "Order.GetFinishOrdersListCount";
            return Convert.ToInt32(selectDS(statementName, ht).Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 根据条码号查询会员信息以及报告单状态
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public DataTable QueryReportStatus(string barcode)
        {
            return selectDS("Order.GetOrderInfoByBarcode", barcode).Tables[0];
        }

        #region 自动初步总检
        /// <summary>
        /// 获取符合自动初步总检条件的订单列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetAutoChuBuList()
        {
            return selectDS("Order.GetAutoChuBuList", null).Tables[0];
        }
        /// <summary>
        /// 检测结果是否有异常
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public bool CheckTestResultIsException(string ordernum)
        {
            return Convert.ToInt32(selectDS("Order.CheciTestResultIsException", ordernum).Tables[0].Rows[0][0].ToString()) > 0;
        }
        /// <summary>
        /// 自动初步总检中结果有异常的做上标记，下次扫描就不需要查询此状态的记录
        /// </summary>
        /// <param name="ht"></param>
        public void SetAutoFirstCheckStatus(Hashtable ht)
        {
            update("Order.SetAutoFirstCheck", ht);
        }
        /// <summary>
        /// 随机获取正常结果描述评价
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        public DataTable GetRandResultComment(int rand)
        {
            return selectDS("Order.GetRandResultComment", rand).Tables[0];
        }
        #endregion

        #region TM交付管理信息统计
        /// <summary>
        /// TM信息列表
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable QueryTMStatList(Hashtable ht)
        {
            return selectDS("Order.QueryTMStatList", ht).Tables[0];
        }
        public int GetTMStatListCount(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Order.QueryTMStatListCount", ht).Tables[0].Rows[0][0].ToString());
        }
        /// <summary>
        /// 操作日志查询
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable QueryOperationLog(Hashtable ht)
        {
            return selectDS("Order.QueryOperationLog", ht).Tables[0];
        }
        /// <summary>
        /// 导出数据查询
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable QueryTMStatExportList(Hashtable ht)
        {
            return selectDS("Order.QueryTMStatExportList", ht).Tables[0];
        }
        #region 预查询方案
        /// <summary>
        /// 插入预查询条件
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool InsertPreSearchInfo(Hashtable ht)
        {
            int flag = 0;
            try
            {
                ht.Add("searchid", getSeqID("SEQ_TMSTATPRESEARCH"));
                insert("Stat.InsertTMSTATPRESEARCH", ht);
                flag = 1;
            }
            catch (Exception e)
            {
                flag = 0;
                throw new Exception(e.Message);
            }
            return flag > 0;
        }
        public DataTable SelectPreSearchList(Hashtable ht)
        {
            return selectDS("Stat.SelectTMSTATPRESEARCH", ht).Tables[0];
        }
        public DataTable GetPreSearchList()
        {
            return selectDS("Stat.GetPreSearchList", null).Tables[0];
        }
        public bool SetPreSearchStatus(Hashtable ht)
        {
            return update("Stat.SetTMPreSearchStatus", ht) > 0;
        }
        public bool InsertTMStatSearchRestult(DataRow dr)
        {
            int flag = 0;
            try
            {
                Hashtable htPara = new Hashtable();
                htPara.Add("tmstatresultid", getSeqID("SEQ_TMSTATRESULT"));
                htPara.Add("searchid", dr["searchid"].ToString());
                htPara.Add("ordercode", dr["ordercode"].ToString());
                htPara.Add("labname", dr["labname"].ToString());
                htPara.Add("customername", dr["customername"].ToString());
                htPara.Add("section", dr["section"].ToString());
                htPara.Add("ordertestlst", dr["ordertestlst"].ToString());
                htPara.Add("createdate", dr["createdate"].ToString());
                htPara.Add("samplingdate", dr["samplingdate"].ToString());
                htPara.Add("ordercount", dr["ordercount"]);
                htPara.Add("importcount", dr["importcount"]);
                htPara.Add("importtime", dr["importtime"]);
                htPara.Add("resultcount", dr["resultcount"]);
                htPara.Add("resulttime", dr["resulttime"]);
                htPara.Add("finishedcount", dr["finishedcount"]);
                htPara.Add("finishedtime", dr["finishedtime"]);
                htPara.Add("printcount", dr["printcount"]);
                htPara.Add("printtime", dr["printtime"]);
                insert("Stat.InsertTMSTATSEARCHRESULT", htPara);
                flag = 1;
            }
            catch (Exception ex)
            {
                flag = 0;
                throw new Exception(ex.Message);
            }
            return flag > 0;
        }
        /// <summary>
        /// 查询统计结果列表
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable SelectTMStatSearchResultList(Hashtable ht)
        {
            return selectDS("Stat.SelectTMStatSearchResultList", ht).Tables[0];
        }
        public int SelectTMStatSearchResultListCount(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Stat.SelectTMStatSearchResultListCount", ht).Tables[0].Rows[0][0].ToString());
        }
        /// <summary>
        /// 查询导出的查询统计结果
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable SelectTMStatSearchResultExportList(Hashtable ht)
        {
            return selectDS("Stat.SelectTMStatSearchResultExportList", ht).Tables[0];
        }
        /// <summary>
        /// 删除查询统计结果
        /// </summary>
        /// <param name="id"></param>
        public void DeleteTMStatSearchRestult(Double id)
        {
            delete("Stat.DeleteTmStatSearchResult", id);
        }
        #endregion
        #endregion

        #region 回访记录
        /// <summary>
        /// 查询回访记录列表
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetVisitList(Hashtable ht)
        {
            return selectDS("Order.GetVisitList", ht).Tables[0];
        }
        /// <summary>
        /// 新增编辑后保存回访记录
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool SaveVisit(Hashtable ht)
        {
            int nflag = 0;
            //新增
            if (ht["visitid"].ToString() == "0" || ht["visitid"] == null)
            {
                try
                {
                    ht["visitid"] = getSeqID("SEQ_ORDERVISIT");
                    insert("Order.InsertVisit", ht);
                    nflag = 1;
                }
                catch (Exception e)
                {
                    nflag = 0;
                    throw new Exception(e.Message);
                }
            }
            else
            {
                nflag = update("Order.UpdateVisit", ht);
            }
            return nflag > 0;
        }
        /// <summary>
        /// 删除回访记录
        /// </summary>
        /// <param name="visitid">回访记录ID</param>
        /// <returns></returns>
        public bool DeleteVisit(Double visitid)
        {
            return delete("Order.DeleteVisit", visitid) > 0;
        }
        #endregion

        #region 迟发退单报告
        public bool InsertOrderExceptionReport(List<Hashtable> list,ref string errstr)
        {
            //sql集合
            SortedList SQLlist = new SortedList(new daan.util.Common.MySort());
            try
            {
                foreach (Hashtable ht in list)
                {
                    SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrderExceptionReport" } }, ht);
                }
            }
            catch (Exception ex)
            {
                errstr = ex.Message;
                return false;
            }
            return ExecuteSqlTran(SQLlist, ref errstr);
        }
        #endregion
    }
}
