using System;
using System.Collections.Generic;
using System.Linq;
using FastReport;
using System.Data;
using daan.service.common;
using daan.service.order;
using daan.service.dict;
using System.Collections;

namespace daan.service.report
{
    public class ReportService : BaseService
    {
        Report fReport = new Report();
        DataSet dsReportData = new DataSet();//报表的数据源
        readonly OrdersService orderService = new OrdersService();
        readonly DictreporttemplateService repService = new DictreporttemplateService();
        string strParth = "";

        static string reporttemplateCode = "";

        #region 获取体检报告数据
        public DataSet GetReportData(string order_num,string repID)
        {
            DataSet ds = new DataSet();
            if (repID == ((int)ParamStatus.ReportTypeStatus.Normal).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.TM15).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.HPV).ToString()) //常规报告模板ID或TM15报告模板
            {
                ds = GetCommonData(order_num);
            }
            else if (repID == ((int)ParamStatus.ReportTypeStatus.C14).ToString()||repID==((int)ParamStatus.ReportTypeStatus.C13).ToString())//C14    C13
            {
                ds = GetC14Data(order_num);
            }
            return ds;
        }

        private DataSet GetCommonData(string order_num)
        {
            DataSet dsReport = new DataSet();
            try
            {
                DataTable dtRepTitle = selectDS("report.GetDataTitle", order_num).Tables[0];//取得表头信息
                DataTable dtRepImportantSigns = selectDS("report.GetDataImportantSigns", order_num).Tables[0];//本次体检结果
                DataTable dtRepMustExam = selectDS("report.GetDataMustExam", order_num).Tables[0];//每次体检必须检查的项目
                DataTable dtRepRecommendExam = selectDS("report.GetDataRecommendExam", order_num).Tables[0];//下次体检特别推荐的项目
                //历史体检日期
                DataTable dtRepExamComparedHead = selectDS("report.GetDataExamComparedHead", order_num).Tables[0];//历次体检对比日期
                Hashtable ht = new Hashtable();
                for (int i = 0; i < 5; i++)
                {
                    ht["ordernum" + (i + 1)] = "";
                    ht["date" + (i + 1)] = "";
                    if (i < dtRepExamComparedHead.Rows.Count)
                    {
                        ht["ordernum" + (i + 1)] = dtRepExamComparedHead.Rows[i]["ordernum"];
                        ht["date" + (i + 1)] = dtRepExamComparedHead.Rows[i]["finishdate"];
                    }
                }
                //根据客户ID获取历史体检数据
                DataTable dtRepExamCompared = selectDS("report.GetDataExamCompared", ht).Tables[0];//历次体检对比     
                DataTable dtRepDiseaseGuide = selectDS("report.GetDataDiseaseGuide", order_num).Tables[0];//解读与建议
                DataTable dtOrderresultcomment = new OrderresultcommentService().SelectOrderresultcommentDs(order_num);//总检评价
                //处理年龄
                for (int i = 0; i < dtRepTitle.Rows.Count; i++)
                {
                    dtRepTitle.Rows[i]["age"] = SetAge(dtRepTitle.Rows[i]["age"]);
                    if (dtRepTitle.Rows[i]["barcode"].ToString().Substring(0, 3) == "800")
                        dtRepTitle.Rows[i]["barcode"] = string.Empty;
                }
                DataTable dtRepTitlenew = dtRepTitle.Copy();
                DataTable dtRepImportantSignsnew = dtRepImportantSigns.Copy();
                DataTable dtRepMustExamnew = dtRepMustExam.Copy();
                DataTable dtRepRecommendExamnew = dtRepRecommendExam.Copy();
                DataTable dtRepExamComparednew = dtRepExamCompared.Copy();
                DataTable dtRepDiseaseGuidenew = dtRepDiseaseGuide.Copy();
                DataTable dtOrderresultcommentnew = dtOrderresultcomment.Copy();

                dtRepTitlenew.TableName = "dtRepTitle";
                dtRepImportantSignsnew.TableName = "dtRepImportantSigns";
                dtRepMustExamnew.TableName = "dtRepMustExam";
                dtRepRecommendExamnew.TableName = "dtRepRecommendExam";
                dtRepExamComparednew.TableName = "dtRepExamCompared";
                dtRepDiseaseGuidenew.TableName = "dtRepDiseaseGuide";
                dtOrderresultcommentnew.TableName = "dtOrderresultcomment";

                dsReport.Tables.Add(dtRepTitlenew);
                dsReport.Tables.Add(dtRepImportantSignsnew);
                dsReport.Tables.Add(dtRepMustExamnew);
                dsReport.Tables.Add(dtRepRecommendExamnew);
                dsReport.Tables.Add(dtRepExamComparednew);
                dsReport.Tables.Add(dtRepDiseaseGuidenew);
                dsReport.Tables.Add(dtOrderresultcommentnew);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dsReport;
        }

        private DataSet GetC14Data(string order_num)
        {
            DataSet dsReport = new DataSet();
            try
            {
                DataTable dtRepTitle = selectDS("report.GetDataTitle", order_num).Tables[0];//取得表头信息
                DataTable dtRepImportantSigns = selectDS("report.GetC14TestResult", order_num).Tables[0];//取得检测项目结果信息
                //处理年龄
                for (int i = 0; i < dtRepTitle.Rows.Count; i++)
                {
                    //C14年龄特殊处理：199显示为“成人”，198显示为“未知”,“197”显示为“”
                    if (dtRepTitle.Rows[i]["age"].ToString().Contains("199"))
                    {
                        dtRepTitle.Rows[i]["age"] = "成人";
                    }
                    else if (dtRepTitle.Rows[i]["age"].ToString().Contains("198"))
                    {
                        dtRepTitle.Rows[i]["age"] = "未知";
                    }
                    else if (dtRepTitle.Rows[i]["age"].ToString().Contains("197"))
                    {
                        dtRepTitle.Rows[i]["age"] = "";
                    }
                    else
                    {
                        dtRepTitle.Rows[i]["age"] = SetAge(dtRepTitle.Rows[i]["age"]);
                    }
                    if (dtRepTitle.Rows[i]["barcode"].ToString().Substring(0, 3) == "800")
                        dtRepTitle.Rows[i]["barcode"] = string.Empty;
                }
                DataTable dtRepTitlenew = dtRepTitle.Copy();
                dtRepTitlenew.TableName = "dtRepTitle";
                DataTable dtRepImportantSignsnew = dtRepImportantSigns.Copy();
                dtRepImportantSignsnew.TableName = "dtRepImportantSigns";
                dsReport.Tables.Add(dtRepTitlenew);
                dsReport.Tables.Add(dtRepImportantSignsnew);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dsReport;
        }
        #endregion

        #region 传入过来的体检流水号和模板编码,然后设置报表
        /// <summary>
        /// 传入过来的体检流水号和模板编码,然后设置报表
        /// </summary>
        /// <param name="order_num"></param>
        /// <param name="reportemplateCode"></param>
        /// <param name="repID"></param>
        /// <param name="ds"></param>
        /// <param name="parth"></param>
        /// <param name="refReportData"></param>
        /// <param name="isprint">1为打印2为预览</param>
        /// <returns></returns>
        public Report GetReport(string order_num, string reportemplateCode, string repID, DataSet ds, string parth, out DataSet refReportData, int isprint)
        {
            strParth = parth;
            reporttemplateCode = reportemplateCode;
            if (repID == ((int)ParamStatus.ReportTypeStatus.Normal).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.TM15).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.HPV).ToString()) //常规报告模板ID或TM15报告模板
            {
                fReport = GetRegisteredCommonDataReport(order_num, repID, isprint);
            }
            else if (repID == ((int)ParamStatus.ReportTypeStatus.C14).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.C13).ToString())//C14   C13
            {
                fReport=GetRegisteredC14DataReport(order_num,repID,isprint);
            }
            //else if (repID == ((int)ParamStatus.ReportTypeStatus.HPV).ToString())//HPV报告模板ID
            //{
            //    fReport = GetRegisteredHPVDataReport(order_num, isprint);
            //}
            else if (repID == ((int)ParamStatus.ReportTypeStatus.Financial).ToString())//财务结算报告
            {
                fReport = GetRegisteredFinancialDataReport(ds, isprint);
            }
            else if (repID == ((int)ParamStatus.ReportTypeStatus.ChcekOrder).ToString())//体检指引单
            {
                fReport = GetHealthGuidelDataReport(ds, isprint);

            }
            else if (repID == ((int)ParamStatus.ReportTypeStatus.MoneyOrder).ToString())//收费收据单
            {
                fReport = GetMoneyReceiptDataReport(ds, isprint);
            }
            else if (repID == ((int)ParamStatus.ReportTypeStatus.GroupOrder).ToString())//团检报告
            {
                fReport = GetGrouptDataReport(ds, isprint);
            }
            refReportData = dsReportData;
            return fReport;
        }
        //根据条码号和类型获取报表信息
        public DataSet GetReportDataByType(string order_num, string type)
        {
            DataSet ds = new DataSet();

            if (type == "BG_1001") //常规报告模板ID或TM15报告模板
            {
                ds = GetCommonData(order_num);
            }
            else if (type == "BG_1006")//C14    C13
            {
                ds = GetC14Data(order_num);
            }
            return ds;
        }
        #endregion

        #region 取得常规报告和TM15报告
        private Report GetRegisteredCommonDataReport(string order_num, string repID, int isprint)
        {
            try
            {
                DataTable dtRepTitle = selectDS("report.GetDataTitle", order_num).Tables[0];//取得表头信息
                //DataTable dtRepCurrentProblem = this.selectDS("report.GetDataCurrentProblem", order_num).Tables[0];//本次发现问题
                DataTable dtRepImportantSigns = selectDS("report.GetDataImportantSigns", order_num).Tables[0];//本次体检结果
                DataTable dtRepMustExam = selectDS("report.GetDataMustExam", order_num).Tables[0];//每次体检必须检查的项目
                DataTable dtRepRecommendExam = selectDS("report.GetDataRecommendExam", order_num).Tables[0];//下次体检特别推荐的项目\


                //历史体检日期
                DataTable dtRepExamComparedHead = selectDS("report.GetDataExamComparedHead", order_num).Tables[0];//历次体检对比日期
                Hashtable ht = new Hashtable();
                for (int i = 0; i < 5; i++)
                {
                    ht["ordernum" + (i + 1)] = "";
                    ht["date" + (i + 1)] = "";
                    if (i < dtRepExamComparedHead.Rows.Count)
                    {
                        ht["ordernum" + (i + 1)] = dtRepExamComparedHead.Rows[i]["ordernum"];
                        ht["date" + (i + 1)] = dtRepExamComparedHead.Rows[i]["finishdate"];
                    }
                }
                //根据客户ID获取历史体检数据
                DataTable dtRepExamCompared = selectDS("report.GetDataExamCompared", ht).Tables[0];//历次体检对比               

                DataTable dtRepDiseaseGuide = selectDS("report.GetDataDiseaseGuide", order_num).Tables[0];//解读与建议
                DataTable dtOrderresultcomment = new OrderresultcommentService().SelectOrderresultcommentDs(order_num);//总检评价
                //DataTable dtRepAbnormalInterpretation = this.selectDS("report.GetDataAbnormalInterpretation", order_num).Tables[0];//异常解读               
                //处理年龄
                for (int i = 0; i < dtRepTitle.Rows.Count; i++)
                {
                    dtRepTitle.Rows[i]["age"] = SetAge(dtRepTitle.Rows[i]["age"]);
                    if (dtRepTitle.Rows[i]["barcode"].ToString().Substring(0, 3) == "800")
                        dtRepTitle.Rows[i]["barcode"] = string.Empty;
                }
                DataTable dtRepTitlenew = dtRepTitle.Copy();
                //DataTable dtRepCurrentProblemnew = dtRepCurrentProblem.Copy();
                DataTable dtRepImportantSignsnew = dtRepImportantSigns.Copy();
                DataTable dtRepMustExamnew = dtRepMustExam.Copy();
                DataTable dtRepRecommendExamnew = dtRepRecommendExam.Copy();
                DataTable dtRepExamComparednew = dtRepExamCompared.Copy();
                DataTable dtRepDiseaseGuidenew = dtRepDiseaseGuide.Copy();
                DataTable dtOrderresultcommentnew = dtOrderresultcomment.Copy();

                //DataTable dtRepAbnormalInterpretationnew = dtRepAbnormalInterpretation.Copy();
                dtRepTitlenew.TableName = "dtRepTitle";
                //dtRepCurrentProblemnew.TableName = "dtRepCurrentProblem";
                dtRepImportantSignsnew.TableName = "dtRepImportantSigns";
                dtRepMustExamnew.TableName = "dtRepMustExam";
                dtRepRecommendExamnew.TableName = "dtRepRecommendExam";
                dtRepExamComparednew.TableName = "dtRepExamCompared";
                dtRepDiseaseGuidenew.TableName = "dtRepDiseaseGuide";
                dtOrderresultcommentnew.TableName = "dtOrderresultcomment";
                //dtRepAbnormalInterpretationnew.TableName = "dtRepAbnormalInterpretation";

                dsReportData = new DataSet();

                dsReportData.Tables.Add(dtRepTitlenew);
                //dsReportData.Tables.Add(dtRepCurrentProblemnew);
                dsReportData.Tables.Add(dtRepImportantSignsnew);
                dsReportData.Tables.Add(dtRepMustExamnew);
                dsReportData.Tables.Add(dtRepRecommendExamnew);
                dsReportData.Tables.Add(dtRepExamComparednew);
                dsReportData.Tables.Add(dtRepDiseaseGuidenew);
                dsReportData.Tables.Add(dtOrderresultcommentnew);
                //dsReportData.Tables.Add(dtRepAbnormalInterpretationnew);
                fReport = new Report();
                fReport.Load(string.Format("{0}{1}.frx", strParth, reporttemplateCode));
                    

                //打印时不用准备和注册数据
                if (isprint != 1)
                {               
                    fReport.RegisterData(dtRepTitle, "dtRepTitle");//注册表头首页信息
                    //fReport.RegisterData(dtRepCurrentProblem, "dtRepCurrentProblem");//注册本次发现问题
                    fReport.RegisterData(dtRepImportantSigns, "dtRepImportantSigns");//注册重要指标情况
                    fReport.RegisterData(dtRepMustExam, "dtRepMustExam");//注册每次体检必须检查的项目
                    fReport.RegisterData(dtRepRecommendExam, "dtRepRecommendExam");//注册下次体检特别推荐的项目
                    fReport.RegisterData(dtRepExamCompared, "dtRepExamCompared");//注册历次体检对比
                    fReport.RegisterData(dtRepDiseaseGuide, "dtRepDiseaseGuide");//注册疾病指南
                    fReport.RegisterData(dtOrderresultcomment, "dtOrderresultcomment");//注册总体评价
                    //fReport.RegisterData(dtRepAbnormalInterpretation, "dtRepAbnormalInterpretation");//注册异常解读
                    fReport.Prepare(true);
                }
                return fReport;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //return fReport;
            }
        }
        #endregion

        #region 取得C14胃部检查报告
        private Report GetRegisteredC14DataReport(string order_num, string repID, int isprint)
        { 
            try
            {
                DataTable dtRepTitle = selectDS("report.GetDataTitle", order_num).Tables[0];//取得表头信息
                DataTable dtRepImportantSigns = selectDS("report.GetC14TestResult", order_num).Tables[0];//取得检测项目结果信息
                //处理年龄
                for (int i = 0; i < dtRepTitle.Rows.Count; i++)
                {
                    //C14年龄特殊处理：199显示为“成人”，198显示为“未知”,“197”显示为“”
                    if (dtRepTitle.Rows[i]["age"].ToString().Contains("199"))
                    {
                        dtRepTitle.Rows[i]["age"] = "成人";
                    }
                    else if (dtRepTitle.Rows[i]["age"].ToString().Contains("198"))
                    {
                        dtRepTitle.Rows[i]["age"] = "未知";
                    }
                    else if (dtRepTitle.Rows[i]["age"].ToString().Contains("197"))
                    {
                        dtRepTitle.Rows[i]["age"] = "";
                    }
                    else
                    {
                        dtRepTitle.Rows[i]["age"] = SetAge(dtRepTitle.Rows[i]["age"]);
                    }
                    if (dtRepTitle.Rows[i]["barcode"].ToString().Substring(0, 3) == "800")
                        dtRepTitle.Rows[i]["barcode"] = string.Empty;
                }
                DataTable dtRepTitlenew = dtRepTitle.Copy();
                dtRepTitlenew.TableName = "dtRepTitle";
                DataTable dtRepImportantSignsnew = dtRepImportantSigns.Copy();
                dtRepImportantSignsnew.TableName = "dtRepImportantSigns";
                dsReportData = new DataSet();
                dsReportData.Tables.Add(dtRepTitlenew);
                dsReportData.Tables.Add(dtRepImportantSignsnew);

                fReport = new Report();
                fReport.Load(string.Format("{0}{1}.frx", strParth, reporttemplateCode));

                //设置参数
                fReport.SetParameterValue("ParaName", dtRepTitle.Rows[0]["realname"]);
                fReport.SetParameterValue("ParaBarcode", dtRepTitle.Rows[0]["barcode"]);
                fReport.SetParameterValue("ParaOrdernum",dtRepTitle.Rows[0]["ordernum"]);
                fReport.SetParameterValue("ParaFinishDate", dtRepTitle.Rows[0]["finishdate"]);
                fReport.SetParameterValue("ParaSex", dtRepTitle.Rows[0]["sex"]);
                fReport.SetParameterValue("ParaLabDes", dtRepTitle.Rows[0]["LABDESCRIPTION"]);
                fReport.SetParameterValue("ParaAge", dtRepTitle.Rows[0]["age"]);
                fReport.SetParameterValue("ParaCustomerName", dtRepTitle.Rows[0]["customername"]);
                
                //打印时不用准备和注册数据
                if (isprint != 1)
                {
                    fReport.RegisterData(dtRepTitle, "dtRepTitle");//注册表头首页信息
                    fReport.RegisterData(dtRepImportantSigns, "dtRepImportantSigns");//注册检测项目信息及结果
                    fReport.Prepare(true);
                }
                return fReport;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 取得HPV报告
        private Report GetRegisteredHPVDataReport(string order_num, int isprint)
        {
            try
            {
                fReport = new Report();
                DataTable dtTitle = selectDS("report.GetTitleByCustomer", order_num).Tables[0];//取报告标题
                if (dtTitle.Rows.Count == 0)
                {
                    dtTitle = selectDS("report.GetTitleByDictlab", order_num).Tables[0];//取报告标题
                }
                else
                {
                    if (dtTitle.Rows[0][0].ToString() == "" || dtTitle.Rows[0][0] == null)
                    {
                        dtTitle = selectDS("report.GetTitleByDictlab", order_num).Tables[0];//取报告标题
                    }
                }
                DataTable dtRepHPVTitle = selectDS("report.GetDataHPVTitle", order_num).Tables[0];//取得HPV表头信息
                DataTable dtRepHPVResult = selectDS("report.GetDataHPVResult", order_num).Tables[0];//取得HPV结果信息
                DataTable dtRepHPVSuggestion = selectDS("report.GetDataHPVSuggestion", order_num).Tables[0];//取得HPV报告风险分析与建议内容
                DataTable dtRepHPVSuggestionTestResult = selectDS("report.GetDataHPVSuggestionTestResult", order_num).Tables[0];//取得HPV报告风险分析与建议的检验项目内容
                //处理年龄
                for (int i = 0; i < dtRepHPVTitle.Rows.Count; i++)
                {
                    dtRepHPVTitle.Rows[i]["age"] = SetAge(dtRepHPVTitle.Rows[i]["age"]);
                }
                fReport.Load(string.Format("{0}{1}.frx", strParth, reporttemplateCode));
                DataTable dtTitlenew = dtTitle.Copy();
                DataTable dtRepHPVTitlenew = dtRepHPVTitle.Copy();
                DataTable dtRepHPVResultnew = dtRepHPVResult.Copy();
                DataTable dtRepHPVSuggestionnew = dtRepHPVSuggestion.Copy();
                DataTable dtRepHPVSuggestionTestResultnew = dtRepHPVSuggestionTestResult.Copy();
                dtTitlenew.TableName = "dtTitle";
                dtRepHPVTitlenew.TableName = "dtRepHPVTitle";
                dtRepHPVResultnew.TableName = "dtRepHPVResult";
                dtRepHPVSuggestionnew.TableName = "dtRepHPVSuggestion";
                dtRepHPVSuggestionTestResultnew.TableName = "dtRepHPVSuggestionTestResult";
                dsReportData = new DataSet();
                dsReportData.Tables.Add(dtTitlenew);
                dsReportData.Tables.Add(dtRepHPVTitlenew);
                dsReportData.Tables.Add(dtRepHPVResultnew);
                dsReportData.Tables.Add(dtRepHPVSuggestionnew);
                dsReportData.Tables.Add(dtRepHPVSuggestionTestResultnew);
                if (isprint != 1)
                {
                    fReport.RegisterData(dtTitle, "dtTitle");
                    fReport.RegisterData(dtRepHPVTitle, "dtRepHPVTitle");//注册HPV表头信息
                    fReport.RegisterData(dtRepHPVResult, "dtRepHPVResult");//注册HPV结果信息
                    fReport.RegisterData(dtRepHPVSuggestion, "dtRepHPVSuggestion");//注册HPV报告风险分析与建议内容
                    fReport.RegisterData(dtRepHPVSuggestionTestResult, "dtRepHPVSuggestionTestResult");//注册HPV报告风险分析与建议内容
                    fReport.Prepare(true);
                }
                return fReport;
            }
            catch
            {
                return fReport;
            }
        }
        #endregion

        #region 取得财务结算报告
        private Report GetRegisteredFinancialDataReport(DataSet ds, int isprint)
        {
            DataTable dtTitle = new DataTable("dtTitle");//报告标题
            DataTable dtRepFinancialTitle = new DataTable();
            DataTable dtRepFinancialResult = new DataTable();
            if (ds.Tables.Count > 0)
            {
                dtTitle = ds.Tables["dtTitle"];
                dtRepFinancialTitle = ds.Tables["headinfo"];
                dtRepFinancialResult = ds.Tables["tbdetail"];
            }
            if (dtTitle.Columns.Count == 0)
            {
                dtTitle.Columns.Add("titleName", typeof(string));//报告标题
                DataRow dr = dtTitle.NewRow();
                dr["titleName"] = "广州达安临床检验中心结算单";
                dtTitle.Rows.Add(dr);
            }
            if (dtRepFinancialTitle.Columns.Count == 0)
            {


                dtRepFinancialTitle.Columns.Add("customername", typeof(string));//客户名称
                dtRepFinancialTitle.Columns.Add("checkbillname", typeof(string));//清单核对人
                dtRepFinancialTitle.Columns.Add("salename", typeof(string));//销售人员
                dtRepFinancialTitle.Columns.Add("phone", typeof(string));//电话
                dtRepFinancialTitle.Columns.Add("fax", typeof(string));//传真
                dtRepFinancialTitle.Columns.Add("website", typeof(string));//网址
                dtRepFinancialTitle.Columns.Add("totalstandardprice", typeof(string));//标准收费
                dtRepFinancialTitle.Columns.Add("totalfinalprice", typeof(string));//实际收费
                dtRepFinancialTitle.Columns.Add("begindate", typeof(string));//开始时间
                dtRepFinancialTitle.Columns.Add("enddate", typeof(string));//结束时间
                dtRepFinancialTitle.Columns.Add("customertype", typeof(string));//表头信息                
            }
            if (dtRepFinancialResult.Columns.Count == 0)
            {
                dtRepFinancialResult.Columns.Add("Ordernum", typeof(string));//体检流水号
                dtRepFinancialResult.Columns.Add("Realname", typeof(string));//姓名
                dtRepFinancialResult.Columns.Add("Productname", typeof(string));//项目
                dtRepFinancialResult.Columns.Add("Standardprice", typeof(decimal));//标准价格
                dtRepFinancialResult.Columns.Add("Finalprice", typeof(decimal));//实收金额
                dtRepFinancialResult.Columns.Add("Remark", typeof(string));//备注
                dtRepFinancialResult.Columns.Add("Orderenterdate", typeof(string));//备注               
            }


            try
            {
                fReport = new Report();
                fReport.Load(string.Format("{0}{1}.frx", strParth, reporttemplateCode));
                DataTable dtRepFinancialTitlenew = dtRepFinancialTitle.Copy();
                DataTable dtRepFinancialResultnew = dtRepFinancialResult.Copy();
                DataTable dtTitlenew = dtTitle.Copy();
                dtTitlenew.TableName = "dtTitle";
                dtRepFinancialTitlenew.TableName = "dtRepFinancialTitle";
                dtRepFinancialResultnew.TableName = "dtRepFinancialResult";

                dsReportData = new DataSet();
                dsReportData.Tables.Add(dtRepFinancialTitlenew);
                dsReportData.Tables.Add(dtRepFinancialResultnew);
                dsReportData.Tables.Add(dtTitlenew);

                if (isprint != 1)
                {
                    dtRepFinancialTitle.TableName = "dtRepFinancialTitle";
                    dtRepFinancialResult.TableName = "dtRepFinancialResult";
                    fReport.RegisterData(dtRepFinancialTitle, "dtRepFinancialTitle");//注册财务结算单表头信息
                    fReport.RegisterData(dtRepFinancialResult, "dtRepFinancialResult");//注册财务结算单结果信息
                    fReport.RegisterData(dtTitle, "dtTitle");
                    fReport.Prepare(true);
                }
                return fReport;
            }
            catch
            {
                return fReport;
            }
        }
        #endregion

        #region 取得体检指引单
        private Report GetHealthGuidelDataReport(DataSet ds, int isprint)
        {
            DataTable dtTitle = new DataTable("dtTitle");//报告标题
            DataTable dt = new DataTable();
            DataTable dtHealthGuideTitle = new DataTable();


            if (ds.Tables.Count > 0)
            {
                dtTitle = ds.Tables["dtTitle"];
                dt = ds.Tables["dtHealthGuideResult"];
                dtHealthGuideTitle = ds.Tables["dtHealthGuideTitle"];
            }
            if (dtTitle.Columns.Count == 0)
            {
                dtTitle.Columns.Add("titleName", typeof(string));//报告标题
                DataRow dr = dtTitle.NewRow();  
                dr["titleName"] = "广州达安临床检验中心";
                dtTitle.Rows.Add(dr);
            }
            if (dtHealthGuideTitle.Columns.Count == 0)
            {

                dtHealthGuideTitle.Columns.Add("customername", typeof(string));//体检单位
                dtHealthGuideTitle.Columns.Add("section", typeof(string));//部门
                dtHealthGuideTitle.Columns.Add("ordernum", typeof(string));//登记号
                dtHealthGuideTitle.Columns.Add("createdate", typeof(string));//登记日期
                dtHealthGuideTitle.Columns.Add("realname", typeof(string));//性名
                dtHealthGuideTitle.Columns.Add("sex", typeof(string));//性别
                dtHealthGuideTitle.Columns.Add("age", typeof(string));//年龄
                dtHealthGuideTitle.Columns.Add("ismarried", typeof(string));//婚姻
                dtHealthGuideTitle.Columns.Add("addres", typeof(string));//住址
                dtHealthGuideTitle.Columns.Add("idnumber", typeof(string));//身份证号
                dtHealthGuideTitle.Columns.Add("enterby", typeof(string));//登记人
                dtHealthGuideTitle.Columns.Add("printdate", typeof(string));//打印日期               
            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("testname", typeof(string));//体检项目
                dt.Columns.Add("labdepttype", typeof(string));//科室
                dt.Columns.Add("operationremark", typeof(string));//提示信息
            }
            try
            {
                fReport = new Report();


                DataTable dtHealthGuideResult = dt;//取得财务结算单结果信息
                fReport.Load(string.Format("{0}{1}.frx", strParth, reporttemplateCode));
                dsReportData = new DataSet();
                DataTable dtHealthGuideTitlenew = dtHealthGuideTitle.Copy();
                DataTable dtHealthGuideResultnew = dtHealthGuideResult.Copy();
                DataTable dtTitlenew = dtTitle.Copy();
                dtTitlenew.TableName = "dtTitle";
                dtHealthGuideTitlenew.TableName = "dtHealthGuideTitle";
                dtHealthGuideResultnew.TableName = "dtHealthGuideResult";
                dsReportData.Tables.Add(dtHealthGuideTitlenew);
                dsReportData.Tables.Add(dtHealthGuideResultnew);
                dsReportData.Tables.Add(dtTitlenew);
                if (isprint != 1)
                {
                    fReport.RegisterData(dtHealthGuideTitle, "dtHealthGuideTitle");//注册财务结算单表头信息
                    fReport.RegisterData(dtHealthGuideResult, "dtHealthGuideResult");//注册财务结算单结果信息
                    fReport.RegisterData(dtTitle, "dtTitle");
                    fReport.Prepare(true);
                }
                return fReport;
            }
            catch
            {
                return fReport;
            }
        }
        #endregion

        #region 取得收费收据单
        private Report GetMoneyReceiptDataReport(DataSet ds, int isprint)
        {
            DataTable dtTitle = new DataTable("dtTitle");//报告标题
            DataTable dtMoneyReceiptResult = new DataTable();
            if (ds.Tables.Count > 0)
            {
                dtMoneyReceiptResult = ds.Tables["dtHealthGuideTitle"];
                dtTitle = ds.Tables["dtTitle"];
            }
            if (dtMoneyReceiptResult.Columns.Count == 0)
            {

                dtMoneyReceiptResult.Columns.Add("Ordernum", typeof(string));//体检流水号
                dtMoneyReceiptResult.Columns.Add("Realname", typeof(string));//客户姓名
                dtMoneyReceiptResult.Columns.Add("Productname", typeof(string));//检测项目
                dtMoneyReceiptResult.Columns.Add("Finalprice", typeof(string));//金额
                dtMoneyReceiptResult.Columns.Add("Billbyname", typeof(string));//受理人
                dtMoneyReceiptResult.Columns.Add("Billbydate", typeof(string));//受理人               
            }
            if (dtTitle.Columns.Count == 0)
            {
                dtTitle.Columns.Add("titleName", typeof(string));//报告标题
                DataRow dr = dtTitle.NewRow();
                dr["titleName"] = "广州达安临床检验中心";
                dtTitle.Rows.Add(dr);
            }

            try
            {
                fReport = new Report();
                fReport.Load(string.Format("{0}{1}.frx", strParth, reporttemplateCode));
                dsReportData = new DataSet();
                DataTable dtMoneyReceiptResultnew = dtMoneyReceiptResult.Copy();
                dtMoneyReceiptResultnew.TableName = "dtMoneyReceiptResult";
                DataTable dtTitlenew = dtTitle.Copy();
                dtTitlenew.TableName = "dtTitle";

                dsReportData.Tables.Add(dtTitlenew);
                dsReportData.Tables.Add(dtMoneyReceiptResultnew);
                dtMoneyReceiptResult.TableName = "dtMoneyReceiptResult";
                if (isprint != 1)
                {
                    fReport.RegisterData(dtMoneyReceiptResult, "dtMoneyReceiptResult");//注册信息
                    fReport.RegisterData(dtTitle, "dtTitle");//注册信息
                    fReport.Prepare(true);
                }
                return fReport;
            }
            catch
            {
                return fReport;
            }
        }
        #endregion

        #region 取得团检报告
        private Report GetGrouptDataReport(DataSet ds, int isprint)
        {
            DataTable dtTitle = new DataTable("dtTitle");//报告标题
            DataTable dtGroupTitle = new DataTable("dtGroupTitle");//团检报告头
            DataTable dtGroupBasicMessage = new DataTable("dtGroupBasicMessage");//团检体检基本信息
            DataTable dtGroupAgeSexColumnResult = new DataTable("dtGroupAgeSexColumnResult");//参检人员年龄和性别柱状图
            DataTable dtGroupAgeSexCakeResult = new DataTable("dtGroupAgeSexCakeResult");//参检人员年龄和性别饼状图
            DataTable dtGroupAgeSexCakeLeftResult = new DataTable("dtGroupAgeSexCakeLeftResult");//参检人员年龄和性别饼状图左侧表格
            DataTable dtGroupAllDiseasePercent = new DataTable("dtGroupAllDiseasePercent");//所有员工前十种疾病比例
            DataTable dtGroupMaleDiseasePercent = new DataTable("dtGroupMaleDiseasePercent");//所有男性员工前十种疾病比例
            DataTable dtGroupFamaleDiseasePercent = new DataTable("dtGroupFamaleDiseasePercent");//所有女性员工前十种疾病比例
            DataTable dtGroupChronicDiseasesCompare = new DataTable("dtGroupChronicDiseases");//慢性病历年比较
            DataTable dtGroupHealthCompare = new DataTable("dtGroupHealthCompare");//健康问题历年比较
            DataTable dtGroupHealthSuggest = new DataTable("dtGroupHealthSuggest");//健康问题分析与建议
            DataTable dtGroupHealthAnormal = new DataTable("dtGroupHealthAnormal");//体检异常统计
            DataTable dtGroupAllResult = new DataTable("dtGroupAllResult");//全体员工体检结果汇总
            DataTable dtGroupAnExamination = new DataTable("dtGroupAnExamination");//已预约未体检人员
            DataTable dtGroupAnFinish = new DataTable("dtGroupAnFinish");//已体检而未完成
            DataTable dtGroupImportResult1 = new DataTable("dtGroupImportResult1");//重要指标检查结果分组一
            DataTable dtGroupImportResult2 = new DataTable("dtGroupImportResult2");
            DataTable dtGroupImportResult3 = new DataTable("dtGroupImportResult3");
            DataTable dtGroupImportResult4 = new DataTable("dtGroupImportResult4");
            DataTable dtGroupImportResult5 = new DataTable("dtGroupImportResult5");
            DataTable dtGroupImportResult6 = new DataTable("dtGroupImportResult6");
            DataTable dtGroupImportResult7 = new DataTable("dtGroupImportResult7");
            DataTable dtGroupImportResult8 = new DataTable("dtGroupImportResult8");
            DataTable dtGroupImportResult9 = new DataTable("dtGroupImportResult9");
            DataTable dtGroupImportResult10 = new DataTable("dtGroupImportResult10");
            DataTable dtGroupImportResult11 = new DataTable("dtGroupImportResult11");
            DataTable dtGroupImportResult12 = new DataTable("dtGroupImportResult12");
            DataTable dtGroupImportResult13 = new DataTable("dtGroupImportResult13");
            DataTable dtGroupImportResult14 = new DataTable("dtGroupImportResult14");
            DataTable dtGroupImportResult15 = new DataTable("dtGroupImportResult15");
            DataTable dtGroupImportResult16 = new DataTable("dtGroupImportResult16");
            DataTable dtGroupImportResult17 = new DataTable("dtGroupImportResult17");
            DataTable dtGroupImportResult18 = new DataTable("dtGroupImportResult18");
            DataTable dtGroupImportResult19 = new DataTable("dtGroupImportResult19");
            DataTable dtGroupImportResult20 = new DataTable("dtGroupImportResult20");
            DataTable dtGroupImportTable1 = new DataTable("dtGroupImportTable1");//重要指标分组后的表格显示数据
            DataTable dtGroupImportTable2 = new DataTable("dtGroupImportTable2");
            DataTable dtGroupImportTable3 = new DataTable("dtGroupImportTable3");
            DataTable dtGroupImportTable4 = new DataTable("dtGroupImportTable4");
            DataTable dtGroupImportTable5 = new DataTable("dtGroupImportTable5");
            DataTable dtGroupImportTable6 = new DataTable("dtGroupImportTable6");
            DataTable dtGroupImportTable7 = new DataTable("dtGroupImportTable7");
            DataTable dtGroupImportTable8 = new DataTable("dtGroupImportTable8");
            DataTable dtGroupImportTable9 = new DataTable("dtGroupImportTable9");
            DataTable dtGroupImportTable10 = new DataTable("dtGroupImportTable10");
            DataTable dtGroupImportTable11 = new DataTable("dtGroupImportTable11");
            DataTable dtGroupImportTable12 = new DataTable("dtGroupImportTable12");
            DataTable dtGroupImportTable13 = new DataTable("dtGroupImportTable13");
            DataTable dtGroupImportTable14 = new DataTable("dtGroupImportTable14");
            DataTable dtGroupImportTable15 = new DataTable("dtGroupImportTable15");
            DataTable dtGroupImportTable16 = new DataTable("dtGroupImportTable16");
            DataTable dtGroupImportTable17 = new DataTable("dtGroupImportTable17");
            DataTable dtGroupImportTable18 = new DataTable("dtGroupImportTable18");
            DataTable dtGroupImportTable19 = new DataTable("dtGroupImportTable19");
            DataTable dtGroupImportTable20 = new DataTable("dtGroupImportTable20");
            DataTable dtGroupImportTableTitle1 = new DataTable("dtGroupImportTableTitle1");//重要指标分组后的表格显示数据表头
            DataTable dtGroupImportTableTitle2 = new DataTable("dtGroupImportTableTitle2");
            DataTable dtGroupImportTableTitle3 = new DataTable("dtGroupImportTableTitle3");
            DataTable dtGroupImportTableTitle4 = new DataTable("dtGroupImportTableTitle4");
            DataTable dtGroupImportTableTitle5 = new DataTable("dtGroupImportTableTitle5");
            DataTable dtGroupImportTableTitle6 = new DataTable("dtGroupImportTableTitle6");
            DataTable dtGroupImportTableTitle7 = new DataTable("dtGroupImportTableTitle7");
            DataTable dtGroupImportTableTitle8 = new DataTable("dtGroupImportTableTitle8");
            DataTable dtGroupImportTableTitle9 = new DataTable("dtGroupImportTableTitle9");
            DataTable dtGroupImportTableTitle10 = new DataTable("dtGroupImportTableTitle10");
            DataTable dtGroupImportTableTitle11 = new DataTable("dtGroupImportTableTitle11");
            DataTable dtGroupImportTableTitle12 = new DataTable("dtGroupImportTableTitle12");
            DataTable dtGroupImportTableTitle13 = new DataTable("dtGroupImportTableTitle13");
            DataTable dtGroupImportTableTitle14 = new DataTable("dtGroupImportTableTitle14");
            DataTable dtGroupImportTableTitle15 = new DataTable("dtGroupImportTableTitle15");
            DataTable dtGroupImportTableTitle16 = new DataTable("dtGroupImportTableTitle16");
            DataTable dtGroupImportTableTitle17 = new DataTable("dtGroupImportTableTitle17");
            DataTable dtGroupImportTableTitle18 = new DataTable("dtGroupImportTableTitle18");
            DataTable dtGroupImportTableTitle19 = new DataTable("dtGroupImportTableTitle19");
            DataTable dtGroupImportTableTitle20 = new DataTable("dtGroupImportTableTitle20");
            DataTable dtCustomernexttest = new DataTable("dtCustomernexttest");//下次体检推荐项目 

            if (ds.Tables.Count > 0)
            {
                dtTitle = ds.Tables["dtTitle"];
                dtGroupTitle = ds.Tables["dtGroupTitle"];
                dtGroupBasicMessage = ds.Tables["dtGroupBasicMessage"];
                dtGroupAgeSexColumnResult = ds.Tables["dtGroupAgeSexColumnResult"];
                dtGroupAgeSexCakeResult = ds.Tables["dtGroupAgeSexCakeResult"];
                dtGroupAgeSexCakeLeftResult = ds.Tables["dtGroupAgeSexCakeLeftResult"];
                dtGroupAllDiseasePercent = ds.Tables["dtGroupAllDiseasePercent"];
                dtGroupMaleDiseasePercent = ds.Tables["dtGroupMaleDiseasePercent"];
                dtGroupFamaleDiseasePercent = ds.Tables["dtGroupFamaleDiseasePercent"];
                dtGroupChronicDiseasesCompare = ds.Tables["dtGroupChronicDiseasesCompare"];
                dtGroupHealthCompare = ds.Tables["dtGroupHealthCompare"];
                dtGroupHealthSuggest = ds.Tables["dtGroupHealthSuggest"];
                dtGroupHealthAnormal = ds.Tables["dtGroupHealthAnormal"];
                dtGroupAllResult = ds.Tables["dtGroupAllResult"];
                dtGroupAnExamination = ds.Tables["dtGroupAnExamination"];
                dtGroupAnFinish = ds.Tables["dtGroupAnFinish"];
                dtGroupImportResult1 = ds.Tables["dtGroupImportResult1"];
                dtGroupImportResult2 = ds.Tables["dtGroupImportResult2"];
                dtGroupImportResult3 = ds.Tables["dtGroupImportResult3"];
                dtGroupImportResult4 = ds.Tables["dtGroupImportResult4"];
                dtGroupImportResult5 = ds.Tables["dtGroupImportResult5"];
                dtGroupImportResult6 = ds.Tables["dtGroupImportResult6"];
                dtGroupImportResult7 = ds.Tables["dtGroupImportResult7"];
                dtGroupImportResult8 = ds.Tables["dtGroupImportResult8"];
                dtGroupImportResult9 = ds.Tables["dtGroupImportResult9"];
                dtGroupImportResult10 = ds.Tables["dtGroupImportResult10"];
                dtGroupImportResult11 = ds.Tables["dtGroupImportResult11"];
                dtGroupImportResult12 = ds.Tables["dtGroupImportResult12"];
                dtGroupImportResult13 = ds.Tables["dtGroupImportResult13"];
                dtGroupImportResult14 = ds.Tables["dtGroupImportResult14"];
                dtGroupImportResult15 = ds.Tables["dtGroupImportResult15"];
                dtGroupImportResult16 = ds.Tables["dtGroupImportResult16"];
                dtGroupImportResult17 = ds.Tables["dtGroupImportResult17"];
                dtGroupImportResult18 = ds.Tables["dtGroupImportResult18"];
                dtGroupImportResult19 = ds.Tables["dtGroupImportResult19"];
                dtGroupImportResult20 = ds.Tables["dtGroupImportResult20"];
                dtGroupImportTable1 = ds.Tables["dtGroupImportTable1"];
                dtGroupImportTable2 = ds.Tables["dtGroupImportTable2"];
                dtGroupImportTable3 = ds.Tables["dtGroupImportTable3"];
                dtGroupImportTable4 = ds.Tables["dtGroupImportTable4"];
                dtGroupImportTable5 = ds.Tables["dtGroupImportTable5"];
                dtGroupImportTable6 = ds.Tables["dtGroupImportTable6"];
                dtGroupImportTable7 = ds.Tables["dtGroupImportTable7"];
                dtGroupImportTable8 = ds.Tables["dtGroupImportTable8"];
                dtGroupImportTable9 = ds.Tables["dtGroupImportTable9"];
                dtGroupImportTable10 = ds.Tables["dtGroupImportTable10"];
                dtGroupImportTable11 = ds.Tables["dtGroupImportTable11"];
                dtGroupImportTable12 = ds.Tables["dtGroupImportTable12"];
                dtGroupImportTable13 = ds.Tables["dtGroupImportTable13"];
                dtGroupImportTable14 = ds.Tables["dtGroupImportTable14"];
                dtGroupImportTable15 = ds.Tables["dtGroupImportTable15"];
                dtGroupImportTable16 = ds.Tables["dtGroupImportTable16"];
                dtGroupImportTable17 = ds.Tables["dtGroupImportTable17"];
                dtGroupImportTable18 = ds.Tables["dtGroupImportTable18"];
                dtGroupImportTable19 = ds.Tables["dtGroupImportTable19"];
                dtGroupImportTable20 = ds.Tables["dtGroupImportTable20"];
                dtGroupImportTableTitle1 = ds.Tables["dtGroupImportTableTitle1"];
                dtGroupImportTableTitle2 = ds.Tables["dtGroupImportTableTitle2"];
                dtGroupImportTableTitle3 = ds.Tables["dtGroupImportTableTitle3"];
                dtGroupImportTableTitle4 = ds.Tables["dtGroupImportTableTitle4"];
                dtGroupImportTableTitle5 = ds.Tables["dtGroupImportTableTitle5"];
                dtGroupImportTableTitle6 = ds.Tables["dtGroupImportTableTitle6"];
                dtGroupImportTableTitle7 = ds.Tables["dtGroupImportTableTitle7"];
                dtGroupImportTableTitle8 = ds.Tables["dtGroupImportTableTitle8"];
                dtGroupImportTableTitle9 = ds.Tables["dtGroupImportTableTitle9"];
                dtGroupImportTableTitle10 = ds.Tables["dtGroupImportTableTitle10"];
                dtGroupImportTableTitle11 = ds.Tables["dtGroupImportTableTitle11"];
                dtGroupImportTableTitle12 = ds.Tables["dtGroupImportTableTitle12"];
                dtGroupImportTableTitle13 = ds.Tables["dtGroupImportTableTitle13"];
                dtGroupImportTableTitle14 = ds.Tables["dtGroupImportTableTitle14"];
                dtGroupImportTableTitle15 = ds.Tables["dtGroupImportTableTitle15"];
                dtGroupImportTableTitle16 = ds.Tables["dtGroupImportTableTitle16"];
                dtGroupImportTableTitle17 = ds.Tables["dtGroupImportTableTitle17"];
                dtGroupImportTableTitle18 = ds.Tables["dtGroupImportTableTitle18"];
                dtGroupImportTableTitle19 = ds.Tables["dtGroupImportTableTitle19"];
                dtGroupImportTableTitle20 = ds.Tables["dtGroupImportTableTitle20"];
                dtCustomernexttest = ds.Tables["dtCustomernexttest"];
            }
            if (dtTitle.Columns.Count == 0)
            {
                dtTitle.Columns.Add("titleName", typeof(string));//报告标题
                DataRow dr = dtTitle.NewRow();
                dr["titleName"] = "团检报告";
                dtTitle.Rows.Add(dr);
            }
            if (dtGroupTitle.Columns.Count == 0)
            {

                dtGroupTitle.Columns.Add("Customercode", typeof(string));//单位编号
                dtGroupTitle.Columns.Add("Customername", typeof(string));//单位名称
                dtGroupTitle.Columns.Add("Enterdate", typeof(string));//体检日期
                dtGroupTitle.Columns.Add("Commentdate", typeof(string));//报告日期
            }
            if (dtGroupBasicMessage.Columns.Count == 0)
            {
                dtGroupBasicMessage.Columns.Add("StartDate", typeof(string));//开始时间
                dtGroupBasicMessage.Columns.Add("EndDate", typeof(string));//截止日期
                dtGroupBasicMessage.Columns.Add("PlanPeople", typeof(string));//计划参加人数
                dtGroupBasicMessage.Columns.Add("ActualPeople", typeof(string));//实际参加人数
                dtGroupBasicMessage.Columns.Add("APRatio", typeof(string));//实际参加率
                dtGroupBasicMessage.Columns.Add("FJoin", typeof(string));//男性参加人数
                dtGroupBasicMessage.Columns.Add("MJoin", typeof(string));//女性参加人数
                dtGroupBasicMessage.Columns.Add("FJoinRatio", typeof(string));//男性参加比例
                dtGroupBasicMessage.Columns.Add("MJoinRatio", typeof(string));//女性参加比例
                dtGroupBasicMessage.Columns.Add("MainTestItem", typeof(string));//主要项目
            }
            if (dtGroupAgeSexColumnResult.Columns.Count == 0)
            {
                dtGroupAgeSexColumnResult.Columns.Add("ageregion", typeof(string));//年龄
                dtGroupAgeSexColumnResult.Columns.Add("sex", typeof(string));//性别
                dtGroupAgeSexColumnResult.Columns.Add("numcount", typeof(string));//人数
            }
            if (dtGroupAgeSexCakeResult.Columns.Count == 0)
            {

                dtGroupAgeSexCakeResult.Columns.Add("AverageAgeType", typeof(string));//平均年龄类型
                dtGroupAgeSexCakeResult.Columns.Add("number", typeof(string));//人数
            }
            if (dtGroupAgeSexCakeLeftResult.Columns.Count == 0)
            {
                dtGroupAgeSexCakeLeftResult.Columns.Add("AverageAge", typeof(string));//年龄
                dtGroupAgeSexCakeLeftResult.Columns.Add("AverageAgeUpNumber", typeof(string));//性别
                dtGroupAgeSexCakeLeftResult.Columns.Add("AverageAgeDownNumber", typeof(string));//人数                
            }
            if (dtGroupAllDiseasePercent.Columns.Count == 0)
            {
                dtGroupAllDiseasePercent.Columns.Add("DiseaseName", typeof(string));//年龄
                dtGroupAllDiseasePercent.Columns.Add("DiseasePercent", typeof(string));//性别
            }
            if (dtGroupMaleDiseasePercent.Columns.Count == 0)
            {
                dtGroupMaleDiseasePercent.Columns.Add("DiseaseName", typeof(string));//年龄
                dtGroupMaleDiseasePercent.Columns.Add("DiseasePercent", typeof(string));//性别

            }
            if (dtGroupFamaleDiseasePercent.Columns.Count == 0)
            {
                dtGroupFamaleDiseasePercent.Columns.Add("DiseaseName", typeof(string));//年龄
                dtGroupFamaleDiseasePercent.Columns.Add("DiseasePercent", typeof(string));//性别

            }
            if (dtGroupChronicDiseasesCompare.Columns.Count == 0)
            {
                dtGroupChronicDiseasesCompare.Columns.Add("MedicalConclusion", typeof(string));//体检结论
                dtGroupChronicDiseasesCompare.Columns.Add("LastResult", typeof(string));//上次
                dtGroupChronicDiseasesCompare.Columns.Add("ThisResult", typeof(string));//本次
                dtGroupChronicDiseasesCompare.Columns.Add("Tendency", typeof(string));//趋势                
            }
            if (dtGroupHealthCompare.Columns.Count == 0)
            {
                dtGroupHealthCompare.Columns.Add("MedicalConclusion", typeof(string));//体检结论
                dtGroupHealthCompare.Columns.Add("LastResult", typeof(string));//上次
                dtGroupHealthCompare.Columns.Add("ThisResult", typeof(string));//本次
                dtGroupHealthCompare.Columns.Add("Tendency", typeof(string));//趋势                
            }
            if (dtGroupHealthSuggest.Columns.Count == 0)
            {
                dtGroupHealthSuggest.Columns.Add("text", typeof(string));//内容                
            }
            if (dtGroupHealthAnormal.Columns.Count == 0)
            {
                dtGroupHealthAnormal.Columns.Add("MedicalConclusion", typeof(string));//体检结论
                dtGroupHealthAnormal.Columns.Add("MaleNumber", typeof(string));//男性人数
                dtGroupHealthAnormal.Columns.Add("FamaleNumber", typeof(string));//女性人数
                dtGroupHealthAnormal.Columns.Add("SumNumber", typeof(string));//总共人数
                dtGroupHealthAnormal.Columns.Add("MalePercent", typeof(string));//男性比例
                dtGroupHealthAnormal.Columns.Add("FamalePercent", typeof(string));//女性比例
                dtGroupHealthAnormal.Columns.Add("SumPercent", typeof(string));//合计比例
            }
            if (dtGroupAllResult.Columns.Count == 0)
            {
                dtGroupAllResult.Columns.Add("number", typeof(string));//编号
                dtGroupAllResult.Columns.Add("name", typeof(string));
                dtGroupAllResult.Columns.Add("sex", typeof(string));
                dtGroupAllResult.Columns.Add("age", typeof(string));
                dtGroupAllResult.Columns.Add("AbnormalText", typeof(string));//异常
            }
            if (dtGroupAnExamination.Columns.Count == 0)
            {
                dtGroupAnExamination.Columns.Add("number", typeof(string));//编号
                dtGroupAnExamination.Columns.Add("name", typeof(string));
                dtGroupAnExamination.Columns.Add("sex", typeof(string));
                dtGroupAnExamination.Columns.Add("age", typeof(string));
            }
            if (dtGroupAnFinish.Columns.Count == 0)
            {
                dtGroupAnFinish.Columns.Add("ordernum", typeof(string));//编号
                dtGroupAnFinish.Columns.Add("name", typeof(string));
                dtGroupAnFinish.Columns.Add("sex", typeof(string));
                dtGroupAnFinish.Columns.Add("age", typeof(string));
                dtGroupAnFinish.Columns.Add("unFinishTest", typeof(string));//异常
            }
            if (dtGroupImportResult1.Columns.Count == 0)
            {
                dtGroupImportResult1.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult1.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult1.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult1.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult2.Columns.Count == 0)
            {
                dtGroupImportResult2.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult2.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult2.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult2.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult3.Columns.Count == 0)
            {
                dtGroupImportResult3.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult3.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult3.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult3.Columns.Add("testName", typeof(string));//名称                
            }
            if (dtGroupImportResult4.Columns.Count == 0)
            {
                dtGroupImportResult4.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult4.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult4.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult4.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult5.Columns.Count == 0)
            {
                dtGroupImportResult5.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult5.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult5.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult5.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult6.Columns.Count == 0)
            {
                dtGroupImportResult6.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult6.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult6.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult6.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult7.Columns.Count == 0)
            {
                dtGroupImportResult7.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult7.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult7.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult7.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult8.Columns.Count == 0)
            {
                dtGroupImportResult8.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult8.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult8.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult8.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult9.Columns.Count == 0)
            {
                dtGroupImportResult9.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult9.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult9.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult9.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult10.Columns.Count == 0)
            {
                dtGroupImportResult10.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult10.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult10.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult10.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult11.Columns.Count == 0)
            {
                dtGroupImportResult11.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult11.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult11.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult11.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult12.Columns.Count == 0)
            {
                dtGroupImportResult12.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult12.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult12.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult12.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult13.Columns.Count == 0)
            {
                dtGroupImportResult13.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult13.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult13.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult13.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult14.Columns.Count == 0)
            {
                dtGroupImportResult14.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult14.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult14.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult14.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult15.Columns.Count == 0)
            {
                dtGroupImportResult15.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult15.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult15.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult15.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult16.Columns.Count == 0)
            {
                dtGroupImportResult16.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult16.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult16.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult16.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult17.Columns.Count == 0)
            {
                dtGroupImportResult17.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult17.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult17.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult17.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult18.Columns.Count == 0)
            {
                dtGroupImportResult18.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult18.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult18.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult18.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult19.Columns.Count == 0)
            {
                dtGroupImportResult19.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult19.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult19.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult19.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportResult20.Columns.Count == 0)
            {
                dtGroupImportResult20.Columns.Add("resultType", typeof(string));//结果种类
                dtGroupImportResult20.Columns.Add("numCount", typeof(string));//人数
                dtGroupImportResult20.Columns.Add("testType", typeof(string));//检测种类
                dtGroupImportResult20.Columns.Add("testName", typeof(string));//名称
            }
            if (dtGroupImportTable1.Columns.Count == 0)
            {
                dtGroupImportTable1.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable1.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable1.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable1.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable1.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable1.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable1.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable1.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable1.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable2.Columns.Count == 0)
            {
                dtGroupImportTable2.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable2.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable2.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable2.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable2.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable2.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable2.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable2.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable2.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable3.Columns.Count == 0)
            {
                dtGroupImportTable3.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable3.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable3.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable3.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable3.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable3.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable3.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable3.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable3.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable4.Columns.Count == 0)
            {
                dtGroupImportTable4.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable4.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable4.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable4.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable4.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable4.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable4.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable4.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable4.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable5.Columns.Count == 0)
            {
                dtGroupImportTable5.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable5.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable5.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable5.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable5.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable5.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable5.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable5.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable5.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable6.Columns.Count == 0)
            {
                dtGroupImportTable6.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable6.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable6.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable6.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable6.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable6.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable6.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable6.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable6.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable7.Columns.Count == 0)
            {
                dtGroupImportTable7.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable7.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable7.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable7.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable7.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable7.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable7.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable7.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable7.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable8.Columns.Count == 0)
            {
                dtGroupImportTable8.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable8.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable8.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable8.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable8.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable8.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable8.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable8.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable8.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable9.Columns.Count == 0)
            {
                dtGroupImportTable9.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable9.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable9.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable9.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable9.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable9.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable9.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable9.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable9.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable10.Columns.Count == 0)
            {
                dtGroupImportTable10.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable10.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable10.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable10.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable10.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable10.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable10.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable10.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable10.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable11.Columns.Count == 0)
            {
                dtGroupImportTable11.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable11.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable11.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable11.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable11.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable11.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable11.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable11.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable11.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable12.Columns.Count == 0)
            {
                dtGroupImportTable12.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable12.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable12.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable12.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable12.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable12.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable12.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable12.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable12.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable13.Columns.Count == 0)
            {
                dtGroupImportTable13.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable13.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable13.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable13.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable13.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable13.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable13.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable13.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable13.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable14.Columns.Count == 0)
            {
                dtGroupImportTable14.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable14.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable14.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable14.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable14.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable14.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable14.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable14.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable14.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable15.Columns.Count == 0)
            {
                dtGroupImportTable15.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable15.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable15.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable15.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable15.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable15.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable15.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable15.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable15.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable16.Columns.Count == 0)
            {
                dtGroupImportTable16.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable16.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable16.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable16.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable16.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable16.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable16.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable16.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable16.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable17.Columns.Count == 0)
            {
                dtGroupImportTable17.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable17.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable17.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable17.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable17.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable17.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable17.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable17.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable17.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable18.Columns.Count == 0)
            {
                dtGroupImportTable18.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable18.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable18.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable18.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable18.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable18.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable18.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable18.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable18.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable19.Columns.Count == 0)
            {
                dtGroupImportTable19.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable19.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable19.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable19.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable19.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable19.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable19.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable19.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable19.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }
            if (dtGroupImportTable20.Columns.Count == 0)
            {
                dtGroupImportTable20.Columns.Add("remark", typeof(string));//说明
                dtGroupImportTable20.Columns.Add("numCount1", typeof(string));//类型1的人数
                dtGroupImportTable20.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
                dtGroupImportTable20.Columns.Add("numCount2", typeof(string));//类型2的人数
                dtGroupImportTable20.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
                dtGroupImportTable20.Columns.Add("numCount3", typeof(string));//类型3的人数
                dtGroupImportTable20.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
                dtGroupImportTable20.Columns.Add("numCount4", typeof(string));//类型4的人数
                dtGroupImportTable20.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            }


            if (dtGroupImportTableTitle1.Columns.Count == 0)
            {
                dtGroupImportTableTitle1.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle1.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle1.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle1.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle1.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle1.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle1.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle1.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle1.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle2.Columns.Count == 0)
            {
                dtGroupImportTableTitle2.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle2.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle2.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle2.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle2.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle2.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle2.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle2.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle2.Columns.Add("numPercent4", typeof(string));

            }
            if (dtGroupImportTableTitle3.Columns.Count == 0)
            {
                dtGroupImportTableTitle3.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle3.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle3.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle3.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle3.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle3.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle3.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle3.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle3.Columns.Add("numPercent4", typeof(string));
                DataRow drq = dtGroupImportTableTitle3.NewRow();
                drq["remark"] = "总人次";
                drq["numCount1"] = "偏低";
                drq["numPercent1"] = "";
                drq["numCount2"] = "正常";
                drq["numPercent2"] = "";
                drq["numCount3"] = "正常高";
                drq["numPercent3"] = "";
                drq["numCount4"] = "升高";
                drq["numPercent4"] = "";
                dtGroupImportTableTitle3.Rows.Add(drq);
            }
            if (dtGroupImportTableTitle4.Columns.Count == 0)
            {
                dtGroupImportTableTitle4.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle4.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle4.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle4.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle4.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle4.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle4.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle4.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle4.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle5.Columns.Count == 0)
            {
                dtGroupImportTableTitle5.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle5.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle5.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle5.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle5.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle5.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle5.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle5.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle5.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle6.Columns.Count == 0)
            {
                dtGroupImportTableTitle6.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle6.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle6.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle6.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle6.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle6.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle6.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle6.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle6.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle7.Columns.Count == 0)
            {
                dtGroupImportTableTitle7.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle7.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle7.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle7.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle7.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle7.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle7.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle7.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle7.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle8.Columns.Count == 0)
            {
                dtGroupImportTableTitle8.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle8.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle8.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle8.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle8.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle8.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle8.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle8.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle8.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle9.Columns.Count == 0)
            {
                dtGroupImportTableTitle9.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle9.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle9.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle9.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle9.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle9.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle9.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle9.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle9.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle10.Columns.Count == 0)
            {
                dtGroupImportTableTitle10.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle10.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle10.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle10.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle10.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle10.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle10.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle10.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle10.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle11.Columns.Count == 0)
            {
                dtGroupImportTableTitle11.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle11.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle11.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle11.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle11.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle11.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle11.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle11.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle11.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle12.Columns.Count == 0)
            {
                dtGroupImportTableTitle12.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle12.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle12.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle12.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle12.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle12.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle12.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle12.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle12.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle13.Columns.Count == 0)
            {
                dtGroupImportTableTitle13.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle13.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle13.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle13.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle13.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle13.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle13.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle13.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle13.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle14.Columns.Count == 0)
            {
                dtGroupImportTableTitle14.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle14.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle14.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle14.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle14.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle14.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle14.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle14.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle14.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle15.Columns.Count == 0)
            {
                dtGroupImportTableTitle15.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle15.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle15.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle15.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle15.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle15.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle15.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle15.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle15.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle16.Columns.Count == 0)
            {
                dtGroupImportTableTitle16.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle16.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle16.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle16.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle16.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle16.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle16.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle16.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle16.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle17.Columns.Count == 0)
            {
                dtGroupImportTableTitle17.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle17.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle17.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle17.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle17.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle17.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle17.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle17.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle17.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle18.Columns.Count == 0)
            {
                dtGroupImportTableTitle18.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle18.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle18.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle18.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle18.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle18.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle18.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle18.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle18.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle19.Columns.Count == 0)
            {
                dtGroupImportTableTitle19.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle19.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle19.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle19.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle19.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle19.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle19.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle19.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle19.Columns.Add("numPercent4", typeof(string));
            }
            if (dtGroupImportTableTitle20.Columns.Count == 0)
            {
                dtGroupImportTableTitle20.Columns.Add("remark", typeof(string));
                dtGroupImportTableTitle20.Columns.Add("numCount1", typeof(string));
                dtGroupImportTableTitle20.Columns.Add("numPercent1", typeof(string));
                dtGroupImportTableTitle20.Columns.Add("numCount2", typeof(string));
                dtGroupImportTableTitle20.Columns.Add("numPercent2", typeof(string));
                dtGroupImportTableTitle20.Columns.Add("numCount3", typeof(string));
                dtGroupImportTableTitle20.Columns.Add("numPercent3", typeof(string));
                dtGroupImportTableTitle20.Columns.Add("numCount4", typeof(string));
                dtGroupImportTableTitle20.Columns.Add("numPercent4", typeof(string));
            }
            if (dtCustomernexttest.Columns.Count == 0)
            {
                dtCustomernexttest.Columns.Add("testname", typeof(string));//项目
                dtCustomernexttest.Columns.Add("rerundate", typeof(string));//时间                
            }

            try
            {
                fReport = new Report();
                fReport.Load(string.Format("{0}{1}.frx", strParth, reporttemplateCode));
                dsReportData = new DataSet();

                DataTable dtTitlenew = dtTitle.Copy();
                dtTitlenew.TableName = "dtTitle";
                DataTable dtGroupTitlenew = dtGroupTitle.Copy();
                dtGroupTitlenew.TableName = "dtGroupTitle";
                DataTable dtGroupBasicMessagenew = dtGroupBasicMessage.Copy();
                dtGroupBasicMessagenew.TableName = "dtGroupBasicMessage";
                DataTable dtGroupAgeSexColumnResultnew = dtGroupAgeSexColumnResult.Copy();
                dtGroupAgeSexColumnResultnew.TableName = "dtGroupAgeSexColumnResult";
                DataTable dtGroupAgeSexCakeResultnew = dtGroupAgeSexCakeResult.Copy();
                dtGroupAgeSexCakeResultnew.TableName = "dtGroupAgeSexCakeResult";
                DataTable dtGroupAgeSexCakeLeftResultnew = dtGroupAgeSexCakeLeftResult.Copy();
                dtGroupAgeSexCakeLeftResultnew.TableName = "dtGroupAgeSexCakeLeftResult";
                DataTable dtGroupAllDiseasePercentnew = dtGroupAllDiseasePercent.Copy();
                dtGroupAllDiseasePercentnew.TableName = "dtGroupAllDiseasePercent";
                DataTable dtGroupMaleDiseasePercentnew = dtGroupMaleDiseasePercent.Copy();
                dtGroupMaleDiseasePercentnew.TableName = "dtGroupMaleDiseasePercent";
                DataTable dtGroupFamaleDiseasePercentnew = dtGroupFamaleDiseasePercent.Copy();
                dtGroupFamaleDiseasePercentnew.TableName = "dtGroupFamaleDiseasePercent";
                DataTable dtGroupChronicDiseasesComparenew = dtGroupChronicDiseasesCompare.Copy();
                dtGroupChronicDiseasesComparenew.TableName = "dtGroupChronicDiseasesCompare";
                DataTable dtGroupHealthComparenew = dtGroupHealthCompare.Copy();
                dtGroupHealthComparenew.TableName = "dtGroupHealthCompare";
                DataTable dtGroupHealthSuggestnew = dtGroupHealthSuggest.Copy();
                dtGroupHealthSuggestnew.TableName = "dtGroupHealthSuggest";
                DataTable dtGroupHealthAnormalnew = dtGroupHealthAnormal.Copy();
                dtGroupHealthAnormalnew.TableName = "dtGroupHealthAnormal";
                DataTable dtGroupAllResultnew = dtGroupAllResult.Copy();
                dtGroupAllResultnew.TableName = "dtGroupAllResult";
                DataTable dtGroupAnExaminationnew = dtGroupAnExamination.Copy();
                dtGroupAnExaminationnew.TableName = "dtGroupAnExamination";
                DataTable dtGroupAnFinishnew = dtGroupAnFinish.Copy();
                dtGroupAnFinishnew.TableName = "dtGroupAnFinish";
                DataTable dtGroupImportResult1new = dtGroupImportResult1.Copy();
                dtGroupImportResult1new.TableName = "dtGroupImportResult1";
                DataTable dtGroupImportResult2new = dtGroupImportResult2.Copy();
                dtGroupImportResult2new.TableName = "dtGroupImportResult2";
                DataTable dtGroupImportResult3new = dtGroupImportResult3.Copy();
                dtGroupImportResult3new.TableName = "dtGroupImportResult3";
                DataTable dtGroupImportResult4new = dtGroupImportResult4.Copy();
                dtGroupImportResult4new.TableName = "dtGroupImportResult4";
                DataTable dtGroupImportResult5new = dtGroupImportResult5.Copy();
                dtGroupImportResult5new.TableName = "dtGroupImportResult5";
                DataTable dtGroupImportResult6new = dtGroupImportResult6.Copy();
                dtGroupImportResult6new.TableName = "dtGroupImportResult6";
                DataTable dtGroupImportResult7new = dtGroupImportResult7.Copy();
                dtGroupImportResult7new.TableName = "dtGroupImportResult7";
                DataTable dtGroupImportResult8new = dtGroupImportResult8.Copy();
                dtGroupImportResult8new.TableName = "dtGroupImportResult8";
                DataTable dtGroupImportResult9new = dtGroupImportResult9.Copy();
                dtGroupImportResult9new.TableName = "dtGroupImportResult9";
                DataTable dtGroupImportResult10new = dtGroupImportResult10.Copy();
                dtGroupImportResult10new.TableName = "dtGroupImportResult10";
                DataTable dtGroupImportResult11new = dtGroupImportResult11.Copy();
                dtGroupImportResult11new.TableName = "dtGroupImportResult11";
                DataTable dtGroupImportResult12new = dtGroupImportResult12.Copy();
                dtGroupImportResult12new.TableName = "dtGroupImportResult12";
                DataTable dtGroupImportResult13new = dtGroupImportResult13.Copy();
                dtGroupImportResult13new.TableName = "dtGroupImportResult13";
                DataTable dtGroupImportResult14new = dtGroupImportResult14.Copy();
                dtGroupImportResult14new.TableName = "dtGroupImportResult14";
                DataTable dtGroupImportResult15new = dtGroupImportResult15.Copy();
                dtGroupImportResult15new.TableName = "dtGroupImportResult15";
                DataTable dtGroupImportResult16new = dtGroupImportResult16.Copy();
                dtGroupImportResult16new.TableName = "dtGroupImportResult16";
                DataTable dtGroupImportResult17new = dtGroupImportResult17.Copy();
                dtGroupImportResult17new.TableName = "dtGroupImportResult17";
                DataTable dtGroupImportResult18new = dtGroupImportResult18.Copy();
                dtGroupImportResult18new.TableName = "dtGroupImportResult18";
                DataTable dtGroupImportResult19new = dtGroupImportResult19.Copy();
                dtGroupImportResult19new.TableName = "dtGroupImportResult19";
                DataTable dtGroupImportResult20new = dtGroupImportResult20.Copy();
                dtGroupImportResult20new.TableName = "dtGroupImportResult20";
                DataTable dtGroupImportTable1new = dtGroupImportTable1.Copy();
                dtGroupImportTable1new.TableName = "dtGroupImportTable1";
                DataTable dtGroupImportTable2new = dtGroupImportTable2.Copy();
                dtGroupImportTable2new.TableName = "dtGroupImportTable2";
                DataTable dtGroupImportTable3new = dtGroupImportTable3.Copy();
                dtGroupImportTable3new.TableName = "dtGroupImportTable3";
                DataTable dtGroupImportTable4new = dtGroupImportTable4.Copy();
                dtGroupImportTable4new.TableName = "dtGroupImportTable4";
                DataTable dtGroupImportTable5new = dtGroupImportTable5.Copy();
                dtGroupImportTable5new.TableName = "dtGroupImportTable5";
                DataTable dtGroupImportTable6new = dtGroupImportTable6.Copy();
                dtGroupImportTable6new.TableName = "dtGroupImportTable6";
                DataTable dtGroupImportTable7new = dtGroupImportTable7.Copy();
                dtGroupImportTable7new.TableName = "dtGroupImportTable7";
                DataTable dtGroupImportTable8new = dtGroupImportTable8.Copy();
                dtGroupImportTable8new.TableName = "dtGroupImportTable8";
                DataTable dtGroupImportTable9new = dtGroupImportTable9.Copy();
                dtGroupImportTable9new.TableName = "dtGroupImportTable9";
                DataTable dtGroupImportTable10new = dtGroupImportTable10.Copy();
                dtGroupImportTable10new.TableName = "dtGroupImportTable10";
                DataTable dtGroupImportTable11new = dtGroupImportTable11.Copy();
                dtGroupImportTable11new.TableName = "dtGroupImportTable11";
                DataTable dtGroupImportTable12new = dtGroupImportTable12.Copy();
                dtGroupImportTable12new.TableName = "dtGroupImportTable12";
                DataTable dtGroupImportTable13new = dtGroupImportTable13.Copy();
                dtGroupImportTable13new.TableName = "dtGroupImportTable13";
                DataTable dtGroupImportTable14new = dtGroupImportTable14.Copy();
                dtGroupImportTable14new.TableName = "dtGroupImportTable14";
                DataTable dtGroupImportTable15new = dtGroupImportTable15.Copy();
                dtGroupImportTable15new.TableName = "dtGroupImportTable15";
                DataTable dtGroupImportTable16new = dtGroupImportTable16.Copy();
                dtGroupImportTable16new.TableName = "dtGroupImportTable16";
                DataTable dtGroupImportTable17new = dtGroupImportTable17.Copy();
                dtGroupImportTable17new.TableName = "dtGroupImportTable17";
                DataTable dtGroupImportTable18new = dtGroupImportTable18.Copy();
                dtGroupImportTable18new.TableName = "dtGroupImportTable18";
                DataTable dtGroupImportTable19new = dtGroupImportTable19.Copy();
                dtGroupImportTable19new.TableName = "dtGroupImportTable19";
                DataTable dtGroupImportTable20new = dtGroupImportTable20.Copy();
                dtGroupImportTable20new.TableName = "dtGroupImportTable20";
                DataTable dtGroupImportTableTitle1new = dtGroupImportTableTitle1.Copy();
                dtGroupImportTableTitle1new.TableName = "dtGroupImportTableTitle1";
                DataTable dtGroupImportTableTitle2new = dtGroupImportTableTitle2.Copy();
                dtGroupImportTableTitle2new.TableName = "dtGroupImportTableTitle2";
                DataTable dtGroupImportTableTitle3new = dtGroupImportTableTitle3.Copy();
                dtGroupImportTableTitle3new.TableName = "dtGroupImportTableTitle3";
                DataTable dtGroupImportTableTitle4new = dtGroupImportTableTitle4.Copy();
                dtGroupImportTableTitle4new.TableName = "dtGroupImportTableTitle4";
                DataTable dtGroupImportTableTitle5new = dtGroupImportTableTitle5.Copy();
                dtGroupImportTableTitle5new.TableName = "dtGroupImportTableTitle5";
                DataTable dtGroupImportTableTitle6new = dtGroupImportTableTitle6.Copy();
                dtGroupImportTableTitle6new.TableName = "dtGroupImportTableTitle6";
                DataTable dtGroupImportTableTitle7new = dtGroupImportTableTitle7.Copy();
                dtGroupImportTableTitle7new.TableName = "dtGroupImportTableTitle7";
                DataTable dtGroupImportTableTitle8new = dtGroupImportTableTitle8.Copy();
                dtGroupImportTableTitle8new.TableName = "dtGroupImportTableTitle8";
                DataTable dtGroupImportTableTitle9new = dtGroupImportTableTitle9.Copy();
                dtGroupImportTableTitle9new.TableName = "dtGroupImportTableTitle9";
                DataTable dtGroupImportTableTitle10new = dtGroupImportTableTitle10.Copy();
                dtGroupImportTableTitle10new.TableName = "dtGroupImportTableTitle10";
                DataTable dtGroupImportTableTitle11new = dtGroupImportTableTitle11.Copy();
                dtGroupImportTableTitle11new.TableName = "dtGroupImportTableTitle11";
                DataTable dtGroupImportTableTitle12new = dtGroupImportTableTitle12.Copy();
                dtGroupImportTableTitle12new.TableName = "dtGroupImportTableTitle12";
                DataTable dtGroupImportTableTitle13new = dtGroupImportTableTitle13.Copy();
                dtGroupImportTableTitle13new.TableName = "dtGroupImportTableTitle13";
                DataTable dtGroupImportTableTitle14new = dtGroupImportTableTitle14.Copy();
                dtGroupImportTableTitle14new.TableName = "dtGroupImportTableTitle14";
                DataTable dtGroupImportTableTitle15new = dtGroupImportTableTitle15.Copy();
                dtGroupImportTableTitle15new.TableName = "dtGroupImportTableTitle15";
                DataTable dtGroupImportTableTitle16new = dtGroupImportTableTitle16.Copy();
                dtGroupImportTableTitle16new.TableName = "dtGroupImportTableTitle16";
                DataTable dtGroupImportTableTitle17new = dtGroupImportTableTitle17.Copy();
                dtGroupImportTableTitle17new.TableName = "dtGroupImportTableTitle17";
                DataTable dtGroupImportTableTitle18new = dtGroupImportTableTitle18.Copy();
                dtGroupImportTableTitle18new.TableName = "dtGroupImportTableTitle18";
                DataTable dtGroupImportTableTitle19new = dtGroupImportTableTitle19.Copy();
                dtGroupImportTableTitle19new.TableName = "dtGroupImportTableTitle19";
                DataTable dtGroupImportTableTitle20new = dtGroupImportTableTitle20.Copy();
                dtGroupImportTableTitle20new.TableName = "dtGroupImportTableTitle20";
                DataTable dtCustomernexttestnew = dtCustomernexttest.Copy();
                dtCustomernexttestnew.TableName = "dtCustomernexttest";

                dsReportData.Tables.Add(dtTitlenew);
                dsReportData.Tables.Add(dtGroupTitlenew);
                dsReportData.Tables.Add(dtGroupBasicMessagenew);
                dsReportData.Tables.Add(dtGroupAgeSexColumnResultnew);
                dsReportData.Tables.Add(dtGroupAgeSexCakeResultnew);
                dsReportData.Tables.Add(dtGroupAgeSexCakeLeftResultnew);
                dsReportData.Tables.Add(dtGroupAllDiseasePercentnew);
                dsReportData.Tables.Add(dtGroupMaleDiseasePercentnew);
                dsReportData.Tables.Add(dtGroupFamaleDiseasePercentnew);
                dsReportData.Tables.Add(dtGroupChronicDiseasesComparenew);
                dsReportData.Tables.Add(dtGroupHealthComparenew);
                dsReportData.Tables.Add(dtGroupHealthSuggestnew);
                dsReportData.Tables.Add(dtGroupHealthAnormalnew);
                dsReportData.Tables.Add(dtGroupAllResultnew);
                dsReportData.Tables.Add(dtGroupAnExaminationnew);
                dsReportData.Tables.Add(dtGroupAnFinishnew);
                dsReportData.Tables.Add(dtGroupImportResult1new);
                dsReportData.Tables.Add(dtGroupImportResult2new);
                dsReportData.Tables.Add(dtGroupImportResult3new);
                dsReportData.Tables.Add(dtGroupImportResult4new);
                dsReportData.Tables.Add(dtGroupImportResult5new);
                dsReportData.Tables.Add(dtGroupImportResult6new);
                dsReportData.Tables.Add(dtGroupImportResult7new);
                dsReportData.Tables.Add(dtGroupImportResult8new);
                dsReportData.Tables.Add(dtGroupImportResult9new);
                dsReportData.Tables.Add(dtGroupImportResult10new);
                dsReportData.Tables.Add(dtGroupImportResult11new);
                dsReportData.Tables.Add(dtGroupImportResult12new);
                dsReportData.Tables.Add(dtGroupImportResult13new);
                dsReportData.Tables.Add(dtGroupImportResult14new);
                dsReportData.Tables.Add(dtGroupImportResult15new);
                dsReportData.Tables.Add(dtGroupImportResult16new);
                dsReportData.Tables.Add(dtGroupImportResult17new);
                dsReportData.Tables.Add(dtGroupImportResult18new);
                dsReportData.Tables.Add(dtGroupImportResult19new);
                dsReportData.Tables.Add(dtGroupImportResult20new);
                dsReportData.Tables.Add(dtGroupImportTable1new);
                dsReportData.Tables.Add(dtGroupImportTable2new);
                dsReportData.Tables.Add(dtGroupImportTable3new);
                dsReportData.Tables.Add(dtGroupImportTable4new);
                dsReportData.Tables.Add(dtGroupImportTable5new);
                dsReportData.Tables.Add(dtGroupImportTable6new);
                dsReportData.Tables.Add(dtGroupImportTable7new);
                dsReportData.Tables.Add(dtGroupImportTable8new);
                dsReportData.Tables.Add(dtGroupImportTable9new);
                dsReportData.Tables.Add(dtGroupImportTable10new);
                dsReportData.Tables.Add(dtGroupImportTable11new);
                dsReportData.Tables.Add(dtGroupImportTable12new);
                dsReportData.Tables.Add(dtGroupImportTable13new);
                dsReportData.Tables.Add(dtGroupImportTable14new);
                dsReportData.Tables.Add(dtGroupImportTable15new);
                dsReportData.Tables.Add(dtGroupImportTable16new);
                dsReportData.Tables.Add(dtGroupImportTable17new);
                dsReportData.Tables.Add(dtGroupImportTable18new);
                dsReportData.Tables.Add(dtGroupImportTable19new);
                dsReportData.Tables.Add(dtGroupImportTable20new);
                dsReportData.Tables.Add(dtGroupImportTableTitle1new);
                dsReportData.Tables.Add(dtGroupImportTableTitle2new);
                dsReportData.Tables.Add(dtGroupImportTableTitle3new);
                dsReportData.Tables.Add(dtGroupImportTableTitle4new);
                dsReportData.Tables.Add(dtGroupImportTableTitle5new);
                dsReportData.Tables.Add(dtGroupImportTableTitle6new);
                dsReportData.Tables.Add(dtGroupImportTableTitle7new);
                dsReportData.Tables.Add(dtGroupImportTableTitle8new);
                dsReportData.Tables.Add(dtGroupImportTableTitle9new);
                dsReportData.Tables.Add(dtGroupImportTableTitle10new);
                dsReportData.Tables.Add(dtGroupImportTableTitle11new);
                dsReportData.Tables.Add(dtGroupImportTableTitle12new);
                dsReportData.Tables.Add(dtGroupImportTableTitle13new);
                dsReportData.Tables.Add(dtGroupImportTableTitle14new);
                dsReportData.Tables.Add(dtGroupImportTableTitle15new);
                dsReportData.Tables.Add(dtGroupImportTableTitle16new);
                dsReportData.Tables.Add(dtGroupImportTableTitle17new);
                dsReportData.Tables.Add(dtGroupImportTableTitle18new);
                dsReportData.Tables.Add(dtGroupImportTableTitle19new);
                dsReportData.Tables.Add(dtGroupImportTableTitle20new);
                dsReportData.Tables.Add(dtCustomernexttestnew);

                if (isprint != 1)
                {
                    fReport.RegisterData(dtTitle, "dtTitle");//注册报告头
                    fReport.RegisterData(dtGroupTitle, "dtGroupTitle");//注册报告头
                    fReport.RegisterData(dtGroupBasicMessage, "dtGroupBasicMessage");//注册体检基本信息
                    fReport.RegisterData(dtGroupAgeSexColumnResult, "dtGroupAgeSexColumnResult");//注册男女年龄分布柱状图
                    fReport.RegisterData(dtGroupAgeSexCakeResult, "dtGroupAgeSexCakeResult");//注册男女年龄分布柱状图
                    fReport.RegisterData(dtGroupAgeSexCakeLeftResult, "dtGroupAgeSexCakeLeftResult");//注册男女年龄分布柱状图左侧表格
                    fReport.RegisterData(dtGroupAllDiseasePercent, "dtGroupAllDiseasePercent");//注册所有员工前十种疾病比例
                    fReport.RegisterData(dtGroupMaleDiseasePercent, "dtGroupMaleDiseasePercent");//注册所有男性员工前十种疾病比例
                    fReport.RegisterData(dtGroupFamaleDiseasePercent, "dtGroupFamaleDiseasePercent");//注册所有女性员工前十种疾病比例
                    fReport.RegisterData(dtGroupChronicDiseasesCompare, "dtGroupChronicDiseasesCompare");//注册慢性病历年比较
                    fReport.RegisterData(dtGroupHealthCompare, "dtGroupHealthCompare");//注册健康问题历年比较
                    fReport.RegisterData(dtGroupHealthSuggest, "dtGroupHealthSuggest");//注册健康问题分析与建议
                    fReport.RegisterData(dtGroupHealthAnormal, "dtGroupHealthAnormal");//注册体检异常统计
                    fReport.RegisterData(dtGroupAllResult, "dtGroupAllResult");//注册所有员工体检结果汇总
                    fReport.RegisterData(dtGroupAnExamination, "dtGroupAnExamination");//注册已预约而未体检
                    fReport.RegisterData(dtGroupAnFinish, "dtGroupAnFinish");//注册已体检而未完成
                    fReport.RegisterData(dtGroupImportResult1, "dtGroupImportResult1");//注册重要指标检查结果分组一
                    fReport.RegisterData(dtGroupImportResult2, "dtGroupImportResult2");
                    fReport.RegisterData(dtGroupImportResult3, "dtGroupImportResult3");
                    fReport.RegisterData(dtGroupImportResult4, "dtGroupImportResult4");
                    fReport.RegisterData(dtGroupImportResult5, "dtGroupImportResult5");
                    fReport.RegisterData(dtGroupImportResult6, "dtGroupImportResult6");
                    fReport.RegisterData(dtGroupImportResult7, "dtGroupImportResult7");
                    fReport.RegisterData(dtGroupImportResult8, "dtGroupImportResult8");
                    fReport.RegisterData(dtGroupImportResult9, "dtGroupImportResult9");
                    fReport.RegisterData(dtGroupImportResult10, "dtGroupImportResult10");
                    fReport.RegisterData(dtGroupImportResult11, "dtGroupImportResult11");
                    fReport.RegisterData(dtGroupImportResult12, "dtGroupImportResult12");
                    fReport.RegisterData(dtGroupImportResult13, "dtGroupImportResult13");
                    fReport.RegisterData(dtGroupImportResult14, "dtGroupImportResult14");
                    fReport.RegisterData(dtGroupImportResult15, "dtGroupImportResult15");
                    fReport.RegisterData(dtGroupImportResult16, "dtGroupImportResult16");
                    fReport.RegisterData(dtGroupImportResult17, "dtGroupImportResult17");
                    fReport.RegisterData(dtGroupImportResult18, "dtGroupImportResult18");
                    fReport.RegisterData(dtGroupImportResult19, "dtGroupImportResult19");
                    fReport.RegisterData(dtGroupImportResult20, "dtGroupImportResult20");
                    fReport.RegisterData(dtGroupImportTable1, "dtGroupImportTable1");//注册重要提示分组后表格显示
                    fReport.RegisterData(dtGroupImportTable2, "dtGroupImportTable2");
                    fReport.RegisterData(dtGroupImportTable3, "dtGroupImportTable3");
                    fReport.RegisterData(dtGroupImportTable4, "dtGroupImportTable4");
                    fReport.RegisterData(dtGroupImportTable5, "dtGroupImportTable5");
                    fReport.RegisterData(dtGroupImportTable6, "dtGroupImportTable6");
                    fReport.RegisterData(dtGroupImportTable7, "dtGroupImportTable7");
                    fReport.RegisterData(dtGroupImportTable8, "dtGroupImportTable8");
                    fReport.RegisterData(dtGroupImportTable9, "dtGroupImportTable9");
                    fReport.RegisterData(dtGroupImportTable10, "dtGroupImportTable10");
                    fReport.RegisterData(dtGroupImportTable11, "dtGroupImportTable11");
                    fReport.RegisterData(dtGroupImportTable12, "dtGroupImportTable12");
                    fReport.RegisterData(dtGroupImportTable13, "dtGroupImportTable13");
                    fReport.RegisterData(dtGroupImportTable14, "dtGroupImportTable14");
                    fReport.RegisterData(dtGroupImportTable15, "dtGroupImportTable15");
                    fReport.RegisterData(dtGroupImportTable16, "dtGroupImportTable16");
                    fReport.RegisterData(dtGroupImportTable17, "dtGroupImportTable17");
                    fReport.RegisterData(dtGroupImportTable18, "dtGroupImportTable18");
                    fReport.RegisterData(dtGroupImportTable19, "dtGroupImportTable19");
                    fReport.RegisterData(dtGroupImportTable20, "dtGroupImportTable20");
                    fReport.RegisterData(dtGroupImportTableTitle1, "dtGroupImportTableTitle1");//注册重要提示分组后表格表头显示
                    fReport.RegisterData(dtGroupImportTableTitle2, "dtGroupImportTableTitle2");
                    fReport.RegisterData(dtGroupImportTableTitle3, "dtGroupImportTableTitle3");
                    fReport.RegisterData(dtGroupImportTableTitle4, "dtGroupImportTableTitle4");
                    fReport.RegisterData(dtGroupImportTableTitle5, "dtGroupImportTableTitle5");
                    fReport.RegisterData(dtGroupImportTableTitle6, "dtGroupImportTableTitle6");
                    fReport.RegisterData(dtGroupImportTableTitle7, "dtGroupImportTableTitle7");
                    fReport.RegisterData(dtGroupImportTableTitle8, "dtGroupImportTableTitle8");
                    fReport.RegisterData(dtGroupImportTableTitle9, "dtGroupImportTableTitle9");
                    fReport.RegisterData(dtGroupImportTableTitle10, "dtGroupImportTableTitle10");
                    fReport.RegisterData(dtGroupImportTableTitle11, "dtGroupImportTableTitle11");
                    fReport.RegisterData(dtGroupImportTableTitle12, "dtGroupImportTableTitle12");
                    fReport.RegisterData(dtGroupImportTableTitle13, "dtGroupImportTableTitle13");
                    fReport.RegisterData(dtGroupImportTableTitle14, "dtGroupImportTableTitle14");
                    fReport.RegisterData(dtGroupImportTableTitle15, "dtGroupImportTableTitle15");
                    fReport.RegisterData(dtGroupImportTableTitle16, "dtGroupImportTableTitle16");
                    fReport.RegisterData(dtGroupImportTableTitle17, "dtGroupImportTableTitle17");
                    fReport.RegisterData(dtGroupImportTableTitle18, "dtGroupImportTableTitle18");
                    fReport.RegisterData(dtGroupImportTableTitle19, "dtGroupImportTableTitle19");
                    fReport.RegisterData(dtGroupImportTableTitle20, "dtGroupImportTableTitle20");
                    fReport.RegisterData(dtCustomernexttest, "dtCustomernexttest");

                    fReport.Prepare(true);
                }
                return fReport;
            }
            catch
            {
                return fReport;
            }
        }
        #endregion

        #region  辅助方法
        /// <summary>截取年龄 >=5岁不取月日时，＜5岁时才取月日时
        /// 截取年龄 >=5岁不取月日时，＜5岁时才取月日时
        /// </summary>
        /// <param name="objage"></param>
        /// <returns></returns>
        public string SetAge(object objage)
        {
            string age = string.Empty;
            string[] strage = objage.ToString().Split('岁');
            int year = Convert.ToInt32(strage[0]);
            if (year >= 5) { age = strage[0] + "岁"; }
            else
            {
                age += strage[0] + "岁";

                string[] strmonth = strage[1].Split('月');
                int month = Convert.ToInt32(strmonth[0]);
                if (month > 0) { age += month + "月"; }

                string[] strday = strmonth[1].Split('日');
                int day = Convert.ToInt32(strday[0]);
                if (day > 0) { age += day + "日"; }

                string[] strhour = strday[1].Split('时');
                int hour = Convert.ToInt32(strhour[0]);
                if (hour > 0) { age += hour + "时"; }
            }
            return age;
        }
        #endregion
    }
}
