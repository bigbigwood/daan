using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using daan.service.order;


namespace daan.ui.main
{
    public partial class FrmXiaoJie : Form
    {
        bool b;
        private readonly System.Timers.Timer timer = new System.Timers.Timer();
        readonly OrderlabdeptresultService orderlabdeptresultService = new OrderlabdeptresultService();
        public FrmXiaoJie()
        {
            InitializeComponent();
            tbTime.Text = "10";
            ContextMenu m = new ContextMenu();
            tbTime.ContextMenu = m;
            timer.Elapsed += timer_Elapsed;
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
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

                using (DataTable dt = orderlabdeptresultService.GetFrmXiaoJieAudioRecord())
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            orderlabdeptresultService.AutoSummary(dr["ordernum"].ToString(), Convert.ToDouble(dr["dictlabid"])); //自动小结
                            string strmessage = String.Format("{0}: 条码号: {1}小结完毕。", DateTime.Now, dr["ordernum"]);
                            SetTB(strmessage);
                        }
                    }
                }
                //orderlabdeptresultService.AutoSummary("2015061102649", 48);
                #endregion
            }
            catch (Exception ex)
            {
                string strmessage = String.Format("{0}:  {1}", DateTime.Now, ex.Message);
                SetTB(strmessage);
                CreateErrorLog(strmessage);
            }
            finally
            {
                timer.Start();
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

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBegan_Click(object sender, EventArgs e)
        {

            b = false;
            timer.Start();
            //设置timer可用
            timer.Enabled = true;
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
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            b = true;
            // timer.Stop();
            // timer.Enabled = true;
            timer.AutoReset = false;
            btnBegan.Enabled = true;
        }

        /// <summary>
        /// 退出
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






        /// <summary>
        /// 验证文本框只能输入数字
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





        private readonly static string FileOrPath = Application.StartupPath + "\\Log\\";
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
        /// <summary>      
        /// 记录日志至文本文件，每天保存一个日志文件
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

    }
}
