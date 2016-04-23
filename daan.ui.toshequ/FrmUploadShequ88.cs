using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Configuration;
using daan.ui.main.LisToShequ88;
using System.Diagnostics;
using System.Security.Cryptography;

namespace daan.ui.LisToshequ
{
    public partial class FrmUploadShequ88 : Form
    { 

        bool b = false;
        string ordernum = "";
        private System.Timers.Timer timer = new System.Timers.Timer();

        public FrmUploadShequ88()
        {
            InitializeComponent();
            string time =  ConfigurationManager.AppSettings["Time"].ToString();
            this.tbTime.Text = time;
            ContextMenu m = new ContextMenu();
            tbTime.ContextMenu = m;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
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
                DataTable dt = new SqlService().GetGetPEEntity();
                Stopwatch sw = new Stopwatch();
                PhysicalExaminationServiceSoapClient client = new PhysicalExaminationServiceSoapClient();
                sw.Start();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //如果身份证号和电话号码其中一个不为空就传输
                        if (dt.Rows[i]["patientphone"].ToString() != "" || dt.Rows[i]["idnum"].ToString() != "")
                        {
                            PEEntity model = new PEEntity();
                            #region
                            if (dt.Rows[i]["age"].ToString() != null)
                            {
                                string userage =  dt.Rows[i]["age"].ToString();
                                if (userage.Equals("成人"))
                                {
                                    model.Age = 999;
                                }
                                else if (userage.Equals("婴儿"))
                                {
                                    model.Age = 998;
                                }
                                else if (userage.IndexOf("岁") > 0)
                                {
                                    model.Age = Convert.ToInt32(userage.Substring(0, userage.ToLower().IndexOf("岁") + "岁".Length - 1));
                                }
                                else if (userage.IndexOf("月") > 0)
                                {
                                    model.Age = 998;
                                }
                                else if (userage.IndexOf("日") > 0)
                                {
                                    model.Age = 998;
                                }
                                else if (userage.IndexOf("时") > 0)
                                {
                                    model.Age = 998;
                                }
                                else
                                {
                                    model.Age = Convert.ToInt32(userage.Split('岁')[0]);
                                }
                            }
                            ordernum = dt.Rows[i]["barcode"].ToString();
                            model.IsSend = 1;
                            model.BarCode = dt.Rows[i]["barcode"].ToString(); //"BARCODE" + DateTime.Now.AddDays(i).ToString("yyyyMMddhhssss"); //   //条码
                            model.LDoctor = dt.Rows[i]["LDoctor"].ToString();    //送检医生
                            model.LOrganization = dt.Rows[i]["LOrganization"].ToString();   //送检单位
                            model.ProductName = dt.Rows[i]["ProductName"].ToString(); //套餐名称
                            model.LResult = dt.Rows[i]["LResult"].ToString() + "|"; //送检结果
                            model.Ltime = DateTime.Now.AddMonths(i).ToString("yyyy-MM-dd HH:mm:ss");   //送检时间
                            model.OrderNo = dt.Rows[i]["barcode"].ToString(); //"PSC" + DateTime.Now.ToString("yyyyMMddhhssss"); //    //订单编号
                            model.Remark = dt.Rows[i]["remark"].ToString();                     //备注
                            model.STime = DateTime.Now.AddDays(-1).AddHours(i).ToString("yyyy-MM-dd HH:mm:ss");
                            //model.Reports = BuildeReport("340000200000", "20651");
                            model.Reports = BuildeReport(dt.Rows[i]["barcode"].ToString(), dt.Rows[i]["customerid"].ToString());    //检验报告列表
                            string invoker = "高新达安";                                        //调用者标识
                            string user = "";
                            //if (dt.Rows[i]["patientphone"].ToString() != "" )
                            //{
                            //    user = "2&" + dt.Rows[i]["patientname"].ToString() + "&" + dt.Rows[i]["patientphone"].ToString();    //体检人+手机号 会验证7位数以上 
                            //}
                            //else
                            //{
                            //    user = "1&" + dt.Rows[i]["patientname"].ToString() + "&" + dt.Rows[i]["idnum"].ToString();    //体检人+身份证号 会验证7位数以上
                            //}
                            user = "1024&" + dt.Rows[i]["patientname"].ToString() + "&" + dt.Rows[i]["patientphone"].ToString() + "&" + dt.Rows[i]["idnum"].ToString() + "&";
                            string authentic = "Invoker=" + invoker + "|BarCode=" + model.BarCode + "|User=" + user;//whatever you want to encrypt
                            //加密authentic参数的公钥
                            string publicauthentickey = "<RSAKeyValue><Modulus>psgX2qvebnjvgHdksSMGacyWj0Ws5oe1X91txkmUtVsFsKhg3HDQVzvq7M+QFKFq8RhHGwVX+949PEnGM4nY7MmynGojV/7kEhI5Rd4vPrUmYS4yUK0f/WPdkgzALb94iYFo0u2CBV+hJKYFY0T7LYg6YJmW1aiVFeTvcGU4YIM=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
                            //加密调用者invoker的公钥
                            string publicinvokerkey = "<RSAKeyValue><Modulus>v6t9eE/+4A4NdVQQUTMxCrSCKHq3bWTwkU3DECuEh3Ddp1iNbECmcYnVOQBwJVwh8ZGDIp7yxUQ2yHbPv0yf2davMuuPNCwx5laXRc7vNbp9F6A6/ijaxMFLxypigYzzcY8vrTblHfTK4q5uxj+Vy2huyarwPpNM/xSNoLK05Ls=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
                            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                            byte[] cipherbytes;
                            rsa.FromXmlString(publicauthentickey);
                            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(authentic), false); //使用UTF-8编码
                            authentic = Convert.ToBase64String(cipherbytes);

                            cipherbytes = null;
                            rsa.FromXmlString(publicinvokerkey);
                            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(invoker), false);
                            invoker = Convert.ToBase64String(cipherbytes);

