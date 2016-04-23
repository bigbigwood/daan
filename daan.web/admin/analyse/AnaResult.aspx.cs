using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtAspNet;
using daan.domain;
using daan.service.common;
using daan.service.dict;
using System.Data;
using daan.service.order;
using System.Collections;
using daan.service.login;
using daan.util.Common;
using System.Text;
using daan.util.Web;
using daan.service.proceed;
using daan.web.code;
namespace daan.web.admin.analyse
{
    public partial class AnaResult : PageBase
    {
        #region >>>0.变量定义 Page_Load
        LoginService loginservice = new LoginService();
        OrderTestService orderTestService = new OrderTestService();
        OrderlabdeptresultService labdeptresultService = new OrderlabdeptresultService();
        OrderdiagnosisService orderdiagnosisService = new OrderdiagnosisService();
        List<object> orderUserInfo = null;//获取选中客户的订单号等信息

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                btnLog.OnClientClick = gvOrderUser.GetNoSelectionAlertReference("您还未选择！", "体检系统");
                if (gvOrderUser.SelectedRowIndexArray.Count() > 0)
                {
                    orderUserInfo = gvOrderUser.DataKeys[gvOrderUser.SelectedRowIndexArray[0]].ToList();

                    //诊断建议模板初始化
                    btnOrderdiagnosisModel.OnClientClick = WinSuggestion.GetShowReference("AnaResultSum_AddDictdiagnosisWin.aspx?" +
                      "ordernum=" + orderUserInfo[0]);
                }
            }
            else
            {
                BindDictLab();
                dpStart.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                dpEnd.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }
        #endregion

        #region >>>1.初始化数据
        /// <summary>
        /// 绑定分点
        /// </summary>
        private void BindDictLab()
        {
            DDLDictLabBinder(ddlDictlab, true);
        }
        #endregion

