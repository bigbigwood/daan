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
using daan.service.order;
using System.Collections;
using daan.service.common;
using daan.domain;
namespace daan.ui.main
{
    /*
     * 自动初步总检条件：
     * 1、状态为待总检
     * 2、报告类型为B类
     * 3、所有项目的检测结果皆为正常
     * 符合以上条件的系统将自动进行初步总检，
     * 根据医学部给出的10种正常结果描述评价，随机填写10种中的一种，
     * 且初步总检医生统一为【2401】【医学部管理员】
     */
    public partial class FrmAutoChuBu : Form
    {
        bool b;
        readonly OrdersService os = new OrdersService();
        private readonly System.Timers.Timer timer = new System.Timers.Timer();
        private static readonly string username = "医学部管理员";
        private static readonly double userid = 2401;
        public FrmAutoChuBu()
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
            timer.Stop();
            try
            {
                //自动总检
                using(DataTable dt=os.GetAutoChuBuList())
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string message = string.Empty;
                            if (dr["reporttype"].ToString() == ((int)ParamStatus.ReportTypeStatus.TM15).ToString())//TM
                            {
                                autoChuBu(dr);
                                message = string.Format("【TM】{0}: 体检号: {1}初步总检完成。", DateTime.Now, dr["ordernum"]);
                            }
                            else//C14
                            {
                                autoC14Check(dr);
                                message = string.Format("【C14】{0}: 体检号: {1}总检完成。", DateTime.Now, dr["ordernum"]);
                            }
                            SetTB(message);
                        }
                        SetTB("=========================================");
                    }
                }
            }
            catch (Exception ex)
            {
                string strmessage = String.Format("自动总检出错；时间{1}:  错误信息：{2}", DateTime.Now, ex.Message);
                SetTB(strmessage);
                CreateErrorLog(strmessage); 
            }
            finally
            {
                timer.Start();
            }
        }

        /// <summary>
        /// TM自动初步总检
        /// </summary>
        /// <param name="dr"></param>
        private void autoChuBu(DataRow dr)
        {
            string ordernum = dr["ordernum"].ToString();
            string resultcomment = dr["resultcomment"].ToString();
            //step one :检测结果是否有异常，无则进行自动初步总检，有则跳过并标记
            if(os.CheckTestResultIsException(ordernum))
            {
                //对此类订单做标记，下次扫描就不查询
                Hashtable htStatus = new Hashtable();
                htStatus.Add("status", 1);
                htStatus.Add("ordernum", ordernum);
                os.SetAutoFirstCheckStatus(htStatus);
                return;
            }
            //step two :根据是否已有结果评价，如果有则直接进入下一步，否则先从正常结果描述评价中随机选一个保存到结果评价表
            if (string.IsNullOrEmpty(resultcomment))
            {
                Random rand = new Random();
                int k = rand.Next(1,11);
                DataTable dtComment = os.GetRandResultComment(k);
                if (dtComment.Rows.Count > 0)
                {
                    resultcomment = dtComment.Rows[0]["resultcomment"].ToString();
                }
                Orderresultcomment orc = new Orderresultcomment();
                orc.Engresultcomment = orc.Engresultsuggestion = string.Empty;
                orc.Resultcomment = resultcomment;
                orc.Resultsuggestion = string.Empty;
                orc.Ordernum = ordernum;
                OrderresultcommentService orcs = new OrderresultcommentService();
                if (orcs.SelectOrderresultcomment(orc.Ordernum) == null)
                {
                    orcs.InsertOrderresultcomment(orc);
                }
                else
                {
                    orcs.UpdateOrderresultcomment(orc);
                }
            }
            //step three :初步总检
            Hashtable ht = new Hashtable();
            ht.Add("ordernum", ordernum);
            ht.Add("oldstatus", (int)ParamStatus.OrdersStatus.WaitCheck);
            ht.Add("status", (int)ParamStatus.OrdersStatus.FirstCheck);
            ht.Add("authorizedbyid", userid);
            if (os.EditStatusByOldStatus(ht))
            {
                //step four :初步总检成功后记录操作日志
                Hashtable htScan = new Hashtable();
                htScan.Add("isScan", true);
                htScan.Add("EnterByID", userid);
                htScan.Add("EnterBy", username);
                os.AddOperationLog(ordernum, null, "初步总检", "对[" + ordernum + "]执行自动初步总检", "修改留痕", "", htScan);
            }
        }

        /// <summary>
        /// C14自动初步总检+完成总检，不用判断结果是否异常，且不用写结果评价
        /// </summary>
        /// <param name="dr"></param>
        private void autoC14Check(DataRow dr)
        {
            string ordernum = dr["ordernum"].ToString();
            Hashtable htScan = new Hashtable();
            htScan.Add("isScan", true);
            htScan.Add("EnterByID", userid);
            htScan.Add("EnterBy", username);
            //1、初步总检
            Hashtable ht = new Hashtable();
            ht.Add("ordernum", ordernum);
            ht.Add("oldstatus", (int)ParamStatus.OrdersStatus.WaitCheck);
            ht.Add("status", (int)ParamStatus.OrdersStatus.FirstCheck);
            ht.Add("authorizedbyid", userid);
            if (os.EditStatusByOldStatus(ht))
            {
                //初步总检成功后记录操作日志
                os.AddOperationLog(ordernum, null, "初步总检", "对[" + ordernum + "]执行自动初步总检", "修改留痕", "", htScan);
                //2、完成总检
                Hashtable ht1 = new Hashtable();
                ht1.Add("ordernum", ordernum);
                ht1.Add("oldstatus", (int)ParamStatus.OrdersStatus.FirstCheck);
                ht1.Add("status", (int)ParamStatus.OrdersStatus.FinishCheck);
                ht1.Add("finishbyid", userid);
                ht1.Add("TRANSED", 0);
                if (os.EditStatusByOldStatus(ht1))
                {
                    //完成总检成功后记录操作日志
                    os.AddOperationLog(ordernum, null, "完成总检", "对[" + ordernum + "]执行自动完成总检", "修改留痕", "", htScan);
                }
            }
        }

        #region >>>> 记录日志
        private readonly static string FileOrPath = Application.StartupPath + "\\Log\\ChuBu\\";
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

        private void btnBegan_Click(object sender, EventArgs e)
        {
            b = false;
            timer.Enabled = true;
            timer.Start();

            int num = Convert.ToInt32(tbTime.Text.Trim() == "" ? 0 : Convert.ToInt32(tbTime.Text.Trim()));
            //设置timer引发 Elapsed 事件的间隔时间 毫秒为单位 1000毫秒为1秒     
            if (num != 0)
            {
                timer.Interval = 1000 * num;
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

        private void btnStop_Click(object sender, EventArgs e)
        {
            b = true;
            timer.Stop();
            timer.AutoReset = false;
            btnBegan.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            FrmAutoChuBu frm = new FrmAutoChuBu();
            frm.Dispose();
            b = true;
            timer.AutoReset = false;
            string strmsg = string.Format(">>>退 出 时 间：{0}", DateTime.Now);
            SetTB(strmsg);
            Close();
        }
    }
}
