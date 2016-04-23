using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Collections;
using daan.service.order;
using daan.service.proceed;

namespace daan.ui.main
{
    public partial class FrmJsonFilesWatcher : Form
    {
        FileSystemWatcher watcher = null;
        static readonly ServiceToAPP.RouteSpecimenServicePortTypeClient rs = new ServiceToAPP.RouteSpecimenServicePortTypeClient();
        static readonly OrderbarcodeService barcodeservice = new OrderbarcodeService();
        public FrmJsonFilesWatcher()
        {
            InitializeComponent();
            this.txtPath.Text = ConfigurationManager.AppSettings["NewJsonFilesPath"].ToString();

            //string res = rs.routeSpecimen("380144119100", "正常迟发", DateTime.Now.ToString(), "2016-01-12", "标本溶血。");
        }

        /// <summary>
        /// 开始监视
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string path=txtPath.Text;
            //开启文件夹文件监听
            WatcherStrat(path, "*.json");
            //处理文件夹中已经存在的json文件

        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (watcher != null)
                watcher.Dispose();
            FrmJsonFilesWatcher fj = new FrmJsonFilesWatcher();
            fj.Dispose();
            string strmsg = string.Format(">>>退 出 时 间：{0}", DateTime.Now);
            SetTB(strmsg);
            Close();
        }

        #region FileWatcher
        private void WatcherStrat(string path, string filter)
        {
            string strmsg = string.Format("文件监听程序开启【{0}】 \n监控路径：[{1}]\n监听文件类型：[{2}]\n======================================================", DateTime.Now.ToString(), path, filter);
            SetTB(strmsg);
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Filter = filter;
            watcher.Created += new FileSystemEventHandler(OnProcess);
            watcher.Deleted += new FileSystemEventHandler(OnProcess);
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess| NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
            watcher.IncludeSubdirectories = true;
        }

