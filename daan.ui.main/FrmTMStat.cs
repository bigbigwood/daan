using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using daan.service.order;
using System.IO;
using System.Collections;

namespace daan.ui.main
{
    public partial class FrmTMStat : Form
    {
        static readonly OrdersService os = new OrdersService();
        bool b;
        int k = 0;
        private readonly System.Timers.Timer timer = new System.Timers.Timer();
        public FrmTMStat()
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
            //开始扫描
            try
            { 
                //查询未生成统计结果的预查询条件
                using (DataTable dt = os.GetPreSearchList())
                {
                    string strmessage = string.Empty;
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        strmessage = string.Format("{0}:未扫描到未生成查询结果的预查询请求", DateTime.Now);
                        SetTB(strmessage);
                    }
                    else
                    {
                        string searchid = string.Empty;
                        foreach (DataRow dr in dt.Rows)
                        {
                            searchid=dr["searchid"].ToString();
                            try
                            {
                                strmessage = String.Format("{0}: 预查询批次: 【{1}】扫描开始。", DateTime.Now, searchid);
                                SetTB(strmessage);
                                TMSTAT(dr);
                                strmessage = String.Format("{0}: 预查询批次: 【{1}】扫描成功。", DateTime.Now, searchid);
                                SetTB(strmessage);
                            }
                            catch(Exception ex)
                            {
                                strmessage = String.Format("{0}: 预查询批次: 【{1}】扫描失败。原因：{2}", DateTime.Now, searchid, ex.Message);
                                SetTB(strmessage);
                                //修改状态
                                Hashtable ht_F = new Hashtable();
                                ht_F.Add("searchid", searchid);
                                ht_F.Add("status", "2");
                                ht_F.Add("scantime", DateTime.Now.ToString());
                                ht_F.Add("reason", ex.Message);
                                os.SetPreSearchStatus(ht_F);
                                os.DeleteTMStatSearchRestult(Convert.ToDouble(searchid));
                                continue;
                            }
                        }
                    }
                }
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

        private void TMSTAT(DataRow dr)
        {
            string searchid = dr["searchid"].ToString();
            Hashtable ht_id = new Hashtable();
            ht_id.Add("searchid", searchid);
            if (os.SelectTMStatSearchResultListCount(ht_id) > 0)
            {
                os.DeleteTMStatSearchRestult(Convert.ToDouble(searchid));
            }
            Hashtable ht = GetPara(dr);
            DataTable dtList = os.QueryTMStatExportList(ht);//分组统计数据列表
            DataTable dtOperaLog = os.QueryOperationLog(ht);//操作日志记录
            DataTable dtSearchResult = dtOperaLog == null ? dtList : FillDataList(dtList, dtOperaLog);
            //将查询统计结果插入到TM统计结果表并修改预查询条件表中的生成状态
            if (dtSearchResult.Rows.Count > 0)
            {
                foreach (DataRow dr_result in dtSearchResult.Rows)
                {
                    dr_result["searchid"] = searchid;
                    os.InsertTMStatSearchRestult(dr_result);
                    k++;
                }
            }
            //修改状态
            Hashtable ht_F = new Hashtable();
            ht_F.Add("searchid", searchid);
            ht_F.Add("status", "1");
            ht_F.Add("scantime", DateTime.Now.ToString());
            ht_F.Add("reason", "");
            if (k == 0)
            {
                os.SetPreSearchStatus(ht_F);
            }
            else
            {
                if (k >= dtSearchResult.Rows.Count)
                {
                    os.SetPreSearchStatus(ht_F);
                }
                else
                {
                    os.DeleteTMStatSearchRestult(Convert.ToDouble(searchid));
                }
            }
        }

