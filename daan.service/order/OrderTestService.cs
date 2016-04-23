using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using daan.domain;
using System.Collections;
using IBatisNet.DataMapper;
using IBatisNet.Common;
using System.Data;
using System.Data;
using daan.util.Common;
namespace daan.service.order
{
    public class OrderTestService : BaseService
    {
        /// <summary>
        /// 查看订单号相对应科室的检查结果
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable DataForOrderLabdeptresult(Hashtable ht)
        {

            return selectDS("order.DataForOrderLabdeptresult", ht).Tables[0];
        }


        public DataTable GetOrdertestByOrdernumGroupBy(Hashtable ht)
        {
            return selectDS("Order.GetOrdertestByOrdernumGroupBy", ht).Tables[0];
        }

        /// <summary>
        /// 团检，重要指标检查结果查询,单项
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable DataForImporttantTestItem(Hashtable ht)
        {

            return selectDS("order.DataForImporttantTestItem", ht).Tables[0];
        }

        /// <summary>
        /// 团检，重要指标检查结果查询,双项
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable DataForImporttantTestItemTwo(Hashtable ht)
        {

            return selectDS("order.DataForImporttantTestItemTwo", ht).Tables[0];
        }

        /// <summary>
        ///团检，重要指标检查结果详细单一查询,单项
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable DataForOneImporttantTestItem(Hashtable ht)
        {
            try
            {
                return selectDS("order.DataForOneImporttantTestItem", ht).Tables[0];
            }
            catch
            {
                return null;
            }

        }
        /// <summary>
        ///团检，重要指标检查结果详细单一查询,双项
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable DataForTwoImporttantTestItem(Hashtable ht)
        {
            try
            {
                return selectDS("order.DataForTwoImporttantTestItem", ht).Tables[0];
            }
            catch
            {
                return null;
            }

        }
        /// <summary>
        ///已体检而未完成项目，团检报告
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetdtGroupAnFinish(Hashtable ht)
        {

            return selectDS("Order.GetdtGroupAnFinish", ht).Tables[0];
        }

