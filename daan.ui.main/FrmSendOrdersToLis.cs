using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using daan.service.order;
using daan.domain;
using System.Configuration;
using daan.service.proceed;
using System.IO;
using System.Diagnostics;
using System.Collections;
using daan.service.dict;

namespace daan.ui.main
{
    public partial class FrmSendOrdersToLis : Form
    {
        readonly DictlabService labser = new DictlabService();
        bool b;
        readonly Hashtable ht = new Hashtable();
        private readonly System.Timers.Timer timer = new System.Timers.Timer();

        public FrmSendOrdersToLis()
        {
            InitializeComponent();
            tbTime.Text = "10";
            ContextMenu m = new ContextMenu();
            tbTime.ContextMenu = m;
            timer.Elapsed += timer_Elapsed;
        }

        //启用
        private void btnCanRun_Click(object sender, EventArgs e)
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
            btnCanRun.Enabled = false;
            SetTB(">>>系统已启动");
        }

        //停止
        private void btnStop_Click(object sender, EventArgs e)
        {
            b = true;
            timer.AutoReset = false;
            btnCanRun.Enabled = true;
        }

        //退出
        private void btnExit_Click(object sender, EventArgs e)
        {
            FrmSendOrdersToLis frm = new FrmSendOrdersToLis();
            frm.Dispose();
            b = true;
            timer.AutoReset = false;
            SetTB(">>>退 出 时 间");
            Close();
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
                #region 传输数据
                // LoginService loginservice = new LoginService();
                OrdersService orderservice = new OrdersService();
                OrderbarcodeService orderbarcodeservice = new OrderbarcodeService();
                Stopwatch sw = new Stopwatch();

                //调用登陆验证方法(string Login(UserName: string; Password: string; Operator: string))返回SID
                //UserName，Password来源配置文件，Operator为空
                string username = ConfigurationManager.AppSettings["UserName"];
                string password = ConfigurationManager.AppSettings["Password"];
                string Operator = ConfigurationManager.AppSettings["Operator"];
                SendOrdersToLis.CenterServiceSoapClient client = new SendOrdersToLis.CenterServiceSoapClient();                
                sw.Start();
                // int t = 0;
                string strM = "";
                //按分点查找
                List<Dictlab> labLst = labser.GetDictlabList().Where(c => c.IsActive == '1').ToList<Dictlab>();
                #region
                foreach (Dictlab dictlab in labLst)
                {
                    //查询分点下面有没有数据
                    DataTable dt = orderservice.GetOrderToLis(dictlab.Dictlabid);// orderservice.GetOrderToLis(dictlab.Dictlabid);
                    if (dt.Rows.Count > 0)
                    {
                        if (!ht.ContainsKey(dictlab.Labcode))
                        {
                            string strsid = client.Login(dictlab.Labcode, username, password, Operator);
                            if (strsid.Split('|')[0] == "1")
                            {
                                strsid = strsid.Split('|')[1];
                                ht.Add(dictlab.Labcode, strsid);
                            }
                            else
                            {
                                string strS01 = String.Format("{0}登录失败！\n{1}", dictlab.Labname, strsid.Split('|')[1]);
                                SetTB(strS01);
                                continue;
                                //return;
                            }
                        }
                        #region
                        dt.TableName = "data_row";
                        string strxml = DataToXml.CDataToXml(dt);//将订单信息转换成xml形式的字符串
                        //根据SID，分点，及分点下的订单信息(xml字符串形式)调用Webservice上传数据到Lis
                        string strmessage = client.SendRequestInfo(ht[dictlab.Labcode].ToString(), dictlab.Labcode, StringToXML(strxml));
          
                        if (strmessage.Contains("MSG0006")) //登陆超时
                        {
                            ht.Remove(dictlab.Labcode);
                            SetTB(dictlab.Labname+" 登录超时！");
                            continue;
                        }
                        else
                        {
                            string[] strsp = strmessage.Split(',');
                            #region for
                            for (int k = 0; k < strsp.Length - 1; k++)
                            {
                                string[] s = strsp[k].Split('|');
                                if (s[3] == "0")
                                {
                                    strM += String.Format("{0}  达安条码[{1}],上传成功！\n", dictlab.Labname, s[0]);
                                    //上传后 如果成功就跟新orderbarcode中transed=1并给出成功提示信息，如果失败则给出失败信息
                                    orderbarcodeservice.UpdateTransedToLis(s[0]);                            
                                }
                                else
                                {
                                    string[] g = s[3].Split('/');
                                    #region for
                                    for (int i = 0; i < g.Length - 1; i++)
                                    {
                                        if (g[i] == "MSG1001")
                                        {
                                            strM += "申请信息不全，达安条码、医院条码、标本唯一标识不能全为空；";
                                        }
                                        if (g[i] == "MSG1002")
                                        {
                                            strM += String.Format("达安条码[{0}],申请信息姓名、性别、年龄必填项，有空值；", s[0]);
                                        }
                                        if (g[i] == "MSG1003")
                                        {
                                            strM += String.Format("达安条码[{0}],有匹配不正确的达安项目代码；", s[0]);
                                        }
                                        if (g[i] == "MSG1004")
                                        {
                                            strM += String.Format("达安条码[{0}],年龄格式不正确；", s[0]);
                                        }
                                        if (g[i] == "MSG1005")
                                        {
                                            strM += String.Format("达安条码[{0}],采样时间不能转换成时间格式；", s[0]);
                                        }
                                        if (g[i] == "MSG1007")
                                        {
                                            strM += String.Format("达安条码[{0}],病人电话格式错误；", s[0]);
                                        }
                                        if (g[i] == "MSG1008")
                                        {
                                            strM += String.Format("达安条码[{0}],医生电话格式错误；", s[0]);
                                        }
                                        if (g[i] == "MSG1009")
                                        {
                                            strM += String.Format("达安条码[{0}],病理标本、细菌标本必须单独信息；", s[0]);
                                        }
                                        if (g[i] == "MSG1010")
                                        {
                                            strM += String.Format("达安条码[{0}],性别必须为M或者F或者U；", s[0]);
                                        }
                                    }
                                    #endregion
                                    //失败信息，更新orderbarcode中transed=2
                                    if (s[0] != null && s[0] != "")
                                    {
                                        orderbarcodeservice.UpdateTransedToLisFail(s[0]);
                                    }
                                }
                            }
                            #endregion
                            SetTB(strM);
                            strM = null;
                        }
                        #endregion
                    }
                    else
                    {
                        string str2 = dictlab.Labname + "没有传输的数据！\n";
                        SetTB(str2);
                    }

                #endregion
                    sw.Stop(); 
                } 
                #endregion
            }
            catch (Exception ex)
            {
                string exmessage = String.Format("{0}:  {1}", DateTime.Now, ex.Message);
                SetTB(exmessage);
                CreateErrorLog(exmessage);
            }
            finally
            {
                //设置timer可用
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
                value = String.Format("{0}:{1}", DateTime.Now, value);
                rtbMessage.Text += value + "\n";
                CreateErrorLog(value);
                if (rtbMessage.Text.Length > 5000)
                {
                    rtbMessage.Text = value + "\n";
                }
                //有滚动条时 ，定位到textbox最下方
                rtbMessage.SelectionStart = rtbMessage.Text.Length;
                rtbMessage.ScrollToCaret();
            }

        }


        private readonly static string FileOrPath = Application.StartupPath + "\\OrderInfoLog\\";
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

        /// <summary>将结果在xml中关键字转义
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string StringToXML(object str)
        {
            return str.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos; ").Replace("\"", "&quot;");
        }
    }
}
