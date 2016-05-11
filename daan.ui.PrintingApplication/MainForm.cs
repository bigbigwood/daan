using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CCWin;
using CCWin.SkinClass;
using daan.ui.PrintingApplication.Control;
using daan.ui.PrintingApplication.PrintingImpl;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models;
using daan.webservice.PrintingSystem.Contract.Models.Order;
using daan.webservice.PrintingSystem.Contract.Models.Report;
using log4net;

namespace daan.ui.PrintingApplication
{
    public partial class MainForm : CCSkinMain
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainFormTabImpl_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            BindQueryGroup();

            BindDataGrid();
        }

        private void BindDataGrid()
        {
            pagerControl1.PageIndex = 1;
            pagerControl1.PageSize = 10;
            pagerControl1.OnPageChanged += new EventHandler(pagerControl1_OnPageChanged);

            AddCheckBoxToDataGridView.dgv = dgv_orders;
            AddCheckBoxToDataGridView.AddFullSelect();
            dgv_orders.AutoGenerateColumns = false;
        }

        private void BindQueryGroup()
        {
            dropStatus.DataSource = new ComboBoxDataSourceProvider().GetOrderStatusDataSource();
            dropStatus.ValueMember = "EnumValue";
            dropStatus.DisplayMember = "EnumDisplayText";
            dropStatus.SelectedValue = -1;

            dropReportStatus.DataSource = new ComboBoxDataSourceProvider().GetReportStatusDataSource();
            dropReportStatus.ValueMember = "EnumValue";
            dropReportStatus.DisplayMember = "EnumDisplayText";
            dropReportStatus.SelectedValue = (int)ReportStatus.Normal;

            var labList = new List<LabInfo>() { new LabInfo() { Id = -1, Name = "全部" } };
            labList.AddRange(PrintingApp.LabAssociations);
            dropDictLab.DataSource = labList;
            dropDictLab.ValueMember = "Id";
            dropDictLab.DisplayMember = "Name";
            if (PrintingApp.CurrentUserInfo.DefaultLab != null && PrintingApp.CurrentUserInfo.DefaultLab.Id != 0)
                dropDictLab.SelectedValue = PrintingApp.CurrentUserInfo.DefaultLab.Id;

            var organizationList = new List<OrganizationInfo>() { new OrganizationInfo() { Id = -1, Name = "全部" } };
            organizationList.AddRange(PrintingApp.OrganizationAssociations);
            dropDictcustomer.DataSource = organizationList;
            dropDictcustomer.ValueMember = "Id";
            dropDictcustomer.DisplayMember = "Name";

            dpFrom.Value = DateTime.Now.AddDays(-7);
            dpTo.Value = DateTime.Now;
            dpSFrom.Value = DateTime.Now.AddDays(-7);
            dpSTo.Value = DateTime.Now;
        }

        private void PresentData(QueryOrdersResponse response)
        {
            dgv_orders.DataSource = response.Result;
            pagerControl1.DrawControl(response.OrderCount);
        }

        private QueryOrdersRequest GetQueryOrdersRequest()
        {
            var request = new QueryOrdersRequest();
            request.Username = PrintingApp.UserCredential.UserName;
            request.Password = PrintingApp.UserCredential.Password;
            request.OrderNumber = tbxOrderNum.Text;
            request.PageStart = ((pagerControl1.PageIndex - 1) * pagerControl1.PageSize + 1).ToString();
            request.PageEnd = ((pagerControl1.PageIndex - 1) * pagerControl1.PageSize + pagerControl1.PageSize).ToString();
            request.StartDate = dpFrom.Value.ToString("yyyy-MM-dd");
            request.EndDate = dpTo.Value.AddDays(1).ToString("yyyy-MM-dd");
            request.SDateBegin = dpSFrom.Value.ToString("yyyy-MM-dd");
            request.SDateEnd = dpSTo.Value.AddDays(1).ToString("yyyy-MM-dd");
            request.Name = tbxName.Text;

            request.Dictlabid = dropDictLab.SelectedValue.ToString();
            request.Dictcustomerid = (dropDictcustomer.SelectedValue.ToString() != "-1") ? dropDictcustomer.SelectedValue.ToString() : null;
            request.Status = (dropStatus.SelectedValue.ToString() != "-1") ? dropStatus.SelectedValue.ToString() : null;
            request.ReportStatus = (dropReportStatus.SelectedValue.ToString() != "-1") ? dropReportStatus.SelectedValue.ToString() : null;

            return request;
        }


        private void btnQueryOrder_Click(object sender, EventArgs e)
        {
            string url = ConfigurationManager.AppSettings.Get("PrintingServiceServiceUrl");
            var printingService = ServiceFactory.GetPrintingService(url);
            var response = printingService.QueryOrders(GetQueryOrdersRequest());

            PresentData(response);
        }

        void pagerControl1_OnPageChanged(object sender, EventArgs e)
        {
            string url = ConfigurationManager.AppSettings.Get("PrintingServiceServiceUrl");
            var printingService = ServiceFactory.GetPrintingService(url);
            var response = printingService.QueryOrders(GetQueryOrdersRequest());

            PresentData(response);
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            var dialogResult = MessageBox.Show("确定打印吗？", "确定打印吗？", MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.No)
                return;

            if (AddCheckBoxToDataGridView.GetSelectedRows().Count == 0)
            {
                MessageBox.Show("请先选择订单");
                return;
            }

            if (AddCheckBoxToDataGridView.GetSelectedRows().Any(r => r.Cells["Cell_OrderStatus"].ToInt32(0) < (int)OrdersStatus.FinishCheck))
            {
                MessageBox.Show("选中订单中有部分没有[完成总检],报告未出,不能预览和打印");
                return;
            }

            List<String> orderNumberList = new List<string>();
            foreach (DataGridViewRow row in AddCheckBoxToDataGridView.GetSelectedRows())
            {
                orderNumberList.Add(row.Cells["Cell_OrderNumber"].Value.ToString());
            }
            string orderNumbers = string.Join(",", orderNumberList.ToArray());

            if (PrintingApp.CurrentUserInfo.UserPrinterConfig == null)
            {
                MessageBox.Show("加载本地打印配置失败！");
                return;
            }
            string printerName = PrintingApp.CurrentUserInfo.UserPrinterConfig.A4Printer ?? PrintingApp.CurrentUserInfo.UserPrinterConfig.A5Printer;
            if (string.IsNullOrWhiteSpace((printerName)))
            {
                MessageBox.Show("请先维护打印机！");
                return;
            }

            string url = ConfigurationManager.AppSettings.Get("PrintingServiceServiceUrl");
            var printingService = ServiceFactory.GetPrintingService(url);

            var request = new GetReportDataRequest()
            {
                Username = PrintingApp.UserCredential.UserName,
                Password = PrintingApp.UserCredential.Password,
                OrderNumbers = orderNumbers
            };
            var response = printingService.GetReportData(request);

            List<string> successOrderNumbers = new List<string>();
            if (response.ResultType == ResultTypes.Ok)
            {
                foreach (var reportInfo in response.Reports)
                {
                    var row = AddCheckBoxToDataGridView.GetSelectedRows().First(r => r.Cells["Cell_OrderNumber"].Value.ToString() == reportInfo.OrderNumber);
                    Int32 reportTemplateId = int.Parse(row.Cells["Cell_ReportTemplateId"].Value.ToString());
                    reportInfo.ReportTemplateCode = PrintingApp.ReportTemplates.First(r => r.Id == reportTemplateId).Code;

                    if (PrintReport(printerName, reportInfo))
                        successOrderNumbers.Add(reportInfo.OrderNumber);
                }
            }

            //更新报告状态
            var updateOrdersStatusRequest = new UpdateOrdersStatusRequest()
            {
                Username = PrintingApp.UserCredential.UserName,
                Password = PrintingApp.UserCredential.Password,
                OrderTransitions = successOrderNumbers.Select(o => new OrderTransition()
                {
                    OrderNumber = o, CurrentStatus = OrdersStatus.FinishCheck, NewStatus = OrdersStatus.FinishPrint
                }).ToArray()
            };
            var updateOrdersStatusResponse = printingService.UpdateOrdersStatus(updateOrdersStatusRequest);
            if (updateOrdersStatusResponse.ResultType == ResultTypes.Ok)
            {
                foreach (var successOrderNumber in successOrderNumbers)
                {
                    var row =
                        dgv_orders.Rows.Cast<DataGridViewRow>()
                            .First(r => r.Cells["Cell_OrderNumber"].Value.ToString() == successOrderNumber);

                    row.Cells["Cell_OrderStatus"].Value = "报告已打印";
                }
            }
        }

        private static bool PrintReport(string printerName, ReportInfo reportInfo)
        {
            try
            {
                PrintingProxy printingProxy = new PrintingProxy();
                printingProxy.PrintReport(printerName, reportInfo);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error while printing report", ex);
                return false;
            }

        }
    }
}
