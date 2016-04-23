using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.dict;
using System.Collections;
using daan.service.order;
using daan.util.Web;
using daan.web.code;
using FastReport;
using System.Data;
using daan.service.common;
using daan.service.login;
using daan.domain;
using ExtAspNet;
using daan.util.Common;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Configuration;
using daan.service.proceed;
using System.Text;

namespace daan.web.admin.analyse
{
    public partial class AnaFocusPrint : PageBase
    {
        #region >>>字段
        LoginService loginservice = new LoginService();
        CommonReport commonReport = new CommonReport();
        OrdersService ordersService = new OrdersService();
        DictCustomerService dictCustomerService = new DictCustomerService();
        /// <summary>
        /// 查询参数缓存,查询条件改变时，必须clear里面的东西
        /// </summary>
        static Hashtable _parameterCache = new Hashtable();
        #endregion

        #region >>>页面初始化及查询
        protected void Page_Load(object sender, EventArgs e)
        {
            ExtAspNet.PageContext.RegisterStartupScript("(Ext.getCmp('" + dropDictcustomer.ClientID + "')).listWidth=250;");
            if (!IsPostBack)
            {
                InitPageData();
            }
        }
        // 初始化下拉列表的数据
        void BindDrop()
        {
            //分点
            DDLDictLabBinder(dropDictLab, true);
            dropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            //体检单位初始化
            BinddropDictcustomer(dropDictLab.SelectedValue);
            //订单状态
            DDLInitbasicBinder(dropStatus, "ORDERSTATUS");
            dropStatus.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            //报告状态
            DDLInitbasicBinder(dropReportStatus, "REPORTSTATUS");
            dropReportStatus.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            dropReportStatus.SelectedValue = "0";
        }

        // 初始化体检单位
        void BinddropDictcustomer(string dictlabid)
        {
            DropDictcustomerBinder(dropDictcustomer, dictlabid, true);
        }