                            InvokeResult result = client.EditPhysicalExamination(invoker, user, model, authentic);  //数据传输
                            string str = (i + 1) + " 传输信息：" + result.MCode + " " + result.Succeed; //"  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                            // 修改Lis上传标志Sqflag 1未上传 2 已上传
                            if (result.MCode == "" && result.Succeed == true)//如果上传成功标识改为1
                            {
                                Hashtable htorder = new Hashtable();
                                htorder.Add("sqflag", 2);
                                htorder.Add("barcode", dt.Rows[i]["barcode"].ToString());
                                bool falg = new SqlService().EditSqflag(htorder);
                                if (falg)
                                {
                                    str += "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + " 条码号：" + dt.Rows[i]["barcode"].ToString() + " 上传成功！";
                                }
                                else
                                {
                                    str += "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + " 条码号：" + dt.Rows[i]["barcode"].ToString() + " 上传失败！";
                                }
                            }
                            else
                            {
                                str += "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + " 条码号：" + dt.Rows[i]["barcode"].ToString() + " 上传失败！";
                                Hashtable htorder = new Hashtable();
                                htorder.Add("sqflag", 3);
                                htorder.Add("barcode", dt.Rows[i]["barcode"].ToString());
                                new SqlService().EditSqflag(htorder);
                            }
                            SetTB(str);
                            #endregion
                        }
                        else
                        {
                           // 如果身份证号和电话号码都为空就直接改状态
                            Hashtable htorder = new Hashtable();
                            htorder.Add("sqflag", 2);
                            htorder.Add("barcode", dt.Rows[i]["barcode"].ToString());
                            bool falg = new SqlService().EditSqflag(htorder);
                            string message = "";
                            if (falg)
                            {
                                message = "电话和身份证为空，" + " 条码号：" + dt.Rows[i]["barcode"].ToString() + "传输取消！";
                            }
                            else
                            {
                                message = "电话和身份证为空，" + " 条码号：" + dt.Rows[i]["barcode"].ToString() + "修改出错！";
                            }
                            SetTB(message);
                        }
                    }
                    sw.Stop();
                    string str1 = "\n总运行时间：" + sw.Elapsed + "\n测量实例得出的总运行时间(毫秒为单位)：" + sw.ElapsedMilliseconds + "\n总运行时间(计时器刻度标识)：" + sw.ElapsedTicks + "\n";
                    SetTB(str1);
                    //设置timer可用
                    timer.Start();
                }
                else
                {
                    string str2 = "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":  没有传输的数据！\n";
                    SetTB(str2);
                }
                #endregion
            }
            catch (Exception ex)
            {
                string strmessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  条码号：" + ordernum + " " + ex.Message;
                SetTB(strmessage);
                CreateErrorLog(strmessage);
                if (strmessage.IndexOf("未能找到文件") <= 0)
                {
                    Hashtable htorder = new Hashtable();
                    htorder.Add("sqflag", 3);
                    htorder.Add("barcode", ordernum);
                    new SqlService().EditSqflag(htorder);
                }
                //MessageBox.Show(ex.Message, "Lis数据传输");
            }
            finally
            {
                timer.Start();
            }
        }


        ///// <summary>
        ///// 构建体检报告数据
        ///// </summary>
        ///// <returns></returns>
        private static PEReportEntity[] BuildeReport(string ordernum, string customerid)
        {
            //根据订单找到检测报告
            List<PEReportEntity> list = new List<PEReportEntity>();
            //OrdersService orderservie = new OrdersService();
            Hashtable ht = new Hashtable();
            ht.Add("strKey", ordernum);
            DataTable dt = new SqlService().GetPEReportEntity(ht);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region
                    PEReportEntity pereport = new PEReportEntity();
                    pereport.ATime = DateTime.Now.AddHours(-i).ToString("yyyy-MM-dd HH:mm:ss");  //审核时间
                    pereport.Auditor = dt.Rows[i]["Auditor"].ToString();    //审核人
                    pereport.Inspector = dt.Rows[i]["Inspector"].ToString();// "检验人";                               
                    pereport.ITime = DateTime.Now.AddMinutes(i).ToString("yyyy-MM-dd HH:mm:ss");  //检验时间
                    pereport.LRoom = "";// "送检医生";
                    pereport.STime = DateTime.Now.AddDays(-1).AddHours(i).ToString("yyyy-MM-dd HH:mm:ss");
                    pereport.Remark = dt.Rows[i]["Remark"].ToString();// "体检报告上的备注信息";  //
                    pereport.SpecialInfo = dt.Rows[i]["SpecialInfo"].ToString(); // "特殊信息";  //"
                    pereport.Sstate = dt.Rows[i]["Sstate"].ToString() == "" ? "正常" : dt.Rows[i]["Sstate"].ToString();  //"正常"：dt.Rows[i]["Sstate"].ToString();// "样本状态:正常";  //
                    pereport.Stype = dt.Rows[i]["Stype"].ToString();// "样本类型";  //
                    pereport.Opinion = dt.Rows[i]["Opinion"].ToString();   //体检报告建议
                    pereport.Proposal = dt.Rows[i]["Proposal"].ToString();  //诊断建议

                    Hashtable htitem = new Hashtable();
                    DataTable dtItems = null;
                    //根据ordernum 查找检测项目
                    if (dt.Rows[i]["reportoption"].ToString() == "0")
                    {
                        htitem.Add("strKey", dt.Rows[i]["specimenreportid"].ToString());
                        dtItems = new SqlService().GetGetPEItemEntityOne(htitem);
                    }
                    else if (dt.Rows[i]["reportoption"].ToString() == "1")
                    {
                        htitem.Add("strKey", ordernum);
                        dtItems = new SqlService().GetPEItemEntityTwo(htitem);
                    }
                    else if (dt.Rows[i]["reportoption"].ToString() == "2")
                    {
                        htitem.Add("strKey", dt.Rows[i]["specimenreportid"].ToString());
                        dtItems = new SqlService().GetPEItemEntityThree(htitem);
                    }

                    PEItemEntity[] peItems = new PEItemEntity[dtItems.Rows.Count];
                    if (dtItems.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtItems.Rows.Count; j++)
                        {
                            PEItemEntity e = new PEItemEntity();
                            //检验项目验证异常
                            e.ICode = dtItems.Rows[j]["ICode"].ToString();  // 检验项目名称 "中性粒细胞百分比（Neu%）";"1201029";//
                            e.Manner = dtItems.Rows[j]["Manner"].ToString();  //"检验方法";// 
                            e.RefValue = dtItems.Rows[j]["RefValue"].ToString(); //参考值1\n参考值2\n参考值3
                            e.Remark = dtItems.Rows[j]["Remark"].ToString(); //检测项目备注
                            if (dtItems.Rows[j]["Remind"].ToString() != "")
                            {
                                e.Remind = dtItems.Rows[j]["Remind"].ToString(); //提示;"H";//
                            }
                            else
                            {
                                e.Remind = "N";
                            }
                            e.Unit = dtItems.Rows[j]["Unit"].ToString(); //单位
                            e.Value = dtItems.Rows[j]["Value"].ToString(); //体检结果值
                            peItems[j] = e;
                        }
                    }
                    pereport.Items = peItems;   //检测项目

                    //传送体检报告单
                    string hostName = ConfigurationManager.AppSettings["hostName"].ToString();
                    string passWord = ConfigurationManager.AppSettings["passWord"].ToString();
                    string userName = ConfigurationManager.AppSettings["userName"].ToString();
                    CmdNetUse(hostName, passWord, userName);
                    if (dt.Rows[i]["reportoption"].ToString() == "1") //病理报告
                    {
                        hostName += "\\pdfdownload\\lispdf\\" + customerid + "\\" + dt.Rows[i]["specimenreportid"].ToString() + ".PDF";
                        //pereport.Files = new FileEntity[2];
                        FileEntity[] fileEntity = new FileEntity[2];
                        FileEntity f = new FileEntity();
                        f.Content = ConvertStreamToByteBuffer(@hostName);
                        f.Extension = "pdf";
                        f.Remark = "这是第一张PDF文件";
                        f.Type = "1";
                        fileEntity[0] = f;
                        Hashtable htsh = new Hashtable();
                        htsh.Add("strKey", ordernum);
                        DataTable dtable = new SqlService().GetFilename(htsh);
                        for (int k = 0; k < dtable.Rows.Count; k++)
                        {
                            hostName = dtable.Rows[k]["filename"].ToString();
                        }
                        FileEntity fi = new FileEntity();
                        fi.Content = ConvertStreamToByteBuffer(@hostName);
                        fi.Extension = "jpg";
                        fi.Remark = "这是第一张jpg文件";
                        fi.Type = "2";
                        fileEntity[1] = fi;

                    }
                    else   //常规 微生物报告
                    {
                        hostName += "\\pdfdownload\\lispdf\\" + customerid + "\\" + dt.Rows[i]["specimenreportid"].ToString() + ".PDF";
                        //hostName = "E:\\pdfdownload\\lispdf\\202083\\203490186.pdf";
                        pereport.Files = new FileEntity[]{
                        new FileEntity(){     

                        Content=ConvertStreamToByteBuffer(@hostName),
                        Extension="pdf",
                        Remark="这是第一张PDF文件",
                        Type="1"
                          }
                        };
                    }
                    list.Add(pereport);
                    #endregion
                }
            }

            return list.ToArray();
        }

        public static void CmdNetUse(string hostName, string passWord, string userName)
        {
            // 实例一个Process类,启动一个独立进程 
            Process p = new Process();
            // 设定程序名 
            p.StartInfo.FileName = "cmd.exe";
            // 关闭Shell的使用 
            p.StartInfo.UseShellExecute = false;
            // 重定向标准输入 
            p.StartInfo.RedirectStandardInput = true;
            // 重定向标准输出 
            p.StartInfo.RedirectStandardOutput = true;
            //重定向错误输出 
            p.StartInfo.RedirectStandardError = true;
            // 设置不显示窗口 
            p.StartInfo.CreateNoWindow = true;

            // 启动进程并执行命令 
            p.Start();

            string str_comm = @"net use " + hostName + " \"" + passWord + "\" /user:" + userName + "";

            p.StandardInput.WriteLine(str_comm);
            p.StandardInput.WriteLine("exit");

            string strRst = p.StandardOutput.ReadToEnd();

            p.StandardInput.Close();
            p.Close();
            //p.Dispose();
            if (strRst.IndexOf("命令成功完成") == -1)
            {
                //MessageBox.Show("没有与文件服务器成功建立连接，会造成无法获取到需要的图像文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                string message = "没有与文件服务器成功建立连接，会造成无法获取到需要的图像文件！";
                FrmUploadShequ88 frm = new FrmUploadShequ88();
                frm.SetTB(message);
                CreateErrorLog(message);
            }
        }


        ///   <summary>
        ///  把给定的文件流转换为二进制字节数组。
        ///   </summary>
        ///   <returns> </returns>
        public static byte[] ConvertStreamToByteBuffer(string path)
        {
            //path = @"\\202.116.104.250\信息部总部文件\\统计结果.png";
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


        private delegate void SetTBMethodInvok(string value);
        private void SetTB(string value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetTBMethodInvok(SetTB), value);
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
                this.tbxMessage.SelectionStart = this.tbxMessage.Text.Length;
                this.tbxMessage.ScrollToCaret();
            }
        }

        /// <summary>
        /// 验证文本框只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbTime_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
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
                            MessageBox.Show("不能以0为开头！", "Lis数据传输", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        e.Handled = false;
                        return;
                    }
                    else
                    {
                        MessageBox.Show("只能输入3位数字！", "Lis数据传输", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }
            else
            {
                MessageBox.Show("只能输入数字！", "Lis数据传输", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }





        public static string FileOrPath = Application.StartupPath + "\\Log\\";
        public static string m_fileName = FileOrPath + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
            m_fileName = FileOrPath + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (File.Exists(m_fileName))
            {
                ///如果日志文件已经存在，则直接写入已有的日志文件    
                StreamWriter sr = File.AppendText(FileName);
                sr.WriteLine("\n");
                sr.WriteLine(message);
                sr.Close();
            }
            else
            {
                ///创建日志文件           
                StreamWriter sr = File.CreateText(FileName);
                sr.WriteLine("\n");
                sr.WriteLine(message);
                sr.Close();
            }
            //删除25天前的备份文件.pdf文件
            //string FilePdfPath = Application.StartupPath + "\\PdfFile\\";
            //string[] fileRar = Directory.GetFiles(FilePdfPath, "*.pdf");
            //for (int i = 0; i < fileRar.Length; i++)
            //{
            //    FileInfo fi = new FileInfo(fileRar[i]);
            //    if (fi.LastWriteTime < DateTime.Now.AddDays(-2))
            //    {
            //        Console.WriteLine(DateTime.Now + "-=|删除:" + fileRar[i]);
            //        fi.Delete();
            //    }
            //}
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
            int num = Convert.ToInt32(this.tbTime.Text.Trim() == "" ? 0 : Convert.ToInt32(this.tbTime.Text.Trim()));
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
            this.btnBegan.Enabled = false;
            string strmsg = string.Format(">>>系统已启动： {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SetTB(strmsg);
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

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
            this.btnBegan.Enabled = true;
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
            string strmsg = string.Format(">>>退 出 时 间：{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SetTB(strmsg);
            this.Close();
        }
    }
}