        /// <summary
        /// 计算各状态数量
        /// </summary>
        /// <param name="dtList"></param>
        /// <param name="dtOperaLog"></param>
        /// <returns></returns>
        private DataTable FillDataList(DataTable dtList, DataTable dtOperaLog)
        {
            string conditions;
            Hashtable htImport, htResult, htFinished, htPrint;
            #region 分类操作记录数据
            DataTable dtImport = OperaDataFilter(string.Format("modulename='{0}'", "单位批量上传"), dtOperaLog);
            DataTable dtResult = OperaDataFilter(string.Format("modulename='{0}'", "检查结果录入"), dtOperaLog);
            DataTable dtFinished = OperaDataFilter(string.Format("modulename='{0}' and content like '%{1}%'", "总检", "完成总检审核成功"), dtOperaLog);
            DataTable dtPrint = OperaDataFilter(string.Format("modulename='{0}'", "报告单集中打印"), dtOperaLog);
            #endregion
            try
            {
                foreach (DataRow dr in dtList.Rows)
                {
                    conditions = String.Format("ordercode='{0}' and dictlabid={1} and dictcustomerid={2} and section='{3}' and ordertestlst='{4}' and createdate='{5}' and (samplingdate='{6}' or samplingdate is null)",
                        dr["ordercode"].ToString(), dr["dictlabid"].ToString(), dr["dictcustomerid"].ToString(), dr["Section"].ToString(),
                        dr["ordertestlst"].ToString(), dr["createdate"].ToString(), dr["samplingdate"].ToString());
                    #region 分别获取各节点数和最后操作时间
                    htImport = GetCountandTime(conditions, dtImport);
                    dr["importCount"] = htImport["count"];
                    dr["importTime"] = htImport["lasttime"];

                    htResult = GetCountandTime(conditions, dtResult);
                    dr["resultCount"] = htResult["count"];
                    dr["resultTime"] = htResult["lasttime"];

                    htFinished = GetCountandTime(conditions, dtFinished);
                    dr["finishedCount"] = htFinished["count"];
                    dr["finishedTime"] = htFinished["lasttime"];

                    htPrint = GetCountandTime(conditions, dtPrint);
                    dr["printCount"] = htPrint["count"];
                    dr["printTime"] = htPrint["lasttime"];
                    #endregion
                }
            }
            catch (Exception ee)
            {
                //messageshow("加载数据出错，异常信息：" + ee.Message);
            }
            return dtList;
        }
        private static DataTable OperaDataFilter(string conditions, DataTable dtsource)
        {
            DataTable dtreturn;
            DataRow[] dr = dtsource.Select(conditions);
            if (dr.Count() > 0)
                dtreturn = dr.CopyToDataTable();
            else
                dtreturn = null;
            return dtreturn;
        }
        private static Hashtable GetCountandTime(string conditions, DataTable dt)
        {
            Hashtable ht = new Hashtable();
            if (dt == null)
            {
                ht["count"] = 0; ht["lasttime"] = "";
                return ht;
            }
            DataRow[] dr = dt.Select(conditions, "logdate desc");
            if (dr.Count() > 0)
            {
                ht["count"] = dr.Count();
                ht["lasttime"] = dr[0]["logdate"].ToString();
            }
            else
            {
                ht["count"] = 0; ht["lasttime"] = "";
            }
            return ht;
        }
        private Hashtable GetPara(DataRow dr)
        {
            Hashtable ht = new Hashtable();
            ht.Add("DateStart", dr["begindate"] == "" ? null : dr["begindate"]);
            ht.Add("DateEnd", dr["enddate"] == "" ? null : Convert.ToDateTime(dr["enddate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            ht.Add("labid", dr["dictlabid"]);
            ht.Add("dictcustomerid", dr["dictcustomerid"]);
            ht.Add("Section",dr["section"].ToString());
            return ht;
        }

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
                timer.Interval = 1000 * num;
            }
            else
            {
                timer.Interval = 1000;
            }
            //设置是否重复计时，如果该属性设为False,则只执行timer_Elapsed方法一次。
            timer.AutoReset = true;
            btnBegan.Enabled = false;
            string strmsg = string.Format(">>>系统已启动【TM统计查询】： {0}", DateTime.Now);
            SetTB(strmsg);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            b = true;
            timer.AutoReset = false;
            btnBegan.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            FrmTMStat frm = new FrmTMStat();
            frm.Dispose();
            b = true;
            timer.AutoReset = false;
            string strmsg = string.Format(">>>退出系统时间【TM统计查询】：{0}", DateTime.Now);
            SetTB(strmsg);
            Close();
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
        private readonly static string FileOrPath = Application.StartupPath + "\\Log\\TMStat\\";
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
            //删除25天前的备份文件.pdf文件
            //string FilePdfPath = Application.StartupPath + "\\PdfFile\\";
            //string[] fileRar = Directory.GetFiles(FilePdfPath, "*.pdf");
            //for (int i = 0; i < fileRar.Length; i++)
            //{
            //    FileInfo fi = new FileInfo(fileRar[i]);
            //    if (fi.LastWriteTime < DateTime.Now.AddDays(-25)) //创建日期：file.CreationTime + 上次访问日期：file.LastAccessTime +  上次写入日期：file.LastWriteTime
            //    {
            //        Console.WriteLine(DateTime.Now + "-=|删除:" + fileRar[i]);
            //        fi.Delete();
            //    }
            //}
        }

    }
}