        // 取得查询参数
        Hashtable GetParm()
        {
            //过滤
            string ordernum = tbxOrderNum.Text = TextUtility.ReplaceText(tbxOrderNum.Text.Trim());

            PageUtil pageUtil = new PageUtil(gdOrders.PageIndex, gdOrders.PageSize);
            Hashtable _parameterCache = new Hashtable();
            _parameterCache.Add("pageStart", pageUtil.GetPageStartNum());
            _parameterCache.Add("pageEnd", pageUtil.GetPageEndNum());
            _parameterCache.Add("ordernum", ordernum);
            //有体检号时 忽略其他条件
            if (ordernum != string.Empty) { return _parameterCache; }
            //分点
            switch (dropDictLab.SelectedText)
            {
                case "全部":
                    {
                        if (dropDictLab.SelectedValue == "-1")
                        {
                            _parameterCache.Add("dictlabid", Userinfo.joinLabidstr);
                        }
                    };
                    break;
                default:
                    {
                        _parameterCache.Add("dictlabid", dropDictLab.SelectedValue);
                    };
                    break;
            }
            //体检单位
            _parameterCache.Add("dictcustomerid", dropDictcustomer.SelectedValue == "-1" ? null : dropDictcustomer.SelectedValue);

            _parameterCache.Add("StartDate", dpFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            _parameterCache.Add("EndDate", dpTo.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
            _parameterCache.Add("SDateBegin", dpSFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            _parameterCache.Add("SDateEnd", dpSTo.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
            _parameterCache.Add("status", dropStatus.SelectedValue == "-1" ? null : dropStatus.SelectedValue);
            _parameterCache.Add("name", TextUtility.ReplaceText(tbxName.Text));
            _parameterCache.Add("reportstatus", this.dropReportStatus.SelectedValue == "-1" ? null : this.dropReportStatus.SelectedValue);
            return _parameterCache;
        }

        //初始化页面的数据
        void InitPageData()
        {
            _parameterCache.Clear();
            dpFrom.SelectedDate = DateTime.Now.AddDays(-7);
            dpTo.SelectedDate = DateTime.Now;
            dpSFrom.SelectedDate = DateTime.Now.AddDays(-7);
            dpSTo.SelectedDate = DateTime.Now;
            BindDrop();
        }

        // 绑定订单主表的数据
        void BindgdOrders()
        {
            Hashtable _parameterCache = GetParm();
            gdOrders.RecordCount = ordersService.DataForFocusPrintPageTotal(_parameterCache);
            gdOrders.DataSource = ordersService.DataForFocusPrintPageLst(_parameterCache);
            gdOrders.DataBind();
        }


        //是否检查作废
        private string GetSelectOrderNums()
        {
            int[] strSelect = gdOrders.SelectedRowIndexArray;
            if (strSelect.Length <= 0)
            {
                MessageBoxShow("您还没有勾选记录!");
                return "";
            }
            string str = string.Empty;
            for (int i = 0; i < strSelect.Length; i++)
            {
                str += gdOrders.DataKeys[strSelect[i]][0].ToString() + ",";

                //判断是否总检完成
                if (CheckIsFinishCheck(gdOrders.DataKeys[strSelect[i]][1]))
                {
                    MessageBoxShow("选中订单中有部分没有[完成总检],报告未出,不能预览和打印"); return "";
                }
            }
            return str.TrimEnd(',');
        }
        private bool CheckIsFinishCheck(object str)
        {
            return Convert.ToInt32(str) < (int)daan.service.common.ParamStatus.OrdersStatus.FinishCheck;
        }
        #endregion

        #region 页面事件

        /// <summary>
        /// 体检单位改变响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BinddropDictcustomer(dropDictLab.SelectedValue);
        }

        /// <summary>
        /// 搜索事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if ((dropDictLab.SelectedValue.Length == 0) && (dropDictcustomer.SelectedValue.Length == 0))
            {
                MessageBoxShow("分点和体检单位必须选择!");

                return;
            }
            if (this.dpFrom.Text != "" && this.dpTo.Text != "")
            {
                if (this.dpFrom.SelectedDate <= this.dpTo.SelectedDate)
                {
                    BindgdOrders();
                }
                else
                {

                    MessageBoxShow("结束时间应大于开始时间！", MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
            }

        }

        /// <summary>
        /// 打印报告单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            SetInitlocalsetting(hdMac.Text);//取mac地址   
            string ordernums = GetSelectOrderNums();
            if (ordernums == string.Empty) { return; }

            //旧版打印1.0
            //Report report = new Report();
            //foreach (int row in gdOrders.SelectedRowIndexArray)
            //{
            //    string ordernum = gdOrders.DataKeys[row][0].ToString();
            //    //根据体检号得到报告单模板，从数据库读取报告数据。
            //    report = commonReport.GetReport(ordernum, 1);
            //    #region 修改打印状态、添加操作日志
            //    Hashtable ht = new Hashtable();
            //    ht.Add("ordernum", ordernum);
            //    ht.Add("oldstatus", (int)ParamStatus.OrdersStatus.FinishCheck);
            //    ht.Add("status", (int)ParamStatus.OrdersStatus.FinishPrint);
            //    ordersService.EditStatusByOldStatus(ht);
            //    ordersService.AddOperationLog(ordernum, null, "报告单集中打印", "打印报告单", "修改留痕", "");
            //    #endregion
            //    //序列化报告、报告数据，获取打印机
            //    commonReport.PrintReport2(report.SaveToString(), commonReport.dsGetReportData, Userinfo);
            //    ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintReport(\'{0}\',\'{1}\',\'{2}\');", CommonReport.printer, CommonReport.json, CommonReport.dsjson));
            //}

            //打印2.0
            foreach (int row in gdOrders.SelectedRowIndexArray)
            {
                string ordernum = gdOrders.DataKeys[row][0].ToString();
                #region 修改打印状态、添加操作日志
                Hashtable ht = new Hashtable();
                ht.Add("ordernum", ordernum);
                ht.Add("oldstatus", (int)ParamStatus.OrdersStatus.FinishCheck);
                ht.Add("status", (int)ParamStatus.OrdersStatus.FinishPrint);
                ordersService.EditStatusByOldStatus(ht);
                ordersService.AddOperationLog(ordernum, null, "报告单集中打印", "打印报告单", "修改留痕", "");
                #endregion
                Hashtable htPrint = commonReport.PrintReport3(ordernum, Userinfo, 1);
                ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintReport(\'{0}\',\'{1}\',\'{2}\');", htPrint["printer"].ToString(), htPrint["json"].ToString(), htPrint["dsjson"].ToString()));
            }
        }

        /// <summary>
        /// （新）打印报告单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrintNew_Click(object sender, EventArgs e)
        {
            string mac = hdMac.Text.ToString();
            if (string.IsNullOrEmpty(mac))
            {
                MessageBoxShow("获取MAC失败！");
                return;
            }
            SetInitlocalsetting(mac);//取mac地址   
            string printName = string.Empty;
            if (Userinfo.initlocalsetting == null) 
            {
                MessageBoxShow("加载本地打印配置失败");
                return; 
            }
            else
            {
                Initlocalsetting initlocalsetting = Userinfo.initlocalsetting;
                printName = initlocalsetting.A4printer ?? initlocalsetting.A5printer;
                if (string.IsNullOrEmpty(printName))
                {
                    MessageBoxShow("请先维护打印机！"); return;
                }
            }
            string ordernums = GetSelectOrderNums();
            if (ordernums == string.Empty) { return; }
            //打印3.0
            foreach (int row in gdOrders.SelectedRowIndexArray)
            {
                string ordernum = gdOrders.DataKeys[row][0].ToString();
                string dictreporttemplateid = gdOrders.DataKeys[row][4].ToString();
                Hashtable htprint = commonReport.getPrintData(ordernum, dictreporttemplateid);
                string repCode = htprint["repCode"].ToString();
                string dsjson = htprint["dsjson"].ToString();
                #region 修改打印状态、添加操作日志
                Hashtable ht = new Hashtable();
                ht.Add("ordernum", ordernum);
                ht.Add("oldstatus", (int)ParamStatus.OrdersStatus.FinishCheck);
                ht.Add("status", (int)ParamStatus.OrdersStatus.FinishPrint);
                ordersService.EditStatusByOldStatus(ht);
                ordersService.AddOperationLog(ordernum, null, "报告单集中打印", "新版打印报告单", "修改留痕", "");
                #endregion
                ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintReport2(\'{0}\',\'{1}\',\'{2}\');", printName, repCode, dsjson));
            }
        }

        /// <summary>
        /// 预览报告单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length != 1)
            {
                MessageBoxShow("请选择一项进行预览！"); return;
            }

            string reportstatus = dropReportStatus.SelectedValue;
            string str = string.Empty;
            string ordernum=gdOrders.DataKeys[gdOrders.SelectedRowIndexArray[0]][0].ToString();
            if (reportstatus == "1" || reportstatus == "2")//异常报告预览
            {
                string rt = reportstatus == "1" ? "迟发" : "退单";
                WinReportView.Title = rt+"报告预览";
                //判断是否有异常报告记录
                Hashtable ht = new Hashtable();
                ht.Add("ordernum", ordernum);
                ht.Add("reportstatus", reportstatus);
                if (ordersService.GetOrderExceptionReport(ht) == 0)
                {
                    MessageBoxShow(string.Format("选中订单未查询到标本{0}记录，不能预览{0}报告", rt));
                    return;
                }
                str = "AnaFocusPrintDetail.aspx?reportType=" + reportstatus + "&order_num=" + ordernum;
            }
            else//正常报告预览
            {
                //判断是否总检完成
                if (CheckIsFinishCheck(gdOrders.DataKeys[gdOrders.SelectedRowIndexArray[0]][1]))
                {
                    MessageBoxShow("选中订单没有[完成总检],不能预览"); return;
                }
                str = "../report/RepShowView.aspx?reportType=1&order_num=" + ordernum;
            }
            PageContext.RegisterStartupScript(WinReportView.GetShowReference(str));
        }


        ///   <summary>
        ///  把给定的文件流转换为二进制字节数组。
        ///   </summary>
        ///   <returns> </returns>
        public static byte[] ConvertStreamToByteBuffer(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
            int b1;
            System.IO.MemoryStream tempStream = new System.IO.MemoryStream();
            while ((b1 = fs.ReadByte()) != -1)
            {
                tempStream.WriteByte(((byte)b1));
            }
            fs.Close();
            fs.Dispose();
            return tempStream.ToArray();
        }

        //分页
        protected void gdOrders_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gdOrders.PageIndex = e.NewPageIndex;
            BindgdOrders();
        }

        //导出
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Hashtable ht = GetParm();
            DataTable dt = ordersService.DataForFocusPrintExport(ht);
            if (dt.Rows.Count > 0)
            {
                String sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                String filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                ExcelOperation<DataTable>.ExportDataTableToExcel(dt, filename, sheetname);
            }
            else
            {
                MessageBoxShow("没有需要导出的数据！", MessageBoxIcon.Information);
            }
        }
        #endregion



        #region >>>10.日志查询
        protected void btnLog_Click(object sender, EventArgs e)
        {
            if (gdOrders.Rows.Count <= 0 || gdOrders.SelectedRowIndexArray.Length <= 0)
            {
                MessageBoxShow("请选择一项进行操作记录查询！");
                return;
            }
            object[] objValue = gdOrders.DataKeys[gdOrders.SelectedRowIndexArray[0]];
            string orderNum = TypeParse.ObjToStr(objValue[0], "");
            WinBillRemark.Hidden = false;
            WinBillRemark.IFrameUrl = "../bill/BillOperationLog.aspx?ordernum=" + orderNum;
            WinBillRemark.Title = "订单日志查询";
        }
        #endregion

        #region >>>打包下载报告
        /// <summary>
        /// 从报告上传社区扫描程序目录下获取已生成的PDF文件，转移至等创建的待压缩目录下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            List<string> ordernums = new List<string>();//存放身份证_体检号_姓名的PDF命名
            List<string> idnumbers = new List<string>();//存放体检号_姓名的PDF命名
            DataTable dt = new DataTable();
            //StringBuilder strExists = new StringBuilder();//不存在的文件清单
            string pdfFile = ConfigurationManager.AppSettings["PdfFile"].ToString();//获取需要下载的报告路径，此路径为报告上传社区扫描程序的路径                  

            #region >>>>获取选中行的订单号并判断是否已完成总检
            foreach (int row in gdOrders.SelectedRowIndexArray)
            {
                Orders order = new ProRegisterService().SelectOrderInfo(gdOrders.DataKeys[row][0].ToString());
                if (order.Status == "25" || order.Status == "30")
                {
                    if (gdOrders.DataKeys[row][3] == null)
                    {
                        idnumbers.Add("");
                    }
                    else
                    {
                        idnumbers.Add(string.Format("{0}_{1}_{2}.pdf", gdOrders.DataKeys[row][3].ToString(), gdOrders.DataKeys[row][0].ToString(), gdOrders.DataKeys[row][2].ToString()));
                    }
                    ordernums.Add(string.Format("{0}_{1}.pdf", gdOrders.DataKeys[row][0].ToString(), gdOrders.DataKeys[row][2].ToString()));
                }
            }
            if (ordernums.Count() <= 0)
            {
                MessageBoxShow("请至少选择一项状态为完成总检或报告已打印的记录!");
                return;
            }
            #endregion

            #region >>>>  检查目录是否存在，如不存在则创建,存在则删除该文件夹所有文件并重新创建

            string foldername = string.Format("{0}_{1}", Userinfo.userName, DateTime.Now.ToString("yyyyMMddHHmmssfff"));//存放PDF待转移压缩目标文件夹的名字
            string fullpath = string.Format("{0}{1}\\", Server.MapPath("../../upload/Pdffile/"), foldername);//完整文件夹名      

            FileInfo fileInfo = new FileInfo(fullpath);
            if (fileInfo.Directory.Exists == false)
            {
                //如果目录不存在，则创建该目录
                try { fileInfo.Directory.Create(); }
                catch (Exception ex)
                {
                    MessageBoxShow("创建文件夹出错,错误信息：" + ex.Message);
                    return;
                }
            }
            else
            {
                //删除整个文件夹之后创建文件夹
                try { WebUtils.DeleteFolder(fullpath); fileInfo.Directory.Create(); }
                catch (Exception ex)
                {
                    MessageBoxShow("创建文件夹出错,错误信息：" + ex.Message);
                    return;
                }
            }
            #endregion

            #region >>>>  复制报告到指定文件夹
            for (int i = 0; i < ordernums.Count; i++)
            {
                string fileIdNumberName = idnumbers[i].ToString();
                string fileOrderNumName = ordernums[i].ToString();
                try
                {
                    //复制pdf报告，先验证身份证命名的文件是否存在，不存在则按体检号命名，都不存在时提示用户
                    if (File.Exists(pdfFile + fileIdNumberName))
                    {
                        File.Copy(pdfFile + fileIdNumberName, fullpath + fileIdNumberName, true);
                    }
                    else
                    {
                        if (File.Exists(pdfFile + fileOrderNumName))
                        {
                            File.Copy(pdfFile + fileOrderNumName, fullpath + fileOrderNumName, true);
                        }
                        //else
                        //{
                        //    strExists.AppendFormat("{0},", fileOrderNumName);
                        //}
                    }
                }
                catch (Exception)
                {
                    MessageBoxShow(pdfFile + fileOrderNumName + "*" + fullpath + fileOrderNumName + "报告可能不存在，请选择总检完的报告！");
                    return;
                }
            }
            #endregion

            //if (strExists.ToString().TrimStart(',').TrimEnd(',').Length > 0)
            //{
            //    MessageBoxShow(string.Format("以下报告不存在，请用PDF虚拟打印机单个下载：{0}", strExists.ToString()));
            //}


            #region >>>>  压缩 供用户下载
            string workpath = fullpath;
            string path = fullpath;

            string rarfullname = string.Format("{0}.rar", foldername);//存放文件的文件夹名字            
            if (File.Exists(workpath + rarfullname)) File.Delete(workpath + rarfullname);
            String the_rar;
            RegistryKey the_Reg;
            Object the_Obj;
            String the_Info;
            ProcessStartInfo the_StartInfo;
            Process the_Process;
            the_Reg = Registry.ClassesRoot.OpenSubKey("WinRAR\\Shell\\Open\\Command");
            the_Obj = the_Reg.GetValue("");
            the_rar = the_Obj.ToString();
            the_Reg.Close();
            the_rar = the_rar.Substring(1, the_rar.Length - 7);
            the_Info = " a -ep " + rarfullname + "  " + path;//压缩命令可参见rar帮助文档
            the_StartInfo = new ProcessStartInfo();
            the_StartInfo.FileName = the_rar;
            the_StartInfo.Arguments = the_Info;
            the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            the_StartInfo.WorkingDirectory = workpath;//获取或设置要启动的进程的初始目录。


            the_Process = new Process();
            the_Process.StartInfo = the_StartInfo;
            the_Process.Start();
            the_Process.WaitForExit(100000);
            the_Process.Close();



            if (!File.Exists(workpath + rarfullname))
            {
                MessageBoxShow("报告压缩文件不存在");
                return;
            }
            else
            {
                Response.Redirect(string.Format("../../upload/Pdffile/{0}/{1}.rar", foldername, foldername));
            }
            #endregion
        }
        #endregion

    }
}