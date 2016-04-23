using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using daan.service.dict;
using FastReport;
using daan.domain;
using ExtAspNet;
using System.Threading;
using daan.service.common;
using daan.web.code;
using System.Data;
using daan.util.Common;
using System.Text;

namespace daan.web.admin.report
{
    public partial class RepMenage : PageBase
    {
        DictreporttemplateService reportService = new DictreporttemplateService();
        Dictreporttemplate reporttemplate = null;
        string erreyType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDelete.OnClientClick = gridReportList.GetNoSelectionAlertReference("至少选择一项！");
                btnDelete.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", gridReportList.GetSelectCountReference());
                BandReportType();
                BandGrid();
            }
        }

        #region >>>初始化及分页
        //绑定报告类型
        private void BandReportType()
        {
            DDLInitbasicBinder(dropReportType, "REPORTTYPE");
            dropReportType.SelectedIndex = 0;
        }
        //绑定Grid数据
        private void BandGrid()
        {
            //清除掉已选择的项，以防报下标错误
            gridReportList.SelectedRowIndexArray = new int[] { };

            gridReportList.DataSource = reportService.GetDictreporttemplateAll();
            gridReportList.DataBind();

            SetEditDate();
        }
        #endregion

        #region >>>编辑
        //获取选中行详细信息
        protected void gridReportList_RowClick(object sender, GridRowClickEventArgs e)
        {
            txtTemCode.Enabled = false;
            SetEditDate();
        }
        //新增
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            gridReportList.SelectedRowIndexArray = new int[] { };
            txtTemCode.Enabled = true;
            SetEditDate();
        }
        //保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //获取文本框值
            if (GetEditDate())
            {
                MessageBoxShow("保存数据成功！");
                gridReportList.SelectedRowIndexArray = new int[] { };
                BandGrid();
            }
            else
            {
                MessageBoxShow(erreyType);
                gridReportList.SelectedRowIndexArray = new int[] { };
                BandGrid();
            }
        }
        //删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridReportList.SelectedRowIndexArray.Count<int>() > 0)
                {
                    MessageBoxShow("请选择要删除的模板", MessageBoxIcon.Warning);
                }

                string[] row = gridReportList.Rows[gridReportList.SelectedRowIndexArray[0]].Values;
                if (reportService.DelReporttemplateByID(row[0].ToString()) > 0)
                {
                    MessageBoxShow("删除成功！", MessageBoxIcon.Information);
                    gridReportList.SelectedRowIndexArray = new int[] { };
                    BandGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        //加载当前行至编辑框
        protected void SetEditDate()
        {
            //新增
            reporttemplate = new Dictreporttemplate();
            SimpleFormEdit.Title = "当前状态-新增";

            //编辑
            if (gridReportList.SelectedRowIndexArray.Length > 0)
            {
                SimpleFormEdit.Title = "当前状态-编辑";

                string[] row = gridReportList.Rows[gridReportList.SelectedRowIndexArray[0]].Values;

                reporttemplate.Dictreporttemplateid = TypeParse.StrToDouble(row[0], 0);
                reporttemplate.Templatename = TypeParse.ObjToStr(row[1], "");
                reporttemplate.Templatecode = TypeParse.ObjToStr(row[2], "");
                reporttemplate.Papersize = TypeParse.ObjToStr(row[3], "");
                reporttemplate.Singleappraise = TypeParse.ObjToStr(row[4], "");
                reporttemplate.Reporttype = TypeParse.StrToDouble(row[5], 0);
                reporttemplate.Remark = TypeParse.ObjToStr(row[6], "");
            }
            txtTemName.Text = reporttemplate.Templatename;
           
            txtTemCode.Text = reporttemplate.Templatecode;
            txtPageSize.Text = reporttemplate.Papersize;
            cbSingleAppraise.Checked = reporttemplate.Singleappraise == "0" ? true : false;
            dropReportType.SelectedValue = reporttemplate.Reporttype.ToString();
            txtRemark.Text = reporttemplate.Remark;
        }
        //加载文本框数据至对象
        protected bool GetEditDate()
        {
            reporttemplate = new Dictreporttemplate();
            if (gridReportList.SelectedRowIndexArray.Count<int>() > 0)
            {
                string[] row = gridReportList.Rows[gridReportList.SelectedRowIndexArray[0]].Values;
                reporttemplate.Dictreporttemplateid = Convert.ToDouble(row[0]);
            }
            else
            {
                reporttemplate.Dictreporttemplateid = 0;
            }
            if (!this.txtPageSize.Text.Trim().Equals(""))
            {
                reporttemplate.Papersize = this.txtPageSize.Text.Trim();
            }
            else
            {
                erreyType = "纸张类型不能为空！";
                return false;
            }
            reporttemplate.Remark = this.txtRemark.Text.Trim();
            reporttemplate.Reporttype = double.Parse(this.dropReportType.SelectedValue);
            reporttemplate.Singleappraise = this.cbSingleAppraise.Checked ? "1" : "0";
            if (!this.txtTemCode.Text.Trim().Equals(""))
            {
                reporttemplate.Templatecode = this.txtTemCode.Text.Trim();
            }
            else
            {
                erreyType = "模板代码不能为空！";
                return false;
            }
            if (!this.txtTemName.Text.Trim().Equals(""))
            {
                reporttemplate.Templatename = this.txtTemName.Text.Trim();
            }
            else
            {
                erreyType = "模板名称不能为空！";
                return false;
            }
            return reportService.SaveReporttemplate(reporttemplate);
        }
        #endregion
    }
}