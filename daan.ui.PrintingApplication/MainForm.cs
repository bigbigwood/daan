using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using CCWin.SkinControl;
using CCWin.Win32.Const;
using daan.ui.PrintingApplication.Helper;
using daan.ui.PrintingApplication.PrintingImpl;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models;
using daan.webservice.PrintingSystem.Contract.Models.Order;
using daan.webservice.PrintingSystem.Contract.Models.Report;
using log4net;
using daan.ui.controls;

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
            Text = string.Format("{0} v{1}", ConstString.ApplicationTitle, PrintingApp.GetVersionManager().ApplicationVersion);
            BindQueryGroup();
            BindDataGrid();
        }

        private void BindDataGrid()
        {
            pagerControl1.OnPageChanged += new EventHandler(pagerControl1_OnPageChanged);
            pagerControl1.OnPageSizeValueInvalid += new EventHandler(pagerControl1_OnPageSizeValueInvalid);

            AddCheckBoxToDataGridView.dgv = dgv_orders;
            AddCheckBoxToDataGridView.AddFullSelect();
            AddCheckBoxToDataGridView.SelectAllCheckBoxChanged += new Action(OnSelectAllCheckBoxChanged);
            dgv_orders.AutoGenerateColumns = false;
        }

        private void EnableControls()
        {
            pagerControl1.Enabled = true;
            btnQueryOrder.Enabled = true;
            btnPrint.Enabled = true;
            btnPrinterSetting.Enabled = true;
        }

        private void DisableControls()
        {
            pagerControl1.Enabled = false;
            btnQueryOrder.Enabled = false;
            btnPrint.Enabled = false;
            btnPrinterSetting.Enabled = false;
        }

        private void BindQueryGroup()
        {
            var comboBoxDataSourceProvider = new ComboBoxDataSourceProvider();
            dropStatus.DataSource = comboBoxDataSourceProvider.GetOrderStatusDataSource();
            dropStatus.ValueMember = "EnumValue";
            dropStatus.DisplayMember = "EnumDisplayText";
            dropStatus.SelectedValue = (int)OrdersStatus.FinishCheck;

            dropReportStatus.DataSource = comboBoxDataSourceProvider.GetReportStatusDataSource();
            dropReportStatus.ValueMember = "EnumValue";
            dropReportStatus.DisplayMember = "EnumDisplayText";
            dropReportStatus.SelectedValue = (int)ReportStatus.Normal;
            dropReportStatus.Enabled = false;

            dropNumberType.DataSource = comboBoxDataSourceProvider.GetNumberTypeDataSource();
            dropNumberType.ValueMember = "EnumValue";
            dropNumberType.DisplayMember = "EnumDisplayText";
            dropNumberType.SelectedValue = 1;

            dropProvince.DataSource = comboBoxDataSourceProvider.GetProvinceDataSource();

            var labList = new List<LabInfo>() { new LabInfo() { Id = -1, Name = ConstString.ALL } };
            labList.AddRange(PrintingApp.LabAssociations);
            dropDictLab.DataSource = labList;
            dropDictLab.ValueMember = "Id";
            dropDictLab.DisplayMember = "Name";
            if (PrintingApp.CurrentUserInfo.DefaultLab != null && PrintingApp.CurrentUserInfo.DefaultLab.Id != 0)
                dropDictLab.SelectedValue = PrintingApp.CurrentUserInfo.DefaultLab.Id;

            BindOrganizationByLab((int)dropDictLab.SelectedValue);

            dpFrom.Value = DateTime.Now.AddDays(-7);
            dpTo.Value = DateTime.Now;

            cbx_ScanDatetimeEnabled.Checked = false;
            DisableScanDatetimeControls();
        }

        private void BindOrganizationByLab(Int32 labId)
        {
            Int32 customerType = 0;
            var mappingOrganizationList = new List<OrganizationInfo>();

            if (labId != -1) //单个lab
            {
                mappingOrganizationList = PrintingApp.OrganizationAssociations.FindAll(c => (c.LabId == labId && c.CustomerType == customerType && c.Active == true) || (c.IsPublic == true && c.Active == true));
            }
            else //全部
            {
                foreach (LabInfo dict in PrintingApp.LabAssociations)
                {
                    List<OrganizationInfo> labMappingOrganizationList = PrintingApp.OrganizationAssociations.FindAll(c => (c.LabId == dict.Id && c.CustomerType == customerType && c.Active == true) || (c.IsPublic == true && c.Active == true));
                    foreach (var organzation in labMappingOrganizationList)
                    {
                        if (!mappingOrganizationList.Contains(organzation))
                            mappingOrganizationList.Add(organzation);
                    }
                }
            }

            var organizationList = new List<OrganizationInfo>() { new OrganizationInfo() { Id = -1, Name = ConstString.ALL } };
            organizationList.AddRange(mappingOrganizationList);
            dropDictcustomer.DataSource = organizationList;
            dropDictcustomer.ValueMember = "Id";
            dropDictcustomer.DisplayMember = "Name";
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
            request.Keyword = tbxName.Text;
            request.Section = tbxSection.Text;
            
            if (dpScanFrom.Enabled)
                request.SamplingDateBegin = dpScanFrom.Value.ToString(ConstString.DateFormat);
            if (dpSTo.Enabled)
                request.SamplingDateEnd = dpSTo.Value.AddDays(1).ToString(ConstString.DateFormat);

            if ((int)dropNumberType.SelectedValue == 1)
                request.OrderNumber = tbxOrderNum.Text;
            else
                request.Barcode = tbxOrderNum.Text;

            request.Dictlabid = (dropDictLab.SelectedValue.ToString() != "-1") ? dropDictLab.SelectedValue.ToString() : null;
            request.Dictcustomerid = (dropDictcustomer.SelectedValue.ToString() != "-1") ? dropDictcustomer.SelectedValue.ToString() : null;
            request.OrderStatus = (dropStatus.SelectedValue.ToString() != "-1") ? dropStatus.SelectedValue.ToString() : null;
            request.ReportStatus = (dropReportStatus.SelectedValue.ToString() != "-1") ? dropReportStatus.SelectedValue.ToString() : null;
            request.Province = (dropProvince.SelectedValue.ToString() != ConstString.ALL) ? dropProvince.SelectedValue.ToString() : null;

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

        private void pagerControl1_OnPageSizeValueInvalid(object sender, EventArgs e)
        {
            var pagerControl = sender as PagerControl;
            if (pagerControl != null)
            {
                Int32 pagerSize = 0;
                String pagerSizeText = pagerControl.GetPageSizeText();
                int.TryParse(pagerSizeText, out pagerSize);

                if (pagerSize < pagerControl.MinPageSize || pagerSize > pagerControl.MaxPageSize)
                {
                    MessageBox.Show(string.Format(@"每页显示订单数量不能为""{0}""，它的范围是：{1} - {2}", pagerSizeText, pagerControl.MinPageSize, pagerControl.MaxPageSize));
                    //ShowMessage(string.Format(@"每页显示订单数量不能为""{0}""，它的范围是：{1} - {2}", pagerSizeText, pagerControl.MinPageSize, pagerControl.MaxPageSize));
                    return;
                }
            }
        }


        private void UpdateOrderStatusRows(string orderNumber, string status)
        {
            var row = dgv_orders.Rows.Cast<DataGridViewRow>().First(r => r.Cells["Cell_OrderNumber"].Value.ToString() == orderNumber);
            row.Cells["Cell_OrderStatus"].Value = status;
        }


        private void dropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dropLabComboBox = sender as SkinComboBox;
            var lab = dropLabComboBox.SelectedItem as LabInfo;
            BindOrganizationByLab(lab.Id);
        }

        public void ShowMessage(string message)
        {
            lbl_Message.Text = message;
        }

        private void cbx_ScanDatetimeEnabled_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox.Checked)
                EnableScanDatetimeControls();
            else
                DisableScanDatetimeControls();
        }

        private void EnableScanDatetimeControls()
        {
            dpScanFrom.Enabled = true;
            dpScanFrom.Format = DateTimePickerFormat.Custom;
            dpScanFrom.CustomFormat = ConstString.DateFormat;
            dpScanFrom.Value = DateTime.Now.AddDays(-7);

            dpSTo.Enabled = true;
            dpSTo.Format = DateTimePickerFormat.Custom;
            dpSTo.CustomFormat = ConstString.DateFormat;
            dpSTo.Value = DateTime.Now;
        }

        private void DisableScanDatetimeControls()
        {
            dpScanFrom.Enabled = false;
            dpScanFrom.Format = DateTimePickerFormat.Custom;
            dpScanFrom.CustomFormat = " ";

            dpSTo.Enabled = false;
            dpSTo.Format = DateTimePickerFormat.Custom;
            dpSTo.CustomFormat = " ";
        }

        private void OnSelectAllCheckBoxChanged()
        {
            ShowMessage(string.Format("当前选中了{0}份报告", AddCheckBoxToDataGridView.GetSelectedRows().Count));
        }

        private void ProcessSelectedCheckBoxForDatagridview(DataGridView datagridview)
        {
            if (datagridview != null && datagridview.SelectedRows != null)
            {
                var selectedRows = datagridview.SelectedRows.Cast<DataGridViewRow>();
                foreach (var dataGridViewRow in selectedRows)
                {
                    var checkboxCell = dataGridViewRow.Cells[0] as DataGridViewCheckBoxCell;
                    checkboxCell.Value = !(bool)checkboxCell.FormattedValue;
                }
            }

            ShowMessage(string.Format("当前选中了{0}份报告", AddCheckBoxToDataGridView.GetSelectedRows().Count));
        }

        private void dgv_orders_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var datagridview = sender as DataGridView;
            ProcessSelectedCheckBoxForDatagridview(datagridview);
        }

        private void dgv_orders_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var datagridview = sender as DataGridView;
            ProcessSelectedCheckBoxForDatagridview(datagridview);
        }

        private void dgv_orders_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置  
                    if (dgv_orders.Rows[e.RowIndex].Selected == false)
                    {
                        dgv_orders.ClearSelection();
                        dgv_orders.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格  
                    if (dgv_orders.SelectedRows.Count == 1)
                    {
                        dgv_orders.CurrentCell = dgv_orders.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //弹出操作菜单  
                    cms_orderGrid.Show(MousePosition.X, MousePosition.Y);
                }
            }  
        }

        private void menuItem_Copy_Click(object sender, EventArgs e)
        {
            if (dgv_orders.CurrentCell != null)
            {
                string sClipboadStr = dgv_orders.CurrentCell.Value.ToString();
                Clipboard.SetText(sClipboadStr);
            }
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
                var exceptionReportDataOrderNumbers = new List<string>();
                string selectOrderNumbers = string.Join(",", reportDtoList.Select(r => r.OrderNumber).ToArray());
                var printingService = ServiceFactory.GetPrintingService();
                var request = new GetReportDataRequest()
                {
                    Username = PrintingApp.UserCredential.UserName,
                    Password = PrintingApp.UserCredential.Password,
                    OrderNumbers = selectOrderNumbers
                };
                var response = printingService.GetReportData(request);
                if (response.ResultType == ResultTypes.Ok || response.ResultType == ResultTypes.PartiallyOk)
                {
                    var allSelectedOrderNumberArray = reportDtoList.Select(r => r.OrderNumber).ToArray();
                    var successGetReportOrderNumberArray = response.Reports.Select(r => r.OrderNumber).ToArray();
                    exceptionReportDataOrderNumbers = allSelectedOrderNumberArray.Except(successGetReportOrderNumberArray).ToList();
                }
                else
                {
                    Invoke(new Action(() =>
                    {
                        EnableControls();
                        MessageBox.Show("获取报告数据出错！");
                    }));
                    return;
                }
                BeginInvoke(new Action(() => extendProgressBar.ReportProgress(getReportDataProgressBarWeight)));

                var finishPrintOrderNumbers = new List<string>();
                var exceptionPrintOrderNumbers = new List<string>();
                var reports = response.Reports;
                int index = 0;
                int count = reports.Count();

                foreach (var reportInfo in reports)
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

                    StringBuilder errorMessage = new StringBuilder();
                    if (exceptionReportDataOrderNumbers.Any())
                    {
                        errorMessage.Append(string.Format("本次获取报告数据有部分异常，体检单号为： {0}", string.Join(",", exceptionReportDataOrderNumbers.ToArray())));
                    }
                    if (exceptionPrintOrderNumbers.Any())
                    {
                        errorMessage.Append(string.Format("本次打印有部分异常，体检单号为： {0}", string.Join(",", exceptionPrintOrderNumbers.ToArray())));
                    }

                    if (string.IsNullOrWhiteSpace(errorMessage.ToString()))
                        MessageBox.Show("打印完成！");
                    else
                        MessageBox.Show(errorMessage.ToString());
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

        private void btnPrinterSetting_Click(object sender, EventArgs e)
        {
            var form = new PrinterSettingForm();
            form.ShowDialog();
        }

    }
}
