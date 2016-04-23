using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using daan.service.order;
using System.Collections;
using FastReport;
using daan.web.code;
using System.Configuration;
using FastReport.Export.Pdf;


namespace daan.ui.main
{
    public partial class FrmUploadShequ88 : Form
    {
        bool b;
        readonly OrdersService orderservice = new OrdersService();

        private readonly System.Timers.Timer timer = new System.Timers.Timer();

        public FrmUploadShequ88()
        {
            InitializeComponent();
            tbTime.Text = "10";
            ContextMenu m = new ContextMenu();
            tbTime.ContextMenu = m;
            timer.Elapsed += timer_Elapsed;
            //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            timer.AutoReset = false; 
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>开始计时
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string strOrderNum="";//条码号

            if (b)
            {
                return;
            }
            //设置timer不可用
            timer.Stop();//
            //传输数据 
            try
            {
                #region
                DataTable dt = orderservice.GetSelectOrdersByStatus();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //生成pdf
                        strOrderNum =dt.Rows[i]["barcode"].ToString();
                        string strpath = ConfigurationManager.AppSettings["path"];
                        using (Report report = new CommonReport().GetReport(strOrderNum, strpath))
                        {
                            //生成PDF文件保存在PdfFile文件夹内,以时间命名
                            string FilePdfPath = Application.StartupPath + "\\PdfFile\\";
                            string randomName = "";
                            string idnumber = dt.Rows[i]["idnumber"].ToString().Trim();
                            string ordernum = strOrderNum;
                            string realname = dt.Rows[i]["realname"].ToString();
                            if (string.IsNullOrEmpty(idnumber))
                            {
                                randomName = String.Format("{0}_{1}.pdf", ordernum, realname);
                            }
                            else
                            {
                                randomName = string.Format("{0}_{1}_{2}.pdf", idnumber, ordernum, realname);
                            }
                            string pdfPath = FilePdfPath + randomName;
                            if (!Directory.Exists(FilePdfPath))
                            {
                                Directory.CreateDirectory(FilePdfPath); //若文件夹不存在则新建文件夹
                            }
                            PDFExport tyt = new PDFExport() { Compressed = true, RichTextQuality = 50, EmbeddingFonts = false };
                            report.Export(tyt, pdfPath);
                        }

                        //修改状态
                        Hashtable htorder = new Hashtable();
                        htorder.Add("Transed", "1");
                        htorder.Add("ordernum", strOrderNum);
                        bool falg = new OrdersService().EditTransed(htorder);

                        SetTB(String.Format("订单号：{0}报告生成状态【{1}】！", strOrderNum, falg));
                    }
                }
                else
                {
                    SetTB(String.Format("---{0}  没有传输的数据！\n", DateTime.Now));

                }
                #endregion
            }
            catch (Exception ex)
            {
                string strmessage = String.Format("订单号：{0}报告生成出错；时间{1}:  错误原因：{2}", strOrderNum, DateTime.Now, ex.Message);
                SetTB(strmessage);
                CreateErrorLog(strmessage);
            }
            finally
            {
                timer.Start();
            }
        }

        /// <summary>启用
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBegan_Click(object sender, EventArgs e)
        {

            b = false;
            timer.Enabled = true;
            timer.Start();
            
            int num = Convert.ToInt32(tbTime.Text.Trim() == "" ? 0 : Convert.ToInt32(tbTime.Text.Trim()));
            //设置timer引发 Elapsed 事件的间隔时间 毫秒为单位 1000毫秒为1秒     
            if (num != 0)
            {
                timer.Interval = 1000 * num;// this.tbTime.Text.Trim();
            }
            else
            {
                timer.Interval = 1000;
            }
            //设置是否重复计时，如果该属性设为False,则只执行timer_Elapsed方法一次。
            timer.AutoReset = true;
            btnBegan.Enabled = false;
            string strmsg = string.Format(">>>系统已启动： {0}", DateTime.Now);
            SetTB(strmsg);
        }

        /// <summary>停止
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {

            b = true;
            timer.Stop();
            timer.AutoReset = false;
            btnBegan.Enabled = true;
        }

        /// <summary>退出
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            FrmUploadShequ88 frm = new FrmUploadShequ88();
            frm.Dispose();
            b = true;
            timer.AutoReset = false;
            string strmsg = string.Format(">>>退 出 时 间：{0}", DateTime.Now);
            SetTB(strmsg);
            Close();
        }


        /// <summary>验证文本框只能输入数字
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 阻止从键盘输入键
            e.Handled = true;
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == (char)8))
            {
                if ((e.KeyChar == (char)8))
                {
                    e.Handled = false;
                    return;
                }
                else
                {
                    int len = tbTime.Text.Length;
                    if (len < 3)
                    {
                        if (len == 0 && e.KeyChar != '0')
                        {
                            e.Handled = false;
                            return;
                        }
                        else if (len == 0)
                        {
                            MessageBox.Show("不能以0为开头！", "体检系统", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        e.Handled = false;
                        return;
                    }
                    else
                    {
                        MessageBox.Show("只能输入3位数字！", "体检系统", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }
            else
            {
                MessageBox.Show("只能输入数字！", "体检系统", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private readonly static string FileOrPath = Application.StartupPath + "\\Log\\UploadShequ88\\";
        private static string m_fileName = String.Format("{0}{1:yyyyMMdd}.txt", FileOrPath, DateTime.Now);
        public static String FileName
        {
            get { return (m_fileName); }
            set
            {
                if (value != null || value != "")
                { m_fileName = value; }
            }
        }
        /// <summary>记录日志至文本文件，每天保存一个日志文件
        /// 
        /// </summary>    
        /// <param name="message">记录的内容</param>    
        public static void CreateErrorLog(string message)
        {
            if (!Directory.Exists(FileOrPath))//若文件夹不存在则新建文件夹
            {
                Directory.CreateDirectory(FileOrPath); //新建文件夹
            }
            m_fileName = String.Format("{0}{1:yyyyMMdd}.txt", FileOrPath, DateTime.Now);
            if (File.Exists(m_fileName))
            {
                ///如果日志文件已经存在，则直接写入已有的日志文件    
                using (StreamWriter sr = File.AppendText(FileName))
                {
                    sr.WriteLine("\n");
                    sr.WriteLine(message);
                    sr.Close();
                }
            }
            else
            {
                ///创建日志文件           
                using (StreamWriter sr = File.CreateText(FileName))
                {
                    sr.WriteLine("\n");
                    sr.WriteLine(message);
                    sr.Close();
                }
            }
        }


        private delegate void SetTBMethodInvok(string value);
        private void SetTB(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new SetTBMethodInvok(SetTB), value);
            }
            else
            {
                if (tbxMessage == null || tbxMessage.IsDisposed)
                {
                    tbxMessage = new RichTextBox();
                }
                tbxMessage.Text += value + "\n";
                CreateErrorLog(value);
                if (tbxMessage.Text.Length > 3000)
                {
                    tbxMessage.Text = value + "\n";
                }
                //有滚动条时 ，定位到textbox最下方
                tbxMessage.SelectionStart = tbxMessage.Text.Length;
                tbxMessage.ScrollToCaret();
            }
        }
    }
}