        #region >>>2.搜索
        // 执行清空动作
        protected void txtSearch_Trigger1Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            txtSearch.ShowTrigger1 = false;
        }
        // 执行搜索动作   
        protected void txtSearch_Trigger2Click(object sender, EventArgs e)
        {
            if (this.dpStart.Text != "" && this.dpEnd.Text != "")
            {
                if (this.dpStart.SelectedDate <= this.dpEnd.SelectedDate)
                {
                    txtSearch.ShowTrigger1 = true;
                    gvOrderUser.PageIndex = 0;
                    Binder();
                }
                else
                {

                    MessageBoxShow("结束时间应大于开始时间！", MessageBoxIcon.Information);
                }
            }
            else
            {
                if (this.dpStart.Text != "" || this.dpEnd.Text != "")
                {

                    MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
                }
                else
                {
                    txtSearch.ShowTrigger1 = true;
                    gvOrderUser.PageIndex = 0;
                    Binder();

                }
            }

        }
        #endregion

        #region >>>2.左边待录入结果检查人员列表绑定
        private void Binder()
        {
            PageUtil pageUtil = new PageUtil(gvOrderUser.PageIndex, gvOrderUser.PageSize);
            Hashtable ht1 = new Hashtable();
            ht1.Add("nState", ddlStatus.SelectedValue == "-1" ? "5,10" : ddlStatus.SelectedValue);
            if (ddlDictlab.SelectedValue == "-1")
                ht1.Add("nDictlabid", Userinfo.joinLabidstr);
            else
                ht1.Add("nDictlabid", ddlDictlab.SelectedValue == "-1" ? null : ddlDictlab.SelectedValue);
            ht1.Add("strKey", TextUtility.ReplaceText(txtSearch.Text) == "" ? null : TextUtility.ReplaceText(txtSearch.Text));
            ht1.Add("startDate", dpStart.Text.ToString() == "" ? null : dpStart.Text.ToString());
            ht1.Add("endDate", dpEnd.Text.ToString() == "" ? null : dpEnd.Text.ToString());
            ht1.Add("pageStart", pageUtil.GetPageStartNum());
            ht1.Add("pageEnd", pageUtil.GetPageEndNum());

            //设置总项数
            gvOrderUser.RecordCount = orderTestService.GetOrderMemberPageLstCount(ht1);

            DataSet ds = orderTestService.GetOrderMemberPageLst(ht1);
            gvOrderUser.DataSource = ds.Tables[0].DefaultView;
            gvOrderUser.DataBind();

            #region 初始化
            // 科室检查结果列表
            List<Ordertest> tempOrderTestLst = new List<Ordertest>();
            gvOrderTest.DataSource = tempOrderTestLst;
            gvOrderTest.DataBind();

            //绑定科室小结列表
            gvOrderLabdeptResult.DataSource = OrderlabdeptresultData("0");
            gvOrderLabdeptResult.DataBind();

            //绑定诊断建议列表
            gvSuggestion.DataSource = OrderdiagnosisData("0");
            gvSuggestion.DataBind();
            gvOrderUser.SelectedRowIndexArray = new int[] { };
            #endregion
        }
        //分页
        protected void gvOrderUser_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            txtSearch.Text = "";
            gvOrderUser.PageIndex = e.NewPageIndex;
            Binder();
        }
        #endregion

        #region >>>3.选择待录入结果检查人员,绑定科室检查结果
        protected void gvOrderUser_RowClick(object sender, GridRowClickEventArgs e)
        {
            highlightRows.Text = "";
            txtSearch.Text = orderUserInfo[0].ToString();


            //未选择客户提示
            if (orderUserInfo == null)
            {
                MessageBoxShow("请至少选择一位客户！", MessageBoxIcon.Warning);
                gvOrderUser.SelectedRowIndexArray = new int[] { e.RowIndex };
                return;
            }

            //初始化客户信息
            labInfo.Text = string.Format("[姓名:{0}][性别:{1}][年龄:{2}][身份证号:{3}][单位:{4}]", orderUserInfo[1], orderUserInfo[2], WebUI.GetAge(orderUserInfo[3]), orderUserInfo[4], orderUserInfo[5]);

            //科室检查结果列表
            List<Ordertest> tempOrderTestLst = OrderTestData(orderUserInfo[0].ToString()).ToList<Ordertest>();
            gvOrderTest.DataSource = tempOrderTestLst;
            gvOrderTest.DataBind();

            //判断是否已审核，如果审核，检查结果、科室小结、诊断建议不能修改
            if (orderUserInfo[7].ToString() == ((int)ParamStatus.Orderlabdepstatus.Audited).ToString())
            {
                //检查结果
                txtTestResult.Enabled = false;
                chkIsexception.Enabled = false;
                btnTestResult.Hidden = true;
                btnResultSelect.Hidden = true;

                //科室小结
                txtOrderLabdeptResult.Enabled = false;
                btnSaveOrderLabdeptResult.Hidden = true;
                //诊断建议
                txtEditSuggestion.Enabled = false;
                btnOrderdiagnosisModel.Hidden = true;
                btnSaveOrderdiagnosis.Hidden = true;
            }
            else
            {
                txtTestResult.Enabled = true;
                chkIsexception.Enabled = true;
                btnTestResult.Hidden = false;
                btnResultSelect.Hidden = false;

                txtOrderLabdeptResult.Enabled = true;
                btnSaveOrderLabdeptResult.Hidden = false;

                txtEditSuggestion.Enabled = true;
                btnOrderdiagnosisModel.Hidden = false;
                btnSaveOrderdiagnosis.Hidden = false;
            }

            //绑定科室小结列表
            gvOrderLabdeptResult.DataSource = OrderlabdeptresultData(orderUserInfo[0].ToString());
            gvOrderLabdeptResult.DataBind();

            //绑定诊断建议列表
            gvSuggestion.DataSource = OrderdiagnosisData(orderUserInfo[0].ToString());
            gvSuggestion.DataBind();

            //清空编辑区先前选择的结果、科室小结、诊断建议
            txtEditSuggestion.Text = "";
            txtOrderLabdeptResult.Text = "";
            txtTestResult.Text = "";
            chkIsexception.Checked = false;

        }
        //科室检查结果数据源
        private IList<Ordertest> OrderTestData(string strOrderNum)
        {
            Hashtable ht = new Hashtable();
            ht.Add("OrderNum", strOrderNum);
            ht.Add("UserId", Userinfo.userId);
            return orderTestService.GetOrderLabdeptresultList(ht);
        }
        //科室小结数据源
        private IList<Orderlabdeptresult> OrderlabdeptresultData(string orderNum)
        {
            Hashtable ht = new Hashtable();
            ht.Add("OrderNum", orderNum);
            ht.Add("UserId", Userinfo.userId);
            return labdeptresultService.GetOrderlabdeptresultLst(ht);
        }
        //诊断建议数据源
        private IList<Orderdiagnosis> OrderdiagnosisData(string orderNum)
        {
            Hashtable ht = new Hashtable();
            ht.Add("OrderNum", orderNum);
            ht.Add("UserId", Userinfo.userId);
            return orderdiagnosisService.SelectSingleOrderdiagnosisLst(ht);
        }
        //获取小结状态描述
        public string GetStatus(object objValue)
        {
            switch (objValue.ToString())
            {
                case "5":
                    return "结果待查";
                case "10":
                    return "已小结";
                default:
                    return "已审核";
            }
        }
        #endregion

        #region >>>4.录入科室检查结果
        //将选中的结果录入值给编辑框
        protected void gvOrderTest_RowClick(object sender, GridRowClickEventArgs e)
        {

            if (gvOrderTest.SelectedRowIndexArray.Count<int>() != 0)
            {
                object[] objValue = gvOrderTest.DataKeys[gvOrderTest.SelectedRowIndexArray[0]];
                //是否检验项目，如果是结果值不能更改[ParamStatus.LabdeptType.FunctionDepartment为检验项目]
                List<Dictlabdept> labdept = loginservice.GetLoginDictlabdeptList();
                if (objValue[6].ToString() != "22"&&labdept.Count(c =>c.Labdepttype == ((int)ParamStatus.LabdeptType.InspectionDepartment).ToString() && c.Dictlabdeptid == TypeParse.StrToDouble(objValue[6], 0)) > 0)
                {
                    txtTestResult.Enabled = false;
                    btnTestResult.Hidden = true;
                    btnResultSelect.Hidden = true;
                    chkIsexception.Enabled = false;
                }
                else
                {
                    txtTestResult.Enabled = true;
                    btnTestResult.Hidden = false;
                    btnResultSelect.Hidden = false;
                    chkIsexception.Enabled = true;
                }
                txtTestResult.Text = TypeParse.ObjToStr(objValue[3], "");
                chkIsexception.Checked = TypeParse.ObjToStr(objValue[7], "正常") == "正常" ? false : true;

                //结果录入模板初始化
                btnResultSelect.OnClientClick = WinResultEdit.GetSaveStateReference(txtTestResult.ClientID)
                        + WinResultEdit.GetShowReference(string.Format("AnaResult_Windows.aspx?testId={0}", objValue[4]), "检查结果模板选择");
            }
            else
            {
                txtTestResult.Text = "";
            }

        }
        //保存编辑的值
        protected void btnTestResult_Click(object sender, EventArgs e)
        {
            highlightRows.Text = "";
            //没有选取行
            if (gvOrderTest.SelectedRowIndexArray.Count<int>() == 0)
            {
                MessageBoxShow("请选择检查项目！", MessageBoxIcon.Warning);
                return;
            }
            if (txtTestResult.Text.Trim() == "")
            {
                MessageBoxShow("请填写检验结果！");
                return;
            }
            //检查项列表
            List<Ordertest> ordertestLst = OrderTestData(orderUserInfo[0].ToString()).ToList<Ordertest>();
            //选中检查项详细
            List<object> objValue = gvOrderTest.DataKeys[gvOrderTest.SelectedRowIndexArray[0]].ToList();
            var testResult = ordertestLst.Find(c => c.Ordertestid == TypeParse.StrToDouble(objValue[0], 0));
            testResult.Testresult = txtTestResult.Text.Trim();
            testResult.Isexception = chkIsexception.Checked ? "1" : "0";
            //testResult.Testresult = txtTestResult.Text.Trim();

            //高低提示
            switch (testResult.Testname)
            {
                case "体重":
                    GetBMLHlflag(ordertestLst);
                    break;
                case "身高":
                    GetBMLHlflag(ordertestLst);
                    break;
                case "收缩压":
                    testResult.Hlhint = GetSBPlflag(testResult);
                    //异常判断
                    testResult.Isexception = testResult.Hlhint == "正常" ? "0" : "1";
                    break;
                case "舒张压":
                    testResult.Hlhint = GetDBPlflag(testResult);
                    //异常判断
                    testResult.Isexception = testResult.Hlhint == "正常" ? "0" : "1";
                    break;
                default:
                    break;
            }
            try
            {
                if (orderTestService.UpdateOrdertestResult(testResult))
                {
                    //绑定科室小结列表                       
                    gvOrderTest.DataSource = OrderTestData(orderUserInfo[0].ToString());
                    gvOrderTest.DataBind();
                    txtTestResult.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }

        }
        //获取体重指数
        private void GetBMLHlflag(List<Ordertest> orderTestLst)
        {
            //如果有体重相关检查项，则计算体重指数

            Ordertest weight = orderTestLst.Find(c => c.Testname == "体重" && !string.IsNullOrEmpty(c.Testresult));
            Ordertest height = orderTestLst.Find(c => c.Testname == "身高" && !string.IsNullOrEmpty(c.Testresult));


            if (weight != null && height != null)
            {
                Ordertest bml = orderTestLst.Find(c => c.Testname == "体重指数");
                //为空时不处理
                if (bml == null)
                {
                    return;
                }
                //体重指数
                double bmlValue = 0;
                try
                {
                    if (double.Parse(weight.Testresult) == 0 || double.Parse(height.Testresult) == 0)
                    {
                        MessageBoxShow("身高或者体重不能为0", MessageBoxIcon.Warning);
                        return;
                    }
                    bmlValue = Math.Round(double.Parse(weight.Testresult) / Math.Pow(double.Parse(height.Testresult), 2), 3);
                }
                catch (Exception ex)
                {
                    MessageBoxShow("身高或者体重必须为数字", MessageBoxIcon.Warning);
                    return;
                }
                bml.Testresult = bmlValue.ToString();
                bml.Isexception = "1";
                if (bmlValue < 18.5)
                {
                    bml.Hlhint = "消瘦";
                }
                else if (bmlValue >= 18.5 && bmlValue < 24)
                {
                    bml.Hlhint = "正常";
                    bml.Isexception = "0";
                }
                else if (bmlValue >= 24 && bmlValue < 28)
                {
                    bml.Hlhint = "超重";
                }
                else
                {
                    bml.Hlhint = "肥胖";
                }
                try
                {
                    orderTestService.UpdateOrdertestResult(bml);
                }
                catch (Exception ex)
                {
                    MessageBoxShow("体重指数计算出错，错误：" + ex.Message, MessageBoxIcon.Error);
                }

                //写日志
                new BaseService().AddOperationLog(bml.Ordernum, "", "检查结果录入", "对[" + bml.Ordernum + "]自动计算体重指数", "修改留痕", "");
            }
        }
        //获收缩压高低值判断
        private string GetSBPlflag(Ordertest paraResult)
        {
            double nResult = TypeParse.StrToDouble(paraResult.Testresult, 0);
            string strbp = string.Empty;//描述
            if (nResult < 90)
            {
                return "偏低";
            }
            else if (nResult >= 90 && nResult <= 139)
            {
                return "正常";
            }
            else
            {
                return "偏高";
            }
        }
        //获舒张压高低值判断
        private string GetDBPlflag(Ordertest paraResult)
        {
            double nResult = TypeParse.StrToDouble(paraResult.Testresult, 0);
            if (nResult < 60)
            {
                return "偏低";
            }
            else if (nResult >= 60 && nResult <= 89)
            {
                return "正常";
            }
            else
            {
                return "偏高";
            }
        }
        #endregion

        #region >>>5.自动小结
        protected void btnAutoSummary_Click(object sender, EventArgs e)
        {
            //未选择客户提示
            if (orderUserInfo == null)
            {
                MessageBoxShow("请至少选择一位客户！", MessageBoxIcon.Warning);
                return;
            }

            //2,获取体检号对应的检查项目
            IList<Ordertest> orderTestList = OrderTestData(orderUserInfo[0].ToString());

            //3,是否有检查项目
            if (orderTestList.Count <= 0)
            {
                MessageBoxShow("没有检查项目,不能自动小结。", MessageBoxIcon.Information);
                return;
            }
            //4,查询检查结果是否都填写，否则提示要求全部填写
            if (orderTestList.Count<Ordertest>(c => string.IsNullOrEmpty(c.Testresult)) > 0)
            {
                MessageBoxShow("请确认检查项目已全部录入结果，否则不能自动小结。", MessageBoxIcon.Information);
                return;
            }
            //自动小结
            try
            {
                labdeptresultService.AutoSummary(orderUserInfo[0].ToString(),Convert.ToDouble(ddlDictlab.SelectedValue));
                txtSearch.Text = "";
                txtSearch_Trigger2Click(null,null);

            }
            catch(Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region >>>6.科室小结编辑
        //将选中的科室小结值给编辑框
        protected void gvOrderLabdeptResult_RowClick(object sender, GridRowClickEventArgs e)
        {
            if (gvOrderLabdeptResult.SelectedRowIndexArray.Count<int>() != 0)
            {
                object[] objValue = gvOrderLabdeptResult.DataKeys[gvOrderLabdeptResult.SelectedRowIndexArray[0]];
                txtOrderLabdeptResult.Text = TypeParse.ObjToStr(objValue[2], "");
            }
            else
            {
                txtOrderLabdeptResult.Text = "";
            }
        }
        //科室小结编辑
        protected void btnSaveOrderLabdeptResult_Click(object sender, EventArgs e)
        {
            //没有选取行
            if (gvOrderLabdeptResult.SelectedRowIndexArray.Count<int>() == 0)
            {
                MessageBoxShow("请选择要编辑的科室小结", MessageBoxIcon.Information);
                return;
            }
            object[] objValue = gvOrderLabdeptResult.DataKeys[gvOrderLabdeptResult.SelectedRowIndexArray[0]];

            List<Orderlabdeptresult> labdeptresultLst = OrderlabdeptresultData(objValue[1].ToString()).ToList<Orderlabdeptresult>();

            var labdeptresult = labdeptresultLst.Find(c => c.Ordertlabdeptresultid == TypeParse.StrToDouble(objValue[0], 0));
            labdeptresult.Labdeptresult = txtOrderLabdeptResult.Text;
            try
            {
                if (labdeptresultService.UpdateOrderlabdeptresult(labdeptresult))
                {
                    //绑定科室小结列表
                    gvOrderLabdeptResult.DataSource = OrderlabdeptresultData(objValue[1].ToString());
                    gvOrderLabdeptResult.DataBind();
                    txtOrderLabdeptResult.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow("科室小结保存出错，错误描述：" + ex.Message, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region >>>7.诊断建议编辑
        //将选中的诊断建议值给编辑框
        protected void gvSuggestion_RowClick(object sender, GridRowClickEventArgs e)
        {
            if (gvSuggestion.SelectedRowIndexArray.Count<int>() != 0)
            {
                object[] objValue = gvSuggestion.DataKeys[gvSuggestion.SelectedRowIndexArray[0]];
                txtEditSuggestion.Text = TypeParse.ObjToStr(objValue[2], "");
            }
            else
            {
                txtEditSuggestion.Text = "";
            }
        }
        //诊断建议编辑
        protected void btnSaveOrderdiagnosis_Click(object sender, EventArgs e)
        {
            highlightRows.Text = "";
            //没有选取行
            if (gvSuggestion.SelectedRowIndexArray.Count<int>() == 0)
            {
                MessageBoxShow("请选择要编辑的诊断建议", MessageBoxIcon.Information);
                return;
            }
            object[] objValue = gvSuggestion.DataKeys[gvSuggestion.SelectedRowIndexArray[0]];

            List<Orderdiagnosis> diagnosisLst = OrderdiagnosisData(objValue[1].ToString()).ToList<Orderdiagnosis>();

            var diagnosis = diagnosisLst.Find(c => c.Orderdiagnosisid == TypeParse.StrToDouble(objValue[0], 0));
            diagnosis.Suggestion = txtEditSuggestion.Text;
            try
            {
                if (orderdiagnosisService.UpdateOrderdiagnosis(diagnosis))
                {
                    //绑定诊断建议列表
                    gvSuggestion.DataSource = OrderdiagnosisData(objValue[1].ToString());
                    gvSuggestion.DataBind();
                    txtEditSuggestion.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow("诊断建议保存出错，错误描述：" + ex.Message, MessageBoxIcon.Error);
            }
        }
        //诊断建议模板选择后刷新新加的诊断信息
        protected void WinSuggestion_Close(object sender, WindowCloseEventArgs e)
        {
            gvSuggestion.DataSource = OrderdiagnosisData(orderUserInfo[0].ToString());
            gvSuggestion.DataBind();
        }
        #endregion

        #region >>>8.删除诊断建议
        protected void btnDelDiagnosis_Click(object sender, EventArgs e)
        {
            if (gvSuggestion.SelectedRowIndexArray.Count<int>() > 0)
            {
                List<object> list = gvSuggestion.DataKeys[gvSuggestion.SelectedRowIndexArray[0]].ToList();
                try
                {
                    orderdiagnosisService.DeleteOrderdiagnosis(list[0].ToString());
                    //绑定诊断建议列表
                    gvSuggestion.DataSource = OrderdiagnosisData(list[1].ToString());
                    gvSuggestion.DataBind();

                }
                catch (Exception ex)
                {
                    MessageBoxShow(ex.Message, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBoxShow("请选择要删除的诊断建议", MessageBoxIcon.Information);
            }
        }
        #endregion

        #region >>>9.更改审核者权限内科室的结果审核状态
        //protected void btnAudit_Click(object sender, EventArgs e)
        //{
        //    //未选择客户提示
        //    if (orderUserInfo == null)
        //    {
        //        MessageBoxShow("请至少选择一位客户！", MessageBoxIcon.Warning);
        //        return;
        //    }
        //    //2,获取体检号对应的检查项目
        //    IList<Ordertest> orderTestList = OrderTestData(orderUserInfo[0].ToString());

        //    //3,是否有检查项目
        //    if (orderTestList.Count <= 0)
        //    {
        //        MessageBoxShow("没有检查项目,不能审核通过！", MessageBoxIcon.Information);
        //        return;
        //    }

        //    //4,查询检查结果是否都填写，否则提示要求全部填写
        //    if (orderTestList.Count<Ordertest>(c => string.IsNullOrEmpty(c.Testresult)) > 0)
        //    {
        //        MessageBoxShow("请确认检查项目已全部录入结果，否则不能审核通过！", MessageBoxIcon.Information);
        //        return;
        //    }
        //    Hashtable ht = new Hashtable();
        //    ht.Add("OrderNum", orderUserInfo[0].ToString());
        //    ht.Add("UserId", Userinfo.userId); //登录用户
        //    ht.Add("Status", (int)ParamStatus.Orderlabdepstatus.Audited);
        //    try
        //    {

        //        bool flag = false;
        //        if (labdeptresultService.UpdateOrderlabdeptresultState(ht, out flag))
        //        {
        //            //写日志
        //            new BaseService().AddOperationLog(orderUserInfo[0].ToString(), "", "检查结果录入", "对[" + orderUserInfo[0].ToString() + "]审核通过", "修改留痕", "");
        //            MessageBoxShow("审核成功", MessageBoxIcon.Information);
        //            highlightRows.Text = "";
        //            txtSearch_Trigger2Click(null, null);
        //            if (flag)
        //            {
        //                // 科室检查结果列表
        //                List<Ordertest> tempOrderTestLst = new List<Ordertest>();
        //                gvOrderTest.DataSource = tempOrderTestLst;
        //                gvOrderTest.DataBind();

        //                //绑定科室小结列表
        //                gvOrderLabdeptResult.DataSource = OrderlabdeptresultData("0");
        //                gvOrderLabdeptResult.DataBind();

        //                //绑定诊断建议列表
        //                gvSuggestion.DataSource = OrderdiagnosisData("0");
        //                gvSuggestion.DataBind();
        //            }
        //            else
        //            {
        //                TabStrip1.ActiveTabIndex = 1;
        //                gvOrderLabdeptResult.DataSource = OrderlabdeptresultData(orderUserInfo[0].ToString());
        //                gvOrderLabdeptResult.DataBind();

        //            }
        //            //清空编辑区先前选择的结果、科室小结、诊断建议
        //            labInfo.Text = "";
        //            txtEditSuggestion.Text = "";
        //            txtOrderLabdeptResult.Text = "";
        //            txtTestResult.Text = "";
        //            chkIsexception.Checked = false;

        //        }
        //        else
        //        {
        //            MessageBoxShow("自动小结后才能进行审核", MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBoxShow(ex.Message, MessageBoxIcon.Error);
        //    }
        //}
        #endregion

        #region >>>9.辅助方法
        /// <summary>
        /// 判断subset的无重复集合是不是bigset的子集
        /// </summary>
        /// <param name="bigSet"></param>
        /// <param name="subSet"></param>
        /// <returns></returns>
        private bool isSubSet(string[] bigSet, string[] subSet)
        {

            for (int i = 0; i < subSet.Length; i++)
            {
                if (string.IsNullOrEmpty(subSet[i])) continue;
                if (!bigSet.Contains(subSet[i]))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 将集合串成字符串
        /// </summary>
        /// <param name="lst">集合</param>
        /// <param name="ch">集合元素分隔符</param>
        /// <returns></returns>
        private string arrayToString(ICollection lst, char ch)
        {
            string result = "";
            IEnumerator e = lst.GetEnumerator();
            int k = 0;
            while (e.MoveNext())
            {
                k++;
                if (ch == '\n')
                {
                    result += k.ToString() + ": " + e.Current.ToString() + ch;

                }
                else
                {
                    result += e.Current.ToString() + ch;
                }
            }
            return result.TrimEnd(new char[] { ch });
        }
        #endregion

        #region >>>10.日志查询
        protected void btnLog_Click(object sender, EventArgs e)
        {
            if (gvOrderUser.Rows.Count <= 0 || gvOrderUser.SelectedRowIndexArray.Length <= 0)
                return;
            object[] objValue = gvOrderUser.DataKeys[gvOrderUser.SelectedRowIndexArray[0]];
            string orderNum = TypeParse.ObjToStr(objValue[0], "");
            WinBillRemark.Hidden = false;
            WinBillRemark.IFrameUrl = "../bill/BillOperationLog.aspx?ordernum=" + orderNum;
            WinBillRemark.Title = "订单日志查询";
        }
        #endregion

        /// <summary>
        /// 结果录入绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOrderTest_RowDataBound(object sender, ExtAspNet.GridRowEventArgs e)
        {
            //实体接收数据
            Ordertest row = e.DataItem as Ordertest;
            if (row != null)
            {
                string entranceYear = row.IsexceptionToBool;
                if (entranceYear == "异常")
                {
                    highlightRows.Text += e.RowIndex.ToString() + ",";
                }
            }
        }
    }
}