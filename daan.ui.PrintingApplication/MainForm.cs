using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using CCWin.Win32.Const;
using daan.ui.PrintingApplication.Control;
using daan.ui.PrintingApplication.Helper;
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
        private readonly List<string> AllowPrintStatusList = new List<string>() { ConstString.OrdersStatus_FinishPrint, ConstString.OrdersStatus_FinishCheck };

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void MainFormTabImpl_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            lbl_Version.Text = PrintingApp.CurrentApplicationVersion;
            BindQueryGroup();
            BindDataGrid();
        }

        private void BindDataGrid()
        {
            pagerControl1.OnPageChanged += new EventHandler(pagerControl1_OnPageChanged);

            AddCheckBoxToDataGridView.dgv = dgv_orders;
            AddCheckBoxToDataGridView.AddFullSelect();
            dgv_orders.AutoGenerateColumns = false;
        }

        private void EnableControls()
        {
            pagerControl1.Enabled = true;
            btnQueryOrder.Enabled = true;
            btnPrint.Enabled = true;
        }

        private void DisableControls()
        {
            pagerControl1.Enabled = false;
            btnQueryOrder.Enabled = false;
            btnPrint.Enabled = false;
        }

        private void BindQueryGroup()
        {
            var comboBoxDataSourceProvider = new ComboBoxDataSourceProvider();
            dropStatus.DataSource = comboBoxDataSourceProvider.GetOrderStatusDataSource();
            dropStatus.ValueMember = "EnumValue";
            dropStatus.DisplayMember = "EnumDisplayText";
            dropStatus.SelectedValue = -1;

            dropReportStatus.DataSource = comboBoxDataSourceProvider.GetReportStatusDataSource();
            dropReportStatus.ValueMember = "EnumValue";
            dropReportStatus.DisplayMember = "EnumDisplayText";
            dropReportStatus.SelectedValue = (int)ReportStatus.Normal;
            dropReportStatus.Enabled = false;

            dropNumberType.DataSource = comboBoxDataSourceProvider.GetNumberTypeDataSource();
            dropNumberType.ValueMember = "EnumValue";
            dropNumberType.DisplayMember = "EnumDisplayText";
            dropNumberType.SelectedValue = 1;

            var labList = new List<LabInfo>() { new LabInfo() { Id = -1, Name = ConstString.ALL } };
            labList.AddRange(PrintingApp.LabAssociations);
            dropDictLab.DataSource = labList;
            dropDictLab.ValueMember = "Id";
            dropDictLab.DisplayMember = "Name";
            if (PrintingApp.CurrentUserInfo.DefaultLab != null && PrintingApp.CurrentUserInfo.DefaultLab.Id != 0)
                dropDictLab.SelectedValue = PrintingApp.CurrentUserInfo.DefaultLab.Id;

            var organizationList = new List<OrganizationInfo>() { new OrganizationInfo() { Id = -1, Name = ConstString.ALL } };
            organizationList.AddRange(PrintingApp.OrganizationAssociations);
            dropDictcustomer.DataSource = organizationList;
            dropDictcustomer.ValueMember = "Id";
            dropDictcustomer.DisplayMember = "Name";

            dpFrom.Value = DateTime.Now.AddDays(-7);
            dpTo.Value = DateTime.Now;
            dpSFrom.Value = DateTime.Now.AddDays(-7);
            dpSTo.Value = DateTime.Now;

            ////test
            //dropStatus.SelectedValue = (int)OrdersStatus.FinishPrint;
            //dpFrom.Value = DateTime.Now.AddDays(-60);
        }

        private void PresentData(QueryOrdersResponse response)
        {
            AddCheckBoxToDataGridView.Refresh();
            dgv_orders.DataSource = response.Result;
            pagerControl1.DrawControl(response.OrderCount);
        }

        private QueryOrdersRequest GetQueryOrdersRequest()
        {
            var request = new QueryOrdersRequest();
            request.Username = PrintingApp.UserCredential.UserName;
            request.Password = PrintingApp.UserCredential.Password;
            request.PageStart = ((pagerControl1.PageIndex - 1) * pagerControl1.PageSize + 1).ToString();
            request.PageEnd = ((pagerControl1.PageIndex - 1) * pagerControl1.PageSize + pagerControl1.PageSize).ToString();
            request.StartDate = dpFrom.Value.ToString(ConstString.DateFormat);
            request.EndDate = dpTo.Value.AddDays(1).ToString(ConstString.DateFormat);
            request.SamplingDateBegin = dpSFrom.Value.ToString(ConstString.DateFormat);
            request.SamplingDateEnd = dpSTo.Value.AddDays(1).ToString(ConstString.DateFormat);
            request.Keyword = tbxName.Text;

            if ((int) dropNumberType.SelectedValue == 1)
                request.OrderNumber = tbxOrderNum.Text;
            else
                request.Barcode = tbxOrderNum.Text;

            request.Dictlabid = dropDictLab.SelectedValue.ToString();
            request.Dictcustomerid = (dropDictcustomer.SelectedValue.ToString() != "-1") ? dropDictcustomer.SelectedValue.ToString() : null;
            request.OrderStatus = (dropStatus.SelectedValue.ToString() != "-1") ? dropStatus.SelectedValue.ToString() : null;
            request.ReportStatus = (dropReportStatus.SelectedValue.ToString() != "-1") ? dropReportStatus.SelectedValue.ToString() : null;

            return request;
        }

        private void btnQueryOrder_Click(object sender, EventArgs e)
        {
            Log.Info("Start querying order...");

            var request = GetQueryOrdersRequest();
            Thread backgroudWorkerThread = new Thread(new ParameterizedThreadStart(backgroudWorker_QueryOrders));
            backgroudWorkerThread.Start(request);
        }

        void pagerControl1_OnPageChanged(object sender, EventArgs e)
        {
            btnQueryOrder_Click(sender, e);
        }

        private void UpdateOrderStatusRows(string orderNumber, string status)
        {
            var row = dgv_orders.Rows.Cast<DataGridViewRow>().First(r => r.Cells["Cell_OrderNumber"].Value.ToString() == orderNumber);
            row.Cells["Cell_OrderStatus"].Value = status;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("确定打印吗？", "确定打印吗？", MessageBoxButtons.YesNo);
                if (dialogResult == System.Windows.Forms.DialogResult.No)
                    return;

                if (AddCheckBoxToDataGridView.GetSelectedRows().Count == 0)
                {
                    MessageBox.Show("请先选择订单");
                    return;
                }

                //Item1 = OrderNumber, Item2 = Cell_OrderStatus, Item3 = Cell_ReportTemplateId
                List<Tuple<string, string, string>> orderDtoList = AddCheckBoxToDataGridView.GetSelectedRows()
                    .Select(r => Tuple.Create(
                        r.Cells["Cell_OrderNumber"].Value.ToString(),
                        r.Cells["Cell_OrderStatus"].Value.ToString(),
                        r.Cells["Cell_ReportTemplateId"].Value.ToString()
                        )).ToList();

                var wrongStatusOrderDtos = orderDtoList.FindAll(o => AllowPrintStatusList.Contains(o.Item2) == false);
                if (wrongStatusOrderDtos.Any())
                {
                    MessageBox.Show(string.Format("选中订单号为:{0},没有[完成总检],报告未出,不能预览和打印", string.Join(",", wrongStatusOrderDtos.Select(o => o.Item1).ToArray())));
                    return;
                }

                var wrongTemplateIdOrderDtos = orderDtoList.FindAll(o => PrintingApp.ReportTemplates.Exists(r => r.Id == int.Parse(o.Item3)) == false);
                if (wrongTemplateIdOrderDtos.Any())
                {
                    MessageBox.Show(string.Format("选中订单号为:{0}, 不能找到报告模板文件", string.Join(",", wrongTemplateIdOrderDtos.Select(o => o.Item1).ToArray())));
                    return;
                }

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

                var reportInfoDtos = new List<ReportInfo>();
                orderDtoList.ForEach(o =>
                {
                    Int32 reportTemplateId = int.Parse(o.Item3);
                    var reportTemplate = PrintingApp.ReportTemplates.FirstOrDefault(r => r.Id == reportTemplateId);
                    if (reportTemplate != null)
                    {
                        reportInfoDtos.Add(new ReportInfo() { OrderNumber = o.Item1, ReportTemplateCode = reportTemplate.Code });
                    }
                });

                Thread backgroudWorkerThread = new Thread(new ParameterizedThreadStart(backgroudWorker_PrintReports));
                backgroudWorkerThread.Start(reportInfoDtos);
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing print button.", ex);
            }
        }

        private void backgroudWorker_QueryOrders(object obj)
        {
            try
            {
                var request = obj as QueryOrdersRequest;
                BeginInvoke(new Action(DisableControls));

                var printingService = ServiceFactory.GetPrintingService();
                var response = printingService.QueryOrders(request);


                Invoke(new Action(() =>
                {
                    EnableControls();
                    PresentData(response);
                }));
                Log.Info("Finish querying order...");
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing query orders.", ex);

                Invoke(new Action(() =>
                {
                    EnableControls();
                    MessageBox.Show("查询订单异常！");
                }));
            }
        }

        private void backgroudWorker_PrintReports(object obj)
        {
            try
            {
                const int getReportDataProgressBarWeight = 30;
                const int printReportProgressBarWeight = 60;
                const int finishUpdateStatusProgressBarWeight = 10;

                string printerName = PrintingApp.CurrentUserInfo.UserPrinterConfig.A4Printer ??
                                     PrintingApp.CurrentUserInfo.UserPrinterConfig.A5Printer;
                List<ReportInfo> reportDtoList = obj as List<ReportInfo>;
                BeginInvoke(new Action(() => extendProgressBar.ReportProgress(0)));
                BeginInvoke(new Action(DisableControls));

                Log.InfoFormat("Start printing report, the count of report is {0}...", reportDtoList.Count);
                string selectOrderNumbers = string.Join(",", reportDtoList.Select(r => r.OrderNumber).ToArray());
                var printingService = ServiceFactory.GetPrintingService();
                var request = new GetReportDataRequest()
                {
                    Username = PrintingApp.UserCredential.UserName,
                    Password = PrintingApp.UserCredential.Password,
                    OrderNumbers = selectOrderNumbers
                };
                var response = printingService.GetReportData(request);
                if (response.ResultType != ResultTypes.Ok)
                {
                    Invoke(new Action(() =>
                    {
                        EnableControls();
                        MessageBox.Show("不能获取到所有选中订单的报告数据！");
                    }));
                    return;
                }
                BeginInvoke(new Action(() => extendProgressBar.ReportProgress(getReportDataProgressBarWeight)));

                var finishPrintOrderNumbers = new List<string>();
                var exceptionPrintOrderNumbers = new List<string>();
                int index = 0;
                int count = response.Reports.Count();
                while (index < count)
                {
                    foreach (var reportInfo in response.Reports)
                    {
                        index++;
                        reportInfo.ReportTemplateCode = reportDtoList.First(r => r.OrderNumber == reportInfo.OrderNumber).ReportTemplateCode;

                        if (PrintReport(printerName, reportInfo))
                            finishPrintOrderNumbers.Add(reportInfo.OrderNumber);
                        else
                            exceptionPrintOrderNumbers.Add(reportInfo.OrderNumber);

                        int calcWeight = (int)((double)index / count * printReportProgressBarWeight + getReportDataProgressBarWeight);
                        BeginInvoke(new Action(() => extendProgressBar.ReportProgress(calcWeight)));
                    }
                }

                //更新报告状态
                var updateOrdersStatusRequest = new UpdateOrdersStatusRequest()
                {
                    Username = PrintingApp.UserCredential.UserName,
                    Password = PrintingApp.UserCredential.Password,
                    OrderTransitions = finishPrintOrderNumbers.Select(o => new OrderTransition()
                    {
                        OrderNumber = o,
                        CurrentStatus = OrdersStatus.FinishCheck,
                        NewStatus = OrdersStatus.FinishPrint
                    }).ToArray()
                };
                var updateOrdersStatusResponse = printingService.UpdateOrdersStatus(updateOrdersStatusRequest);
                if (updateOrdersStatusResponse.ResultType == ResultTypes.Ok)
                {
                    Invoke(new Action(() => finishPrintOrderNumbers.ForEach(o => UpdateOrderStatusRows(o, ConstString.OrdersStatus_FinishPrint))));
                }

                Invoke(new Action(() =>
                {
                    extendProgressBar.ReportProgress(100);
                    EnableControls();
                    if (exceptionPrintOrderNumbers.Any())
                    {
                        string errorMessage = string.Format("本次打印有部分异常，体检单号为： {0}", string.Join(",", exceptionPrintOrderNumbers.ToArray()));
                        MessageBox.Show(errorMessage);
                    }
                    else
                        MessageBox.Show("打印完成！");
                }));

                Log.Info("Finish printing report...");
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing print reports.", ex);

                Invoke(new Action(() =>
                {
                    EnableControls();
                    MessageBox.Show("打印异常！");
                }));
            }
        }


        /// <summary>
        /// Print report function
        /// </summary>
        /// <param name="printerName"></param>
        /// <param name="reportInfo"></param>
        /// <returns></returns>
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
                Log.ErrorFormat("Error while printing report (order number = {0}, report code = {1}).", reportInfo.OrderNumber, reportInfo.ReportTemplateCode);
                Log.Error(ex);
                return false;
            }
        }
    }
}
