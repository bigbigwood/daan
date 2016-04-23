using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using daan.web.code;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Xml;
using daan.domain;
using System.Collections;
using daan.service;

namespace daan.ui.main
{
    public partial class FrmTestitems : Form
    {
        public bool _IsRunning = false;

        /// <summary>
        /// 开启线程  在线程中定时获取接口数据
        /// </summary>
        public void ThreadStart()
        {
            if (_IsRunning)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(MainDeposit));
            }
        }

        /// <summary>
        /// 每次间隔时间
        /// </summary>
        private int outTime
        {
            get
            {
                if (!string.IsNullOrEmpty(this.tbTime.Text) && this.tbTime.Text != "0")
                {
                    return Convert.ToInt32(this.tbTime.Text);
                }
                else
                    return 5;
            }
        }

        /// <summary>
        /// 2015-09-16  李国庆
        /// 根据康源提供的接口获取项目对照的数据 
        /// </summary>
        /// <param name="ob"></param>
        private void MainDeposit(object ob)
        {
            int n = 0;
            string msg = string.Empty;
            while (this._IsRunning)
            {
                try
                {
                    ServiceByKangSource.TestItemUniquecodeCompareService kangSource = new ServiceByKangSource.TestItemUniquecodeCompareService();
                    string reValue = kangSource.findUniquecodeCompare();
                    //string reValue = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><data><data_row><olduniquecode>CAAR10000451</olduniquecode><newuniquecode>CAAB1000025052</newuniquecode><testname>大血小板比率(P-LCR）</testname><testmethodname>仪器法</testmethodname></data_row></data>";
                    if (!string.IsNullOrEmpty(reValue))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.InnerXml = reValue;

                        XmlNodeList topM = xmlDoc.DocumentElement.ChildNodes;
                        IList<ProjectControl> list = new List<ProjectControl>();

                        foreach (XmlElement element in topM)
                        {
                            if (element.Name.ToLower() == "data_row")
                            {
                                XmlNodeList nodelist = element.ChildNodes;
                                if (nodelist.Count > 0)
                                {
                                    ProjectControl item = new ProjectControl();
                                    item.PID = Guid.NewGuid().ToString();
                                    foreach (XmlElement el in nodelist)//读元素值  
                                    {
                                        if (el != null)
                                        {
                                            switch (el.Name.ToLower())
                                            {
                                                case "olduniquecode":
                                                    item.OldUniquecode = el.InnerText;
                                                    break;
                                                case "newuniquecode":
                                                    item.NewUniquecode = el.InnerText;
                                                    break;
                                                case "testname":
                                                    item.TestName = el.InnerText;
                                                    break;
                                            }
                                        }
                                    }
                                    list.Add(item);
                                }

                            }

                        }

                        if (list != null && list.Count > 0)
                        {
                            ProjectControlService pr = new ProjectControlService();
                            string error = string.Empty;
                            if (pr.InsertProjectControl(list, ref error))
                            {
                                msg = "获取数据成功！此次共获取：" + list.Count + "条数据";
                            }
                            else
                            {
                                msg = "获取数据失败。" + error;
                            }
                            n++;
                        }

                    }
                }
                catch (Exception ex)
                {
                    n++;
                    msg = "获取数据失败，失败原因：" + ex.Message;
                    CreateErrorLog(msg);
                }
                //
                SetTB("第" + n + "次获取数据。" + msg + " 获取时间：" + DateTime.Now);
                Thread.Sleep(outTime * 1000);
            }
        }

        public FrmTestitems()
        {
            InitializeComponent();
            this.tbTime.Text = "7200";
            //ContextMenu m = new ContextMenu();
            //tbTime.ContextMenu = m;
            //timer.Elapsed += timer_Elapsed;
            //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            //timer.AutoReset = false;
            //  Control.CheckForIllegalCrossThreadCalls = false;

            this.btnStop.Enabled = false;
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //if (b)
        //{
        //    return;
        //}
        ////设置timer不可用
        //timer.Stop();
        ////传输数据 
        //try
        //{
        //    //SendOrdersToLis.CenterServiceSoapClient c = new SendOrdersToLis.CenterServiceSoapClient();
        //    //调用康源系统接口读取项目对照表，并将数据同步到体检系统

        //}
        //catch (Exception ex)
        //{
        //    string strmessage = String.Format("同步康源项目对照出错；时间{1}:  错误原因：{2}",DateTime.Now, ex.Message);
        //    SetTB(strmessage);
        //    CreateErrorLog(strmessage);
        //}
        //finally
        //{
        //    timer.Start();
        //}

        #region >>>> 记录
        private readonly static string FileOrPath = Application.StartupPath + "\\Log\\Testitems\\";
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
        #endregion

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
                    if (len < 4)
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
                        MessageBox.Show("只能输入4位数字！", "体检系统", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }
            else
            {
                MessageBox.Show("只能输入数字！", "体检系统", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnBegan_Click(object sender, EventArgs e)
        {
            this._IsRunning = true;
            this.ThreadStart();

            btnBegan.Enabled = false;
            this.btnExit.Enabled = false;
            this.btnStop.Enabled = true;
            string strmsg = string.Format(">>>系统已启动： {0}", DateTime.Now);
            SetTB(strmsg);

            //b = false;
            //timer.Enabled = true;
            //timer.Start();

            //int num = Convert.ToInt32(tbTime.Text.Trim() == "" ? 0 : Convert.ToInt32(tbTime.Text.Trim()));
            ////设置timer引发 Elapsed 事件的间隔时间 毫秒为单位 1000毫秒为1秒     
            //if (num != 0)
            //{
            //    timer.Interval = 1000 * num;
            //}
            //else
            //{
            //    timer.Interval = 1000;
            //}
            ////设置是否重复计时，如果该属性设为False,则只执行timer_Elapsed方法一次。
            //timer.AutoReset = true;

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //b = true;
            //timer.Stop();
            //timer.AutoReset = false;

            this.btnBegan.Enabled = true;
            this.btnStop.Enabled = false;
            this.btnExit.Enabled = true;
            this._IsRunning = false;

            string strmsg = string.Format(">>>系统已停止： {0}", DateTime.Now);
            SetTB(strmsg);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            FrmTestitems frm = new FrmTestitems();
            frm.Dispose();
            //b = true;
            //timer.AutoReset = false;
            string strmsg = string.Format(">>>退 出 时 间：{0}", DateTime.Now);
            SetTB(strmsg);
            Close();
        }

    }
}
