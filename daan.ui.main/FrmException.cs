using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using daan.service.dict;
using System.Collections;
using System.Configuration;
using daan.domain;
using System.IO;
using daan.service.order;
using System.Diagnostics;
using daan.service;
using log4net;
using System.Reflection;

namespace daan.ui.main
{
    public partial class FrmException : Form
    {
        string strMsg = string.Empty;
        DictlabService labser = new DictlabService();
        Hashtable ht = new Hashtable();
        //日志记录
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType); 
        private System.Timers.Timer timer = new System.Timers.Timer(1000);


        //初始化
        public FrmException()
        {
            InitializeComponent();
            this.tbTime.Text = "10";
            ContextMenu m = new ContextMenu();
            tbTime.ContextMenu = m;
        }

        //启用
        private void btnCanRun_Click(object sender, EventArgs e)
        {
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            int num = Convert.ToInt32(this.tbTime.Text.Trim() == "" ? 0 : Convert.ToInt32(this.tbTime.Text.Trim()));
            if (num != 0)
            {
                //timer.Interval = 1000 * num * 60;
                timer.Interval = 1000 * num;
            }
            else
            {
                timer.Interval = 1000*60;
            }
            timer.AutoReset = true;//设置是否重复计时，如果该属性设为False,则只执行timer_Elapsed方法一次。
            timer.Enabled = true;            

            strMsg = string.Format(">>>{0}    启动系统", DateTime.Now);
            log.InfoFormat(strMsg);
            tree_logview.Nodes.Add(strMsg);
   
            this.btnCanRun.Enabled = false;
            
        }
        
        //退出
        private void btnExit_Click(object sender, EventArgs e)
        {
            strMsg = string.Format(">>>{0}    退出系统", DateTime.Now);
            log.Info(strMsg);
            tree_logview.Nodes.Add(strMsg);

            //timer.Stop();
            //timer.Dispose();
            //timer.Close();

            FrmSendOrdersToLis frm = new FrmSendOrdersToLis();
            frm.Dispose();            
            this.Close(); 
    
        }

        /// <summary>扫描业务主体
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {  

            //设置timer不可用
            timer.Stop();
            try
            {              
                Orderexception exception = new Orderexception();
                OrderexceptionService service = new OrderexceptionService();

                //调用登陆验证方法(string Login(UserName: string; Password: string; Operator: string))返回SID
                //UserName，Password来源配置文件，Operator为空
                string username = ConfigurationManager.AppSettings["UserName"];
                string password = ConfigurationManager.AppSettings["Password"];
                string Operator = ConfigurationManager.AppSettings["Operator"];
                SendOrdersToLis.CenterServiceSoapClient client = new SendOrdersToLis.CenterServiceSoapClient();      
                
                //按分点查找
                List<Dictlab> labLst = labser.GetDictlabList().Where(c=>c.IsActive == '1').ToList<Dictlab>();
                //最后一次更新时间
                
                foreach (Dictlab dictlab in labLst)
                {
                    string lastDate = service.SelectOrderExceptionLastDate(dictlab.Labcode);
                    if (lastDate == null)
                    {
                        lastDate = DateTime.Now.AddDays(-30).ToString();
                        //lastDate = "2012-12-01";
                    }
                    if (!ht.ContainsKey(dictlab.Labcode))
                    {
                        string strsid = client.Login(dictlab.Labcode, username, password, Operator);
                        if (strsid.Split('|')[0].ToString() == "1")
                        {
                            strsid = strsid.Split('|')[1].ToString();
                            ht.Add(dictlab.Labcode, strsid);
                        }
                        else
                        {
                            strMsg = string.Format(">>>{0}    {1}:登录失败!{2}", DateTime.Now, dictlab.Labname, strsid.Split('|')[1].ToString());
                           
                            AddNodeHandler addNode = new AddNodeHandler(this.TreeViewAdd);
                            this.Invoke(addNode, strMsg);
                            continue;
                        }
                    }
                    // 获取LIS的取消审核与退单信息
                    string strmessage = client.SelectPesExceptionLst(ht[dictlab.Labcode].ToString(), dictlab.Labcode, lastDate);

                    if (strmessage.Contains("MSG0006")) //登陆超时
                    {
                        ht.Remove(dictlab.Labcode);                        
                        strMsg=string.Format(">>>{0}    {1}:登录超时",DateTime.Now,dictlab.Labname);
                        AddNodeHandler addNode = new AddNodeHandler(this.TreeViewAdd);
                        this.Invoke(addNode, strMsg);      
                        continue;
                    }
                    else
                    {
                        string[] strcontent = strmessage.Split('|');
                        if (strcontent[0] == "0")
                        {
                            strMsg=string.Format(">>>{0}    {1}:未查询到数据",DateTime.Now,dictlab.Labname);
                            AddNodeHandler addNode = new AddNodeHandler(this.TreeViewAdd);
                            this.Invoke(addNode, strMsg);
                            continue;
                        }
                        else
                        {
                            DataSet ds = new CommonFuncLibService().CXmlToDataSet(strcontent[1]);
                            if (service.AddOrderExceptional(ds.Tables[0], dictlab.Labcode))
                            {
                                strMsg=string.Format("***{0}    {1}:异常信息获取成功", DateTime.Now, dictlab.Labname);
                                AddNodeHandler addNode = new AddNodeHandler(this.TreeViewAdd);
                                this.Invoke(addNode, strMsg);
                            }
                            else
                            {
                                strMsg=string.Format(">>>{0}    {1}:异常信息获取失败，方法名称：SelectPesExceptionLst", DateTime.Now, dictlab.Labname);
                                AddNodeHandler addNode = new AddNodeHandler(this.TreeViewAdd);
                                this.Invoke(addNode, strMsg);
                            }
                        }  
                    }
                }
            }
            catch (Exception ex)
            {
                strMsg = string.Format(">>>{0}    {1}", DateTime.Now.ToString() + ":  " + ex.Message);
                AddNodeHandler addNode = new AddNodeHandler(this.TreeViewAdd);
                this.Invoke(addNode, strMsg);
            }
            finally
            {
                if (timer != null)
                {
                    timer.Start();
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
        private string StringToXML(object str)
        {
            return str.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos; ").Replace("\"", "&quot;");
        }

        #region 日志显示
        delegate void AddNodeHandler(string node);
        private void TreeViewAdd(string str)
        {
            if (!str.Contains("未查询到数据"))
            {
                log.Info(str);
            }
            if (tree_logview.Nodes.Count > 11)
            {
                tree_logview.Nodes.Clear();
            }
            tree_logview.Nodes.Add(str);
            tree_logview.SelectedNode = tree_logview.Nodes[tree_logview.Nodes.Count - 1];

        }
        #endregion
    }
}