        /// <summary>
        /// 获取详细内容
        /// </summary>
        /// <param name="strid"></param>
        /// <returns></returns>
        public Ordertest GetOrdertestInfo(string strid)
        {
            return this.selectObj<Ordertest>("Order.SelectOrdertest", strid);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="rerundate"></param>
        /// <returns></returns>
        public bool Update(Ordertest orderTest)
        {
            return this.update("Order.UpdateOrdertest", orderTest) > 0;
        }
        /// <summary>
        /// 结果录入编辑
        /// </summary>
        /// <param name="rerundate"></param>
        /// <returns></returns>
        public bool UpdateOrdertestResult(Ordertest orderTest)
        {
            return this.update("Order.UpdateOrdertestResult", orderTest) > 0;
        }

        #region 检查人员列表分页
        /// <summary>
        /// 获取待录入检查人员总数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int GetOrderMemberPageLstCount(Hashtable ht)
        {
            return int.Parse(this.selectDS("order.GetOrderMemberPageLstCount", ht).Tables[0].Rows[0]["pageCount"].ToString());
        }
        /// <summary>
        /// 获取待录入检查人员分页
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataSet GetOrderMemberPageLst(Hashtable ht)
        {
            return selectDS("order.GetOrderMemberPageLst", ht);
        }
        #endregion

        /// <summary>
        /// 获取科室检查结果
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Ordertest> GetOrderLabdeptresultList(Hashtable ht)
        {
            try
            {
                return this.QueryList<Ordertest>("order.GetOrderLabdeptresultList", ht);
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 按体检编号取全部检查结果
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Ordertest> GetAllOrderLabdeptresultList(string strordernum)
        {
            try
            {
                return this.QueryList<Ordertest>("order.GetAllOrderLabdeptresultList", strordernum).ToList<Ordertest>();
            }
            catch
            {
                return null;
            }

        }
        /// <summary>
        /// [自动小结]检查结果是否为空
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool IsNullOrderTestResult(Hashtable ht)
        {
            try
            {
                return selectDS("order.GetCountNullOrderTestResult", ht).Tables[0].Rows.Count > 0;
            }
            catch
            {
                return false;
            }

        }


        /// <summary>
        ///根据查询ordernum查找体检报告
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public DataTable GetSelectOrdertestByOrdernum(Hashtable ht)
        {
            return selectDS("Order.GetOrdertestByOrdernum", ht).Tables[0];
        }

        /// <summary>
        /// 按条码号更新到ORDERTEST表
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool UpdateOrdertestByBarcode(Hashtable ht)
        {
            try
            {
                DataTable dtdatarow = (DataTable)ht["data_row"];
                DataTable dtTestresults = (DataTable)ht["Testresults"];
                string ordernum = ht["ordernum"].ToString();

                SortedList SQLlist = new SortedList(new MySort());

                for (int k = 0; k < dtdatarow.Rows.Count; k++)
                {
                    #region 修改ordertest表

                    //修改ordertest表
                    for (int i = 0; i < dtTestresults.Rows.Count; i++)
                    {
                        Hashtable htinfo = new Hashtable();
                        htinfo["ORDERNUM"] = ordernum;
                        
                        htinfo["UNIT"] = dtTestresults.Columns.Contains("Unit") == false ? null : dtTestresults.Rows[i]["Unit"].ToString();                        
                        //edit by lee
                        //htinfo["HLFLAG"] = dtTestresults.Columns.Contains("Hlflag") == false ? null : dtTestresults.Rows[i]["Hlflag"].ToString();
                        //htinfo["TESTRESULT"] = dtTestresults.Columns.Contains("ItemResult") == false ? "" : dtTestresults.Rows[i]["ItemResult"].ToString();
                        //htinfo["REFLOW"] = dtTestresults.Columns.Contains("RefLow") == false ? "" : dtTestresults.Rows[i]["RefLow"].ToString();
                        //htinfo["REFHIGH"] = dtTestresults.Columns.Contains("RefHigh") == false ? "" : dtTestresults.Rows[i]["RefHigh"].ToString();
                        string strHlflag = dtTestresults.Columns.Contains("Hlflag") == false ? "" : dtTestresults.Rows[i]["Hlflag"].ToString();
                        string strTestResult = dtTestresults.Columns.Contains("ItemResult") == false ? "" : dtTestresults.Rows[i]["ItemResult"].ToString();
                        string strRefLow = dtTestresults.Columns.Contains("RefLow") == false ? "" : dtTestresults.Rows[i]["RefLow"].ToString();
                        string strRefHigh = dtTestresults.Columns.Contains("RefHigh") == false ? "" : dtTestresults.Rows[i]["RefHigh"].ToString();

                        if (strHlflag.Equals(""))
                        {
                            htinfo["HLHINT"] = "正常";
                            htinfo["ISEXCEPTION"] = "0";
                            
                        }
                        else
                        {
                            if (strHlflag.ToUpper() == "L")
                            {
                                htinfo["HLHINT"] = "偏低";
                                htinfo["ISEXCEPTION"] = "1";
                            }
                            if (strHlflag.ToUpper() == "H")
                            {
                                htinfo["HLHINT"] = "升高"; 
                                htinfo["ISEXCEPTION"] = "1";
                            }
                            else if (strHlflag.ToUpper() == "*")//结果为阴性或者阳性
                            {
                                htinfo["HLHINT"] = "异常";
                                htinfo["ISEXCEPTION"] = "1";
                            }
                        }

                        //重算参考范围   防范带有大于号和小于号的结果提示未传      
                        if (strTestResult.Contains("<"))
                        {
                            try
                            {
                                double resultTemp = Convert.ToDouble(strTestResult.Replace('<', ' ').Trim()) - 0.0000001;
                                double reflow = Convert.ToDouble(strRefLow);
                                if (resultTemp < reflow)
                                {
                                    strHlflag = "L";
                                    htinfo["HLHINT"] = "偏低";
                                    htinfo["ISEXCEPTION"] = "1";
                                }
                            }
                            catch { }
                        }
                        if (strTestResult.Contains(">"))
                        {
                            try
                            {
                                double resultTemp = Convert.ToDouble(strTestResult.Replace('>', ' ').Trim()) + 0.0000001;
                                double refhigh = Convert.ToDouble(strRefHigh);
                                if (resultTemp > refhigh)
                                {
                                    strHlflag = "H";
                                    htinfo["HLHINT"] = "升高";
                                    htinfo["ISEXCEPTION"] = "1";
                                }
                            }
                            catch { }                            
                        }                        
                        htinfo["HLFLAG"] = strHlflag;
                        htinfo["TESTRESULT"]=strTestResult;
                        htinfo["REFLOW"] = strRefLow;
                        htinfo["REFHIGH"] = strRefHigh;

                        if (dtTestresults.Columns.Contains("SingleItem"))
                            htinfo["UNIQUEID"] = dtTestresults.Rows[i]["SingleItem"].ToString();

                        if (dtTestresults.Columns.Contains("Reference"))
                            htinfo["TEXTSHOW"] = dtTestresults.Rows[i]["Reference"].ToString();

                        if (dtTestresults.Rows[i]["data_row_id"].ToString() == dtdatarow.Rows[k]["data_row_id"].ToString())
                            htinfo["BARCODE"] = dtdatarow.Rows[k]["RequestCode"].ToString();

                        SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrdertestByBarcode" } }, htinfo);
                    }
                    #endregion

                    #region 修改orderbarcode表

                    //修改orderbarcode表
                    DataTable newdt = new DataTable();
                    newdt = dtTestresults.Clone();

                    Hashtable htbarcode = new Hashtable();
                    string rowid = dtdatarow.Rows[k]["data_row_id"].ToString();
                    htbarcode["BARCODE"] = dtdatarow.Rows[k]["RequestCode"].ToString();
                    DataRow[] dataRows = dtTestresults.Select("data_row_id=" + rowid + "");

                    if (dataRows.Length <= 0)
                        continue;

                    newdt.ImportRow((DataRow)dataRows[0]);

                    htbarcode["RELEASEBYNAME"] = newdt.Columns.Contains("ReleaseByName") == false ? null : newdt.Rows[0]["ReleaseByName"].ToString();
                    htbarcode["RELEASEDATE"] = newdt.Columns.Contains("ReleaseDate") == false ? null : newdt.Rows[0]["ReleaseDate"].ToString().Substring(0, newdt.Rows[0]["ReleaseDate"].ToString().IndexOf('+')).Replace('T', ' ');
                    htbarcode["AUTHORIZEBYNAME"] = newdt.Columns.Contains("AuthorizeByName") == false ? null : newdt.Rows[0]["AuthorizeByName"].ToString();
                    htbarcode["AUTHORIZEDATE"] = newdt.Columns.Contains("AuthorizeTime") == false ? null : newdt.Rows[0]["AuthorizeTime"].ToString().Substring(0, newdt.Rows[0]["AuthorizeTime"].ToString().IndexOf('+')).Replace('T', ' ');
                    htbarcode["HASHCODE"] = newdt.Columns.Contains("HASHCODE") == false ? null : newdt.Rows[0]["HASHCODE"].ToString();
                    htbarcode["SIGNATUREBYNAME"] = newdt.Columns.Contains("SIGNATUREBYNAMES") == false ? null : newdt.Rows[0]["SIGNATUREBYNAMES"].ToString();
                    htbarcode["SIGNATUREDATE"] = newdt.Columns.Contains("SIGNATUREDATE") == false ? null : newdt.Rows[0]["SIGNATUREDATE"].ToString().Substring(0, newdt.Rows[0]["SIGNATUREDATE"].ToString().IndexOf('+')).Replace('T', ' ');


                    DateTime dt = new DateTime();
                    dt.ToUniversalTime();

                    SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrderbarcodeInfo" } }, htbarcode);

                    #endregion
                }

                bool result = this.ExecuteSqlTran(SQLlist);
                return result;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 按条码号更新到ORDERTEST表 局域网内获取
        /// </summary>
        /// <param name="ordernum">订单号</param>
        /// <param name="dt">结果表</param>
        /// <returns></returns>
        public bool UpdateOrdertestByBarcode(string ordernum, string[] arrbarcode, DataTable dt)
        {
            try
            {
                SortedList SQLlist = new SortedList(new MySort());
                for (int i = 0; i < arrbarcode.Length; i++)
                {
                    DataRow[] dataRows = dt.Select("barcode=" + arrbarcode[i]);//dt.Select("barcode is null");

                    if (dataRows.Length == 0) { continue; }

                    #region 修改orderbarcode表
                    Hashtable htbarcode = new Hashtable();
                    htbarcode["BARCODE"] = arrbarcode[i];
                    htbarcode["RELEASEBYNAME"] = dataRows[0]["RELEASEBYNAME"];
                    htbarcode["RELEASEDATE"] = dataRows[0]["RELEASEDATE"].ToString();
                    htbarcode["AUTHORIZEBYNAME"] = dataRows[0]["AUTHORIZEBYNAME"];
                    htbarcode["AUTHORIZEDATE"] = dataRows[0]["AUTHORIZETIME"].ToString();
                    htbarcode["HASHCODE"] = htbarcode["SIGNATUREDATE"] = htbarcode["SIGNATUREBYNAME"] = null;//达康LIS无数字签名

                    SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrderbarcodeInfo" } }, htbarcode);

                    #endregion

                    for (int j = 0; j < dataRows.Length; j++)
                    {
                        DataRow dr = dataRows[j];

                        #region 修改ordertest表
                        Hashtable htinfo = new Hashtable();
                        htinfo["HLFLAG"] = dr["Hlflag"];
                        htinfo["UNIT"] = dr["Unit"];
                        htinfo["TESTRESULT"] = dr["ItemResult"];
                        htinfo["HLHINT"] = "正常";
                        htinfo["ORDERNUM"] = ordernum;
                        htinfo["UNIQUEID"] = dr["SingleItem"];
                        htinfo["TEXTSHOW"] = dr["Reference"];
                        htinfo["BARCODE"] = arrbarcode[i];
                        //add by lee
                        htinfo["REFLOW"] = dr["Reflow"];
                        htinfo["REFHIGH"] = dr["Refhigh"];

                        if (!string.IsNullOrEmpty(htinfo["HLFLAG"].ToString().Trim()))
                        {
                            htinfo["ISEXCEPTION"] = "1";
                            htinfo["HLHINT"] = dr["Hlflag"].ToString() == "H" ? "偏高" : "偏低";
                        }
                        else
                        {
                            htinfo["ISEXCEPTION"] = "0";
                        }
                        SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrdertestByBarcode" } }, htinfo);
                        #endregion
                    }
                }

                bool result = this.ExecuteSqlTran(SQLlist);
                return result;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 按条码号查询ORDERTEST表的所有检验记录是否取到结果
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public int SelectTestResultByOrdernum(string ordernum)
        {
            try
            {
                return int.Parse(this.selectIList("Order.SelectTestResultByOrdernum", ordernum)[0].ToString());
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>按条码号查询ORDERTEST表的所有物理检验有结果lee
        /// 
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns>True 有结果为空的物理检查项 false 全有结果</returns>
        public bool SelectWLResultByOrdernum(string ordernum)
        {        
            DataTable dtTemp = this.selectDS("Order.SelectWLResultByOrdernum", ordernum).Tables[0];
            if (dtTemp.Rows[0]["counts"].ToString() == "0")
            {
                return false;
            }
            else
                return true;
        }
    }
}