        private void OnProcess(object source, FileSystemEventArgs e)
        {
            try
            {
                if (e.ChangeType == WatcherChangeTypes.Created)
                {
                    OnCreated(source, e);
                }
                else if (e.ChangeType == WatcherChangeTypes.Deleted)
                {
                    OnDeleted(source, e);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}]解析JSON文件出错\n文件信息：[{1}]\n错误信息：{2}", DateTime.Now.ToString(), e.FullPath, ex.Message);
                SetTB(msg);
                //转移异常文件
                string excPath = ConfigurationManager.AppSettings["ExcJsonFilesPath"];
                moveJsonFile(e.FullPath, excPath+"\\"+e.Name,true);
            }
        }
        /// <summary>
        /// 新建文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            string msg = string.Format("[{0}]监听到有文件【{1}】,文件信息:[{2}]", DateTime.Now.ToString(), e.ChangeType, e.FullPath);
            SetTB(msg);
            if (File.Exists(e.FullPath))
            {
                //开始解析文件
                msg = string.Format("[{0}]开始解析json文件,[{1}]", DateTime.Now.ToString(), e.Name);
                SetTB(msg);
                string json = File.ReadAllText(e.FullPath, Encoding.Default);
                JArray j = JArray.Parse(json);

                List<Hashtable> list = new List<Hashtable>();
                List<Hashtable> listapp = new List<Hashtable>();
                string res=readJsonString(j,ref list,ref listapp);
                if (!string.IsNullOrEmpty(res)||list.Count!=j.Count)
                {
                    SetTB(string.Format("[{0}]解析文件过程中产生异常，异常信息：[{1}]",DateTime.Now.ToString(),res));
                    //将json文件转移到异常文件夹
                    moveJsonFile(e.FullPath, ConfigurationManager.AppSettings["ExcJsonFilesPath"] + "\\" + e.Name, true);
                    return;
                }
                OrdersService os = new OrdersService();

                //如果条码来自医护平台，同时调用医护平台webservice方法插入异常报告记录
                if (listapp.Count > 0)
                {
                    foreach (Hashtable item in listapp)
                    {
                        //string res = rs.routeSpecimen("380144119100", "正常迟发", DateTime.Now.ToString(), "2015-12-12", "病理会诊。");
                        string response = rs.routeSpecimen(item["BARCODE"].ToString(), item["STATUS"].ToString(), item["CREATEDATE"].ToString(),
                            item["EXPREPORTDATE"].ToString(), item["EXCEPTIONREASON"].ToString());

                    }
                }
                string errstr = string.Empty;
                bool b = os.InsertOrderExceptionReport(list, ref errstr);
                if (!b)
                {
                    SetTB(string.Format("[{0}]解析文件过程中产生异常，异常信息：[{1}]", DateTime.Now.ToString(), errstr));
                    //将json文件转移到异常文件夹
                    moveJsonFile(e.FullPath, ConfigurationManager.AppSettings["ExcJsonFilesPath"] + "\\" + e.Name, true);
                }
                else
                {
                    //文件解析完毕后转移文件
                    moveJsonFile(e.FullPath, ConfigurationManager.AppSettings["OldJsonFilesPath"] + "\\" + e.Name, false);
                }
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnDeleted(object source, FileSystemEventArgs e)
        {

        }
        /// <summary>
        /// 解析Json
        /// </summary>
        /// <param name="json"></param>
        private string readJsonString(JArray json,ref List<Hashtable> list,ref List<Hashtable> listapp)
        {
            string errstr=string.Empty;
            foreach (var item in json)
            {
                string barcode=item["BARCODE"].ToString();
                if (string.IsNullOrEmpty(barcode))
                {
                    errstr += "条码号为空；";
                }
                string testcode = item["TESTCODE"].ToString();
                if (string.IsNullOrEmpty(testcode))
                {
                    errstr += "全国统一码为空；";
                }
                string exctype = item["EXCEPTIONTYPE"].ToString();
                if (string.IsNullOrEmpty(exctype))
                {
                    errstr += "异常报告类型为空；";
                }
                else
                {
                    if (exctype != "2" && exctype != "1")
                    {
                        errstr += "异常报告类型错误；";
                    }
                }
                string reason = item["EXCEPTIONREASON"].ToString();
                if (string.IsNullOrEmpty(reason))
                {
                    errstr += "原因为空；";
                }
                string expdate = item["EXPREPORTDATE"].ToString();
                if (!string.IsNullOrEmpty(expdate))
                {
                    DateTime expreportdate;
                    bool f = DateTime.TryParse(expdate,out expreportdate);
                    if (!f)
                    {
                        errstr += "预计发单日期格式错误；";
                    }
                    else
                    {
                        expdate = expreportdate.ToString("yyyy-mm-dd");
                    }
                }
                DateTime createdate;
                bool dateb = DateTime.TryParse(item["CREATEDATE"].ToString(), out createdate);
                if (!dateb)
                {
                    errstr += "生成异常信息时间格式错误；";
                }
                if (string.IsNullOrEmpty(errstr))
                {
                    try
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("BARCODE", barcode);
                        ht.Add("TESTCODE", testcode);
                        ht.Add("EXCEPTIONTYPE", exctype);
                        ht.Add("EXCEPTIONREASON", reason);
                        ht.Add("EXPREPORTDATE", expdate);
                        ht.Add("STATUS", item["STATUS"].ToString() == "" ? "0" : item["STATUS"].ToString());
                        ht.Add("CREATEDATE", createdate);
                        list.Add(ht);
                        if (!string.IsNullOrEmpty(barcode))
                        {
                            //判断条码是否来自大众健康系统
                            DataTable dtBarcode = barcodeservice.CheckBarCode2(barcode);
                            if (dtBarcode != null && dtBarcode.Rows.Count > 0)
                            {
                                if (dtBarcode.Rows[0]["enterby"].ToString().Contains("大众平台"))
                                {
                                    Hashtable htapp = new Hashtable();
                                    htapp.Add("BARCODE", barcode);
                                    htapp.Add("CREATEDATE", createdate);
                                    htapp.Add("EXPREPORTDATE", expdate);
                                    htapp.Add("EXCEPTIONREASON", reason);
                                    string status = item["STATUS"].ToString() == "" ? "0" : item["STATUS"].ToString();
                                    string text=string.Empty;
                                    if (expdate == "1")
                                    {
                                        if (status == "0")
                                        {
                                            text = "正常迟发";
                                        }
                                        else
                                        {
                                            text = "取消迟发";
                                        }
                                    }
                                    else if (expdate=="2")
                                    {
                                        text = "退单";
                                    }
                                    htapp.Add("STATUS",text);
                                    listapp.Add(htapp);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        errstr = e.Message;
                    }
                }
            }
            return errstr;
        }
        /// <summary>  
        /// 移动文件  
        /// </summary>  
        /// <param name="olderpath">待移动的文件</param>  
        /// <param name="topath">新目录</param>  
        /// <param name="f">是否异常文件转移</param>
        private void moveJsonFile(string olderpath, string topath, bool f)
        {
            string msg = string.Empty;
            string folder = topath.Substring(0, topath.LastIndexOf("\\"));
            string filename = topath.Substring(topath.LastIndexOf("\\")+1);
            try
            {
                if (!Directory.Exists(folder))//若文件夹不存在则新建文件夹
                {
                    Directory.CreateDirectory(folder); //新建文件夹
                }
                if (File.Exists(topath))//目标文件夹存同名文件则先删除
                {
                    File.Delete(topath);
                }
                File.Move(olderpath, topath); //移动文件  .
                if (f)
                {
                    msg = string.Format("已将解析异常json文件[{0}]转移至【{1}】", filename, folder);
                }
                else
                {
                    msg = string.Format("[{0}]json文件[{1}]解析成功,已将源文件转移至【{2}】",DateTime.Now.ToString(), filename, folder);
                }
                SetTB(msg + "\n======================================================");
            }
            catch (Exception ex)
            {
                string s = f == true ? "失败" : "成功";
                msg = string.Format("[{0}]文件[{1}]解析{2}后,转移文件过程中出现异常[{2}]。", DateTime.Now.ToString(), filename, s, ex.Message);
                SetTB(msg + "\n======================================================");
            }
        }
        #endregion

        #region 实时记录
        private delegate void SetTBMethodInvok(string value);
        /// <summary>
        /// 写出返回的消息
        /// </summary>
        /// <param name="value"></param>
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

        #region 记录日志
        private readonly static string FileOrPath = Application.StartupPath + "\\Log\\JsonWatcher\\";
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
        #endregion
    }
}
